using System;
using System.Data;
using System.Linq;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class KiemTraService
    {
        // Kiem tra tinh trang phong dinh ky moi nam 1 lan; phong trong ma hu hong thi chuyen "Sua chua".
        public static KetQuaXuLy GhiNhanKiemTra(string maPhong, DateTime ngay, string tinhTrang, string moTa, string keHoach, string nguoiKiemTra)
        {
            if (string.IsNullOrWhiteSpace(maPhong)) return KetQuaXuLy.Fail("Chon phong");
            if (!QuyDinh.TinhTrangPhong.Contains(tinhTrang)) return KetQuaXuLy.Fail("Tinh trang khong hop le");
            if (ngay.Date > DateTime.Today) return KetQuaXuLy.Fail("Ngay kiem tra khong duoc sau hom nay");
            if (string.IsNullOrWhiteSpace(nguoiKiemTra)) return KetQuaXuLy.Fail("Nhap nguoi kiem tra");
            if (tinhTrang != "Tot" && string.IsNullOrWhiteSpace(keHoach)) return KetQuaXuLy.Fail("Phong can sua chua phai co ke hoach tu sua");

            object daKT = Db.Scalar("SELECT COUNT(*) FROM KiemTraPhong WHERE MaPhong=@p AND Nam=@n", Db.P("@p", maPhong), Db.P("@n", ngay.Year));
            if (Convert.ToInt32(daKT) > 0) return KetQuaXuLy.Fail("Phong " + maPhong + " da duoc kiem tra trong nam " + ngay.Year);

            string ma = QuyDinh.SinhMa("KT");
            using (var cn = Db.GetConnection())
            {
                cn.Open();
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        Db.ExecuteInTransaction(cn, tran,
                            @"INSERT INTO KiemTraPhong(MaKT, MaPhong, NgayKiemTra, Nam, TinhTrang, MoTa, KeHoachTuSua, NguoiKiemTra)
                              VALUES(@m,@p,@ng,@nam,@tt,@mt,@kh,@nkt)",
                            Db.P("@m", ma), Db.P("@p", maPhong), Db.P("@ng", ngay.Date), Db.P("@nam", ngay.Year), Db.P("@tt", tinhTrang),
                            Db.P("@mt", moTa), Db.P("@kh", keHoach), Db.P("@nkt", nguoiKiemTra));
                        if (tinhTrang == "Hu hong")
                            Db.ExecuteInTransaction(cn, tran, "UPDATE Phong SET TrangThai=@sc WHERE MaPhong=@p AND TrangThai=@trong",
                                Db.P("@sc", QuyDinh.PhongSuaChua), Db.P("@p", maPhong), Db.P("@trong", QuyDinh.PhongTrong));
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return KetQuaXuLy.Fail("Loi khi ghi nhan kiem tra: " + ex.Message);
                    }
                }
            }
            return KetQuaXuLy.Ok("Ghi nhan kiem tra phong " + maPhong + " nam " + ngay.Year + " thanh cong", ma);
        }

        // Phat khi van phong tu y thay doi, chinh sua ket cau: phong phai dang thuoc mot hop dong hieu luc.
        public static KetQuaXuLy LapPhieuPhat(string maPhong, string noiDung, decimal soTien)
        {
            if (string.IsNullOrWhiteSpace(maPhong)) return KetQuaXuLy.Fail("Chon phong");
            if (string.IsNullOrWhiteSpace(noiDung)) return KetQuaXuLy.Fail("Nhap noi dung vi pham");
            if (soTien <= 0) return KetQuaXuLy.Fail("So tien phat phai lon hon 0");
            object soHD = Db.Scalar(@"SELECT TOP 1 ct.SoHD FROM ChiTietHopDong ct JOIN HopDongThue hd ON ct.SoHD = hd.SoHD
                                      WHERE ct.MaPhong=@p AND hd.TrangThai=@tt AND ct.NgayKetThuc > @n",
                Db.P("@p", maPhong), Db.P("@tt", QuyDinh.HDHieuLuc), Db.P("@n", DateTime.Today));
            if (soHD == null) return KetQuaXuLy.Fail("Phong " + maPhong + " hien khong cho thue, khong the lap phieu phat");
            string ma = QuyDinh.SinhMa("PP");
            try
            {
                Db.Execute(@"INSERT INTO PhieuPhat(MaPhieuPhat, MaPhong, SoHD, NgayLap, NoiDungViPham, SoTienPhat, TrangThai)
                             VALUES(@m,@p,@hd,@n,@nd,@st,@tt)",
                    Db.P("@m", ma), Db.P("@p", maPhong), Db.P("@hd", soHD), Db.P("@n", DateTime.Today), Db.P("@nd", noiDung),
                    Db.P("@st", soTien), Db.P("@tt", QuyDinh.ChuaNop));
            }
            catch (Exception ex) { return KetQuaXuLy.Fail("Loi khi lap phieu phat: " + ex.Message); }
            return KetQuaXuLy.Ok("Lap phieu phat thanh cong. Ma: " + ma + " (hop dong " + soHD + ")", ma);
        }

        public static KetQuaXuLy NopPhat(string maPhieuPhat)
        {
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) return KetQuaXuLy.Fail("Chon phieu phat");
            int n = Db.Execute("UPDATE PhieuPhat SET TrangThai=@da, NgayNop=@n WHERE MaPhieuPhat=@m AND TrangThai=@chua",
                Db.P("@da", QuyDinh.DaNop), Db.P("@n", DateTime.Today), Db.P("@m", maPhieuPhat), Db.P("@chua", QuyDinh.ChuaNop));
            return n > 0 ? KetQuaXuLy.Ok("Da ghi nhan nop phat") : KetQuaXuLy.Fail("Phieu phat khong ton tai hoac da nop");
        }

        public static DataTable DsKiemTra(int nam)
            => Db.Query(@"SELECT kt.MaKT, kt.MaPhong, t.TenTang, kt.NgayKiemTra, kt.TinhTrang, kt.MoTa, kt.KeHoachTuSua, kt.NguoiKiemTra
                          FROM KiemTraPhong kt JOIN Phong p ON kt.MaPhong = p.MaPhong JOIN Tang t ON p.MaTang = t.MaTang
                          WHERE kt.Nam=@n ORDER BY kt.MaPhong", Db.P("@n", nam));

        public static DataTable DsPhongChuaKiemTra(int nam)
            => Db.Query(@"SELECT p.MaPhong FROM Phong p
                          WHERE NOT EXISTS (SELECT 1 FROM KiemTraPhong kt WHERE kt.MaPhong = p.MaPhong AND kt.Nam = @n)
                          ORDER BY p.MaPhong", Db.P("@n", nam));

        public static DataTable DsPhieuPhat()
            => Db.Query(@"SELECT pp.MaPhieuPhat, pp.MaPhong, pp.SoHD, kh.TenCongTy, pp.NgayLap, pp.NoiDungViPham, pp.SoTienPhat,
                                 pp.TrangThai, pp.NgayNop
                          FROM PhieuPhat pp JOIN HopDongThue hd ON pp.SoHD = hd.SoHD JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                          ORDER BY pp.NgayLap DESC");
    }
}
