using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class LuongService
    {
        // Tinh bang luong thang cho moi nhan vien co bang cham cong trong thang.
        //   Luong theo cong = LCB / 26 * so ngay cong
        //   Tien phu troi   = so gio phu troi * (LCB / 26 / 8) * 1.5
        //   Phu cap         = PC chuc vu + PC luong + PC doc hai (theo quyet dinh dang hieu luc)
        //   Thuong / phat   = quyet dinh khen thuong (BANG_LUONG) / ky luat (TRU_LUONG) ky trong thang
        //   Bao hiem        = LCB * ty le BHXH, BHYT (chi nhan vien co ho so bao hiem - hop dong dai han)
        // Nhan vien thu viec: LCB = luong thu viec, khong phu cap, khong bao hiem.
        // Tinh lai cung thang se xoa bang luong cu cua thang do (trong cung transaction).
        public static KetQuaXuLy TinhLuongThang(int thang, int nam)
        {
            if (thang < 1 || thang > 12 || nam < 2000) return KetQuaXuLy.Fail("Thang/nam khong hop le");
            DateTime dauThang = new DateTime(nam, thang, 1);
            DateTime cuoiThang = dauThang.AddMonths(1).AddDays(-1);

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        DataTable dsCong = Db.QueryInTransaction(cn, tran,
                            @"SELECT cc.MaNV, cc.SoNgayCong, nv.MaPB FROM BangChamCong cc
                              JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                              WHERE cc.Thang=@t AND cc.Nam=@n", Db.P("@t", thang), Db.P("@n", nam));
                        if (dsCong.Rows.Count == 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Chua co bang cham cong thang " + thang + "/" + nam); }

                        Db.ExecuteInTransaction(cn, tran, "DELETE FROM BangLuong WHERE Thang=@t AND Nam=@n",
                            Db.P("@t", thang), Db.P("@n", nam));

                        foreach (DataRow r in dsCong.Rows)
                        {
                            string maNV = r["MaNV"].ToString();
                            decimal soCong = (decimal)r["SoNgayCong"];
                            var bl = TinhMotNhanVien(cn, tran, maNV, r["MaPB"].ToString(), soCong, dauThang, cuoiThang);
                            if (bl == null)
                            { tran.Rollback(); return KetQuaXuLy.Fail("Nhan vien " + maNV + " chua co quyet dinh luong / tuyen dung"); }

                            Db.ExecuteInTransaction(cn, tran,
                                @"INSERT INTO BangLuong(MaBL, MaNV, Thang, Nam, LuongCoBan, SoNgayCong, LuongTheoCong, PhuCapChucVu,
                                                        PhuCapLuong, PhuCapDocHai, SoGioPhuTroi, TienPhuTroi, TienThuong, TienPhat,
                                                        TruBHXH, TruBHYT, ThucLanh)
                                  VALUES(@ma,@nv,@t,@n,@lcb,@cong,@ltc,@pccv,@pcl,@pcdh,@gio,@tpt,@th,@ph,@xh,@yt,@tl)",
                                Db.P("@ma", "BL" + (nam % 100).ToString("00") + thang.ToString("00") + maNV),
                                Db.P("@nv", maNV), Db.P("@t", thang), Db.P("@n", nam),
                                Db.P("@lcb", bl.LuongCoBan), Db.P("@cong", soCong), Db.P("@ltc", bl.LuongTheoCong),
                                Db.P("@pccv", bl.PhuCapChucVu), Db.P("@pcl", bl.PhuCapLuong), Db.P("@pcdh", bl.PhuCapDocHai),
                                Db.P("@gio", bl.SoGioPhuTroi), Db.P("@tpt", bl.TienPhuTroi), Db.P("@th", bl.TienThuong),
                                Db.P("@ph", bl.TienPhat), Db.P("@xh", bl.TruBHXH), Db.P("@yt", bl.TruBHYT), Db.P("@tl", bl.ThucLanh));
                        }

                        tran.Commit();
                        return KetQuaXuLy.Ok("Tinh luong thang " + thang + "/" + nam + " thanh cong cho " + dsCong.Rows.Count + " nhan vien");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi tinh luong: " + ex.Message);
                    }
                }
            }
        }

        private class DongLuong
        {
            public decimal LuongCoBan, LuongTheoCong, PhuCapChucVu, PhuCapLuong, PhuCapDocHai;
            public decimal SoGioPhuTroi, TienPhuTroi, TienThuong, TienPhat, TruBHXH, TruBHYT, ThucLanh;
        }

        private static DongLuong TinhMotNhanVien(SqlConnection cn, SqlTransaction tran, string maNV, string maPB,
            decimal soCong, DateTime dauThang, DateTime cuoiThang)
        {
            var bl = new DongLuong();

            DataTable qdl = Db.QueryInTransaction(cn, tran,
                @"SELECT TOP 1 LuongCoBan, PhuCapLuong, PhuCapDocHai FROM QuyetDinhLuong
                  WHERE MaNV=@m AND NgayHieuLuc<=@c ORDER BY NgayHieuLuc DESC", Db.P("@m", maNV), Db.P("@c", cuoiThang));
            if (qdl.Rows.Count > 0)
            {
                bl.LuongCoBan = (decimal)qdl.Rows[0]["LuongCoBan"];
                bl.PhuCapLuong = (decimal)qdl.Rows[0]["PhuCapLuong"];
                bl.PhuCapDocHai = (decimal)qdl.Rows[0]["PhuCapDocHai"];

                object pccv = Db.ScalarInTransaction(cn, tran,
                    @"SELECT TOP 1 PhuCapChucVu FROM QuyetDinhChucVu
                      WHERE MaNV=@m AND NgayHieuLuc<=@c ORDER BY NgayHieuLuc DESC", Db.P("@m", maNV), Db.P("@c", cuoiThang));
                bl.PhuCapChucVu = pccv == null ? 0 : (decimal)pccv;

                DataTable bh = Db.QueryInTransaction(cn, tran,
                    "SELECT TyLeBHXH, TyLeBHYT FROM HoSoBaoHiem WHERE MaNV=@m AND NgayLap<=@c",
                    Db.P("@m", maNV), Db.P("@c", cuoiThang));
                if (bh.Rows.Count > 0)
                {
                    bl.TruBHXH = Math.Round(bl.LuongCoBan * (decimal)bh.Rows[0]["TyLeBHXH"] / 100, 0);
                    bl.TruBHYT = Math.Round(bl.LuongCoBan * (decimal)bh.Rows[0]["TyLeBHYT"] / 100, 0);
                }
            }
            else
            {
                object ltv = Db.ScalarInTransaction(cn, tran,
                    "SELECT LuongThuViec FROM QuyetDinhTuyenDung WHERE MaNV=@m", Db.P("@m", maNV));
                if (ltv == null) return null;
                bl.LuongCoBan = (decimal)ltv;
            }

            bl.LuongTheoCong = Math.Round(bl.LuongCoBan / QuyDinh.NgayCongChuan * soCong, 0);

            object gio = Db.ScalarInTransaction(cn, tran,
                @"SELECT ISNULL(SUM(ct.SoGio), 0) FROM ChiTietPhuTroi ct
                  JOIN PhieuPhuTroi p ON ct.SoPhieu = p.SoPhieu
                  WHERE ct.MaNV=@m AND p.NgayPhuTroi BETWEEN @d AND @c",
                Db.P("@m", maNV), Db.P("@d", dauThang), Db.P("@c", cuoiThang));
            bl.SoGioPhuTroi = (decimal)gio;
            decimal luongGio = bl.LuongCoBan / QuyDinh.NgayCongChuan / QuyDinh.GioMotNgay;
            bl.TienPhuTroi = Math.Round(bl.SoGioPhuTroi * luongGio * QuyDinh.HeSoPhuTroi, 0);

            object thuongCaNhan = Db.ScalarInTransaction(cn, tran,
                @"SELECT ISNULL(SUM(SoTien), 0) FROM QuyetDinhKhenThuong
                  WHERE DoiTuong=@cn AND MaNV=@m AND HinhThucChi=@bl AND NgayKy BETWEEN @d AND @c",
                Db.P("@cn", QuyDinh.CaNhan), Db.P("@m", maNV), Db.P("@bl", QuyDinh.BangLuong),
                Db.P("@d", dauThang), Db.P("@c", cuoiThang));
            // Thuong tap the qua bang luong: chia deu cho nhan vien dang lam cua phong ban.
            object tongTapThe = Db.ScalarInTransaction(cn, tran,
                @"SELECT ISNULL(SUM(SoTien), 0) FROM QuyetDinhKhenThuong
                  WHERE DoiTuong=@tt AND MaPB=@pb AND HinhThucChi=@bl AND NgayKy BETWEEN @d AND @c",
                Db.P("@tt", QuyDinh.TapThe), Db.P("@pb", maPB), Db.P("@bl", QuyDinh.BangLuong),
                Db.P("@d", dauThang), Db.P("@c", cuoiThang));
            object soNVPhong = Db.ScalarInTransaction(cn, tran,
                "SELECT COUNT(*) FROM NhanVien WHERE MaPB=@pb AND TrangThai<>@nghi",
                Db.P("@pb", maPB), Db.P("@nghi", QuyDinh.NghiViec));
            decimal thuongTapThe = Convert.ToInt32(soNVPhong) == 0 ? 0 : (decimal)tongTapThe / Convert.ToInt32(soNVPhong);
            bl.TienThuong = Math.Round((decimal)thuongCaNhan + thuongTapThe, 0);

            object phat = Db.ScalarInTransaction(cn, tran,
                @"SELECT ISNULL(SUM(SoTienPhat), 0) FROM QuyetDinhKyLuat
                  WHERE DoiTuong=@cn AND MaNV=@m AND HinhThucThu=@tl AND NgayKy BETWEEN @d AND @c",
                Db.P("@cn", QuyDinh.CaNhan), Db.P("@m", maNV), Db.P("@tl", QuyDinh.TruLuong),
                Db.P("@d", dauThang), Db.P("@c", cuoiThang));
            bl.TienPhat = (decimal)phat;

            bl.ThucLanh = bl.LuongTheoCong + bl.PhuCapChucVu + bl.PhuCapLuong + bl.PhuCapDocHai + bl.TienPhuTroi
                        + bl.TienThuong - bl.TienPhat - bl.TruBHXH - bl.TruBHYT;
            return bl;
        }

        public static DataTable DsBangLuong(int thang, int nam)
            => Db.Query(@"SELECT bl.MaNV, nv.HoTen, bl.LuongCoBan, bl.SoNgayCong, bl.LuongTheoCong,
                                 bl.PhuCapChucVu + bl.PhuCapLuong + bl.PhuCapDocHai AS TongPhuCap,
                                 bl.SoGioPhuTroi, bl.TienPhuTroi, bl.TienThuong, bl.TienPhat,
                                 bl.TruBHXH, bl.TruBHYT, bl.ThucLanh
                          FROM BangLuong bl JOIN NhanVien nv ON bl.MaNV = nv.MaNV
                          WHERE bl.Thang=@t AND bl.Nam=@n ORDER BY bl.MaNV", Db.P("@t", thang), Db.P("@n", nam));
    }
}
