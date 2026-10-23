using System;
using System.Data;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class HopDongService
    {
        // Ky hop dong chinh thuc sau khi het thoi gian thu viec. Trong 1 transaction:
        // hop dong + quyet dinh luong + quyet dinh chuc vu (neu co) + ho so BHYT/BHXH (neu dai han)
        // + chuyen nhan vien sang "Chinh thuc".
        public static KetQuaXuLy KyHopDong(ThongTinHopDong hd)
        {
            if (hd == null || string.IsNullOrWhiteSpace(hd.MaNV)) return KetQuaXuLy.Fail("Chon nhan vien ky hop dong");
            if (hd.LoaiHD != QuyDinh.DaiHan && hd.LoaiHD != QuyDinh.NganHan) return KetQuaXuLy.Fail("Loai hop dong khong hop le");
            if (hd.LoaiHD == QuyDinh.NganHan && (hd.NgayKetThuc == null || hd.NgayKetThuc <= hd.NgayBatDau))
                return KetQuaXuLy.Fail("Hop dong ngan han phai co ngay ket thuc sau ngay bat dau");
            if (hd.LuongCoBan <= 0) return KetQuaXuLy.Fail("Luong co ban phai lon hon 0");
            if (hd.PhuCapLuong < 0 || hd.PhuCapDocHai < 0 || hd.PhuCapChucVu < 0) return KetQuaXuLy.Fail("Phu cap khong duoc am");
            if (hd.LoaiHD == QuyDinh.DaiHan && (string.IsNullOrWhiteSpace(hd.SoBHXH) || string.IsNullOrWhiteSpace(hd.SoBHYT)))
                return KetQuaXuLy.Fail("Hop dong dai han phai lap ho so BHXH va BHYT");
            if (string.IsNullOrWhiteSpace(hd.NguoiKy)) return KetQuaXuLy.Fail("Nhap nguoi ky");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        DataTable dt = Db.QueryInTransaction(cn, tran,
                            @"SELECT nv.TrangThai, qd.NgayKetThucTV FROM NhanVien nv
                              JOIN QuyetDinhTuyenDung qd ON nv.MaNV = qd.MaNV WHERE nv.MaNV=@m", Db.P("@m", hd.MaNV));
                        if (dt.Rows.Count == 0)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Nhan vien chua co quyet dinh tuyen dung"); }
                        if (dt.Rows[0]["TrangThai"].ToString() != QuyDinh.ThuViec)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Chi nhan vien dang thu viec moi duoc ky hop dong chinh thuc"); }
                        DateTime ketThucTV = (DateTime)dt.Rows[0]["NgayKetThucTV"];
                        if (hd.NgayBatDau.Date <= ketThucTV)
                        { tran.Rollback(); return KetQuaXuLy.Fail("Hop dong chi bat dau sau khi het thoi gian thu viec (" + ketThucTV.ToString("dd/MM/yyyy") + ")"); }

                        string soHD = QuyDinh.SinhMa("HD");
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO HopDongLaoDong(SoHD, MaNV, LoaiHD, NgayKy, NgayBatDau, NgayKetThuc, NoiDung, TrangThai)
                              VALUES(@hd,@m,@l,@nk,@bd,@kt,@nd,@tt)",
                            Db.P("@hd", soHD), Db.P("@m", hd.MaNV), Db.P("@l", hd.LoaiHD), Db.P("@nk", DateTime.Today),
                            Db.P("@bd", hd.NgayBatDau.Date), Db.P("@kt", hd.LoaiHD == QuyDinh.NganHan ? (object)hd.NgayKetThuc.Value.Date : null),
                            Db.P("@nd", hd.NoiDung), Db.P("@tt", QuyDinh.HieuLuc));

                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO QuyetDinhLuong(SoQD, MaNV, NgayKy, NgayHieuLuc, LuongCoBan, PhuCapLuong, PhuCapDocHai, NguoiKy, GhiChu)
                              VALUES(@qd,@m,@nk,@hl,@lcb,@pcl,@pcdh,@ky,@gc)",
                            Db.P("@qd", QuyDinh.SinhMa("QDL")), Db.P("@m", hd.MaNV), Db.P("@nk", DateTime.Today),
                            Db.P("@hl", hd.NgayBatDau.Date), Db.P("@lcb", hd.LuongCoBan), Db.P("@pcl", hd.PhuCapLuong),
                            Db.P("@pcdh", hd.PhuCapDocHai), Db.P("@ky", hd.NguoiKy), Db.P("@gc", "Luong khi ky hop dong " + soHD));

                        if (!string.IsNullOrWhiteSpace(hd.MaCV))
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                @"INSERT INTO QuyetDinhChucVu(SoQD, MaNV, MaCV, NgayKy, NgayHieuLuc, PhuCapChucVu, NguoiKy)
                                  VALUES(@qd,@m,@cv,@nk,@hl,@pc,@ky)",
                                Db.P("@qd", QuyDinh.SinhMa("QDCV")), Db.P("@m", hd.MaNV), Db.P("@cv", hd.MaCV),
                                Db.P("@nk", DateTime.Today), Db.P("@hl", hd.NgayBatDau.Date), Db.P("@pc", hd.PhuCapChucVu),
                                Db.P("@ky", hd.NguoiKy));
                        }

                        if (hd.LoaiHD == QuyDinh.DaiHan)
                        {
                            Db.ExecuteInTransaction(cn, tran,
                                @"INSERT INTO HoSoBaoHiem(MaHSBH, MaNV, SoHD, SoBHXH, SoBHYT, NoiKCB, NgayLap)
                                  VALUES(@bh,@m,@hd,@xh,@yt,@kcb,@nl)",
                                Db.P("@bh", QuyDinh.SinhMa("BH")), Db.P("@m", hd.MaNV), Db.P("@hd", soHD),
                                Db.P("@xh", hd.SoBHXH.Trim()), Db.P("@yt", hd.SoBHYT.Trim()), Db.P("@kcb", hd.NoiKCB),
                                Db.P("@nl", DateTime.Today));
                        }

                        Db.ExecuteInTransaction(cn, tran,
                            "UPDATE NhanVien SET TrangThai=@tt WHERE MaNV=@m",
                            Db.P("@tt", QuyDinh.ChinhThuc), Db.P("@m", hd.MaNV));

                        tran.Commit();
                        return KetQuaXuLy.Ok("Ky hop dong thanh cong. So HD: " + soHD, soHD);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi ky hop dong: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsHopDong()
            => Db.Query(@"SELECT hd.SoHD, hd.MaNV, nv.HoTen, hd.LoaiHD, hd.NgayBatDau, hd.NgayKetThuc, hd.TrangThai,
                                 bh.SoBHXH, bh.SoBHYT
                          FROM HopDongLaoDong hd
                          JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                          LEFT JOIN HoSoBaoHiem bh ON bh.SoHD = hd.SoHD
                          ORDER BY hd.NgayKy DESC");
    }
}
