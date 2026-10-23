using System.Data;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public static class DanhMucService
    {
        // ===== Khoa =====
        public static DataTable DsKhoa()
            => Db.Query("SELECT MaKhoa, TenKhoa FROM Khoa ORDER BY TenKhoa");

        public static KetQuaXuLy ThemKhoa(string maKhoa, string tenKhoa)
        {
            if (string.IsNullOrWhiteSpace(maKhoa) || string.IsNullOrWhiteSpace(tenKhoa))
                return KetQuaXuLy.Fail("Ma khoa va ten khoa khong duoc de trong");
            if (Db.Scalar("SELECT COUNT(*) FROM Khoa WHERE MaKhoa=@m", Db.P("@m", maKhoa)) is int c && c > 0)
                return KetQuaXuLy.Fail("Ma khoa da ton tai");
            Db.Execute("INSERT INTO Khoa(MaKhoa, TenKhoa) VALUES(@m,@t)", Db.P("@m", maKhoa), Db.P("@t", tenKhoa));
            return KetQuaXuLy.Ok("Them khoa thanh cong");
        }

        // ===== Bac si =====
        public static DataTable DsBacSi()
            => Db.Query(@"SELECT bs.MaBacSi, bs.HoTen, k.TenKhoa, bs.ChuyenMon
                          FROM BacSi bs JOIN Khoa k ON bs.MaKhoa = k.MaKhoa ORDER BY bs.HoTen");

        public static KetQuaXuLy ThemBacSi(string maBacSi, string hoTen, string maKhoa, string chuyenMon)
        {
            if (string.IsNullOrWhiteSpace(maBacSi) || string.IsNullOrWhiteSpace(hoTen))
                return KetQuaXuLy.Fail("Ma bac si va ho ten khong duoc de trong");
            if (Db.Scalar("SELECT COUNT(*) FROM BacSi WHERE MaBacSi=@m", Db.P("@m", maBacSi)) is int c && c > 0)
                return KetQuaXuLy.Fail("Ma bac si da ton tai");
            Db.Execute("INSERT INTO BacSi(MaBacSi, HoTen, MaKhoa, ChuyenMon) VALUES(@m,@h,@k,@c)",
                Db.P("@m", maBacSi), Db.P("@h", hoTen), Db.P("@k", maKhoa), Db.P("@c", chuyenMon));
            return KetQuaXuLy.Ok("Them bac si thanh cong");
        }

        // ===== Thuoc =====
        public static DataTable DsThuoc()
            => Db.Query("SELECT MaThuoc, TenThuoc, DonViTinh, GhiChu FROM Thuoc ORDER BY TenThuoc");

        public static KetQuaXuLy ThemThuoc(string maThuoc, string tenThuoc, string donVi, string ghiChu)
        {
            if (string.IsNullOrWhiteSpace(maThuoc) || string.IsNullOrWhiteSpace(tenThuoc))
                return KetQuaXuLy.Fail("Ma thuoc va ten thuoc khong duoc de trong");
            if (Db.Scalar("SELECT COUNT(*) FROM Thuoc WHERE MaThuoc=@m", Db.P("@m", maThuoc)) is int c && c > 0)
                return KetQuaXuLy.Fail("Ma thuoc da ton tai");
            Db.Execute("INSERT INTO Thuoc(MaThuoc, TenThuoc, DonViTinh, GhiChu) VALUES(@m,@t,@d,@g)",
                Db.P("@m", maThuoc), Db.P("@t", tenThuoc), Db.P("@d", donVi), Db.P("@g", ghiChu));
            return KetQuaXuLy.Ok("Them thuoc thanh cong");
        }
    }
}
