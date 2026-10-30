using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    public static class DanhMucService
    {
        public static DataTable DsTang() => Db.Query("SELECT MaTang, TenTang, SoTang, HeSoGia FROM Tang ORDER BY SoTang");

        public static DataTable DsPhong()
            => Db.Query(@"SELECT p.MaPhong, t.TenTang, p.DienTich, p.Huong, p.GiaThue, p.TrangThai
                          FROM Phong p JOIN Tang t ON p.MaTang = t.MaTang ORDER BY t.SoTang, p.MaPhong");

        public static DataTable DsPhongTrong()
            => Db.Query(@"SELECT p.MaPhong, t.TenTang, p.DienTich, p.Huong, p.GiaThue,
                                 p.MaPhong + ' - ' + t.TenTang + ' - ' + CAST(p.DienTich AS VARCHAR(20)) + ' m2' AS HienThi
                          FROM Phong p JOIN Tang t ON p.MaTang = t.MaTang
                          WHERE p.TrangThai = @tt ORDER BY t.SoTang, p.MaPhong", Db.P("@tt", QuyDinh.PhongTrong));

        public static DataTable DsKhachHang()
            => Db.Query(@"SELECT MaKH, TenCongTy, MaSoThue, NguoiDaiDien, DienThoai, Email, DiaChi,
                                 MaKH + ' - ' + TenCongTy AS HienThi
                          FROM KhachHang ORDER BY MaKH");

        public static DataTable DsDichVu() => Db.Query("SELECT MaDV, TenDV, DonViTinh, DonGia FROM DichVu ORDER BY MaDV");

        public static KetQuaXuLy ThemTang(string ma, string ten, int soTang, decimal heSo)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten)) return KetQuaXuLy.Fail("Nhap ma va ten tang");
            if (soTang < 1) return KetQuaXuLy.Fail("So tang phai >= 1");
            if (heSo <= 0) return KetQuaXuLy.Fail("He so gia phai lon hon 0");
            return Them("INSERT INTO Tang(MaTang, TenTang, SoTang, HeSoGia) VALUES(@m,@t,@s,@h)",
                Db.P("@m", ma.Trim()), Db.P("@t", ten.Trim()), Db.P("@s", soTang), Db.P("@h", heSo));
        }

        // Gia thue tu tinh theo dien tich va he so tang (do cao).
        public static KetQuaXuLy ThemPhong(string ma, string maTang, decimal dienTich, string huong)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(maTang)) return KetQuaXuLy.Fail("Nhap ma phong va chon tang");
            if (dienTich <= 0) return KetQuaXuLy.Fail("Dien tich phai lon hon 0");
            if (string.IsNullOrWhiteSpace(huong)) return KetQuaXuLy.Fail("Nhap huong phong");
            object heSo = Db.Scalar("SELECT HeSoGia FROM Tang WHERE MaTang=@t", Db.P("@t", maTang));
            if (heSo == null) return KetQuaXuLy.Fail("Tang khong ton tai");
            decimal gia = QuyDinh.TinhGiaThue(dienTich, (decimal)heSo);
            var kq = Them("INSERT INTO Phong(MaPhong, MaTang, DienTich, Huong, GiaThue, TrangThai) VALUES(@m,@t,@d,@h,@g,@tt)",
                Db.P("@m", ma.Trim()), Db.P("@t", maTang), Db.P("@d", dienTich), Db.P("@h", huong.Trim()),
                Db.P("@g", gia), Db.P("@tt", QuyDinh.PhongTrong));
            if (kq.ThanhCong) kq.ThongBao = "Them phong thanh cong. Gia thue: " + gia.ToString("N0") + " d/thang";
            return kq;
        }

        public static KetQuaXuLy ThemKhachHang(string ma, string tenCongTy, string mst, string daiDien, string dienThoai, string email, string diaChi)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(tenCongTy)) return KetQuaXuLy.Fail("Nhap ma va ten cong ty");
            if (string.IsNullOrWhiteSpace(mst)) return KetQuaXuLy.Fail("Nhap ma so thue");
            if (string.IsNullOrWhiteSpace(daiDien) || string.IsNullOrWhiteSpace(dienThoai)) return KetQuaXuLy.Fail("Nhap nguoi dai dien va dien thoai");
            return Them(@"INSERT INTO KhachHang(MaKH, TenCongTy, MaSoThue, NguoiDaiDien, DienThoai, Email, DiaChi)
                          VALUES(@m,@t,@mst,@dd,@dt,@e,@dc)",
                Db.P("@m", ma.Trim()), Db.P("@t", tenCongTy.Trim()), Db.P("@mst", mst.Trim()), Db.P("@dd", daiDien.Trim()),
                Db.P("@dt", dienThoai.Trim()), Db.P("@e", email), Db.P("@dc", diaChi));
        }

        public static KetQuaXuLy ThemDichVu(string ma, string ten, string donViTinh, decimal donGia)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(donViTinh))
                return KetQuaXuLy.Fail("Nhap ma, ten dich vu va don vi tinh");
            if (donGia < 0) return KetQuaXuLy.Fail("Don gia khong duoc am");
            return Them("INSERT INTO DichVu(MaDV, TenDV, DonViTinh, DonGia) VALUES(@m,@t,@d,@g)",
                Db.P("@m", ma.Trim()), Db.P("@t", ten.Trim()), Db.P("@d", donViTinh.Trim()), Db.P("@g", donGia));
        }

        private static KetQuaXuLy Them(string sql, params SqlParameter[] ps)
        {
            try
            {
                Db.Execute(sql, ps);
                return KetQuaXuLy.Ok("Them thanh cong");
            }
            catch (Exception ex)
            {
                return KetQuaXuLy.Fail("Khong the them (ma co the da ton tai): " + ex.Message);
            }
        }
    }
}
