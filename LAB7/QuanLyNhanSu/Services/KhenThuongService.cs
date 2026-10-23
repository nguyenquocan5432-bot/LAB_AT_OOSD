using System;
using System.Data;
using System.Linq;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class KhenThuongService
    {
        // Lap quyet dinh khen thuong cho ca nhan hoac tap the phong ban (giam doc ky).
        // Hinh thuc PHIEU_CHI: lap phieu chi khen thuong ngay trong cung transaction.
        // Hinh thuc BANG_LUONG: tien thuong duoc cong vao bang luong thang cua ngay ky.
        public static KetQuaXuLy LapQuyetDinhKhenThuong(ThongTinKhenThuong kt)
        {
            if (kt == null) return KetQuaXuLy.Fail("Thieu thong tin khen thuong");
            if (!QuyDinh.LoaiKhenThuong.Contains(kt.LoaiKT)) return KetQuaXuLy.Fail("Loai khen thuong khong hop le");
            string loi = KiemTraDoiTuong(kt.DoiTuong, kt.MaNV, kt.MaPB);
            if (loi != null) return KetQuaXuLy.Fail(loi);
            if (string.IsNullOrWhiteSpace(kt.LyDo)) return KetQuaXuLy.Fail("Nhap ly do khen thuong");
            if (kt.SoTien <= 0) return KetQuaXuLy.Fail("So tien khen thuong phai lon hon 0");
            if (kt.HinhThucChi != QuyDinh.PhieuChi && kt.HinhThucChi != QuyDinh.BangLuong) return KetQuaXuLy.Fail("Hinh thuc chi khong hop le");
            if (string.IsNullOrWhiteSpace(kt.NguoiKy)) return KetQuaXuLy.Fail("Nhap nguoi ky (giam doc)");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        if (kt.DoiTuong == QuyDinh.TapThe && kt.HinhThucChi == QuyDinh.BangLuong)
                        {
                            object soNV = Db.ScalarInTransaction(cn, tran,
                                "SELECT COUNT(*) FROM NhanVien WHERE MaPB=@pb AND TrangThai<>@nghi",
                                Db.P("@pb", kt.MaPB), Db.P("@nghi", QuyDinh.NghiViec));
                            if (Convert.ToInt32(soNV) == 0)
                            { tran.Rollback(); return KetQuaXuLy.Fail("Phong ban khong co nhan vien de chia thuong qua bang luong"); }
                        }

                        string soQD = QuyDinh.SinhMa("QDKT");
                        bool caNhan = kt.DoiTuong == QuyDinh.CaNhan;
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO QuyetDinhKhenThuong(SoQD, NgayKy, LoaiKT, DoiTuong, MaNV, MaPB, LyDo, SoTien, HinhThucChi, NguoiKy)
                              VALUES(@qd,@nk,@l,@dt,@nv,@pb,@ld,@st,@ht,@ky)",
                            Db.P("@qd", soQD), Db.P("@nk", DateTime.Today), Db.P("@l", kt.LoaiKT), Db.P("@dt", kt.DoiTuong),
                            Db.P("@nv", caNhan ? kt.MaNV : null), Db.P("@pb", caNhan ? null : kt.MaPB),
                            Db.P("@ld", kt.LyDo), Db.P("@st", kt.SoTien), Db.P("@ht", kt.HinhThucChi), Db.P("@ky", kt.NguoiKy));

                        string thongBao = "Lap quyet dinh khen thuong thanh cong. So QD: " + soQD;
                        if (kt.HinhThucChi == QuyDinh.PhieuChi)
                        {
                            string soPC = QuyDinh.SinhMa("PC");
                            Db.ExecuteInTransaction(cn, tran,
                                "INSERT INTO PhieuChiKhenThuong(SoPhieuChi, SoQD, NgayChi, SoTien, NguoiNhan) VALUES(@pc,@qd,@n,@st,@nn)",
                                Db.P("@pc", soPC), Db.P("@qd", soQD), Db.P("@n", DateTime.Today), Db.P("@st", kt.SoTien),
                                Db.P("@nn", string.IsNullOrWhiteSpace(kt.NguoiNhan) ? (caNhan ? kt.MaNV : kt.MaPB) : kt.NguoiNhan));
                            thongBao += ", phieu chi: " + soPC;
                        }
                        else
                        {
                            thongBao += " (cong vao bang luong thang " + DateTime.Today.ToString("MM/yyyy") + ")";
                        }

                        tran.Commit();
                        return KetQuaXuLy.Ok(thongBao, soQD);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap quyet dinh khen thuong: " + ex.Message);
                    }
                }
            }
        }

        // Lap quyet dinh ky luat; PHIEU_THU: lap phieu thu nop phat, TRU_LUONG: tru vao bang luong.
        public static KetQuaXuLy LapQuyetDinhKyLuat(ThongTinKyLuat kl)
        {
            if (kl == null) return KetQuaXuLy.Fail("Thieu thong tin ky luat");
            string loi = KiemTraDoiTuong(kl.DoiTuong, kl.MaNV, kl.MaPB);
            if (loi != null) return KetQuaXuLy.Fail(loi);
            if (string.IsNullOrWhiteSpace(kl.LyDo) || string.IsNullOrWhiteSpace(kl.HinhThucKL))
                return KetQuaXuLy.Fail("Nhap ly do va hinh thuc ky luat");
            if (kl.SoTienPhat < 0) return KetQuaXuLy.Fail("So tien phat khong duoc am");
            if (kl.HinhThucThu != QuyDinh.PhieuThu && kl.HinhThucThu != QuyDinh.TruLuong) return KetQuaXuLy.Fail("Hinh thuc thu khong hop le");
            if (kl.DoiTuong == QuyDinh.TapThe && kl.HinhThucThu == QuyDinh.TruLuong && kl.SoTienPhat > 0)
                return KetQuaXuLy.Fail("Phat tap the thu bang phieu thu nop phat");
            if (string.IsNullOrWhiteSpace(kl.NguoiKy)) return KetQuaXuLy.Fail("Nhap nguoi ky (giam doc)");

            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        string soQD = QuyDinh.SinhMa("QDKL");
                        bool caNhan = kl.DoiTuong == QuyDinh.CaNhan;
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO QuyetDinhKyLuat(SoQD, NgayKy, DoiTuong, MaNV, MaPB, LyDo, HinhThucKL, SoTienPhat, HinhThucThu, NguoiKy)
                              VALUES(@qd,@nk,@dt,@nv,@pb,@ld,@ht,@st,@thu,@ky)",
                            Db.P("@qd", soQD), Db.P("@nk", DateTime.Today), Db.P("@dt", kl.DoiTuong),
                            Db.P("@nv", caNhan ? kl.MaNV : null), Db.P("@pb", caNhan ? null : kl.MaPB),
                            Db.P("@ld", kl.LyDo), Db.P("@ht", kl.HinhThucKL), Db.P("@st", kl.SoTienPhat),
                            Db.P("@thu", kl.HinhThucThu), Db.P("@ky", kl.NguoiKy));

                        string thongBao = "Lap quyet dinh ky luat thanh cong. So QD: " + soQD;
                        if (kl.HinhThucThu == QuyDinh.PhieuThu && kl.SoTienPhat > 0)
                        {
                            string soPT = QuyDinh.SinhMa("PTH");
                            Db.ExecuteInTransaction(cn, tran,
                                "INSERT INTO PhieuThuNopPhat(SoPhieuThu, SoQD, NgayThu, SoTien, NguoiNop) VALUES(@pt,@qd,@n,@st,@nn)",
                                Db.P("@pt", soPT), Db.P("@qd", soQD), Db.P("@n", DateTime.Today), Db.P("@st", kl.SoTienPhat),
                                Db.P("@nn", string.IsNullOrWhiteSpace(kl.NguoiNop) ? (caNhan ? kl.MaNV : kl.MaPB) : kl.NguoiNop));
                            thongBao += ", phieu thu: " + soPT;
                        }

                        tran.Commit();
                        return KetQuaXuLy.Ok(thongBao, soQD);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi lap quyet dinh ky luat: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable DsKhenThuong()
            => Db.Query(@"SELECT kt.SoQD, kt.NgayKy, kt.LoaiKT, kt.DoiTuong,
                                 COALESCE(nv.HoTen, pb.TenPB) AS DoiTuongNhan, kt.LyDo, kt.SoTien, kt.HinhThucChi, pc.SoPhieuChi
                          FROM QuyetDinhKhenThuong kt
                          LEFT JOIN NhanVien nv ON kt.MaNV = nv.MaNV
                          LEFT JOIN PhongBan pb ON kt.MaPB = pb.MaPB
                          LEFT JOIN PhieuChiKhenThuong pc ON pc.SoQD = kt.SoQD
                          ORDER BY kt.NgayKy DESC");

        public static DataTable DsKyLuat()
            => Db.Query(@"SELECT kl.SoQD, kl.NgayKy, kl.DoiTuong, COALESCE(nv.HoTen, pb.TenPB) AS DoiTuongBiKL,
                                 kl.LyDo, kl.HinhThucKL, kl.SoTienPhat, kl.HinhThucThu, pt.SoPhieuThu
                          FROM QuyetDinhKyLuat kl
                          LEFT JOIN NhanVien nv ON kl.MaNV = nv.MaNV
                          LEFT JOIN PhongBan pb ON kl.MaPB = pb.MaPB
                          LEFT JOIN PhieuThuNopPhat pt ON pt.SoQD = kl.SoQD
                          ORDER BY kl.NgayKy DESC");

        private static string KiemTraDoiTuong(string doiTuong, string maNV, string maPB)
        {
            if (doiTuong == QuyDinh.CaNhan) return string.IsNullOrWhiteSpace(maNV) ? "Chon nhan vien" : null;
            if (doiTuong == QuyDinh.TapThe) return string.IsNullOrWhiteSpace(maPB) ? "Chon phong ban" : null;
            return "Doi tuong phai la ca nhan hoac tap the";
        }
    }
}
