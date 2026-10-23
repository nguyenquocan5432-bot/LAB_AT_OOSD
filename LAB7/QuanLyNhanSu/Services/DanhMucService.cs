using System;
using System.Data;
using QuanLyNhanSu.Data;

namespace QuanLyNhanSu.Services
{
    public static class DanhMucService
    {
        public static DataTable DsPhongBan() => Db.Query("SELECT MaPB, TenPB, NhuCauTuyen FROM PhongBan ORDER BY MaPB");

        public static DataTable DsChucVu() => Db.Query("SELECT MaCV, TenCV, PhuCapMacDinh FROM ChucVu ORDER BY MaCV");

        public static DataTable DsCaLamViec() => Db.Query("SELECT MaCa, TenCa, GioBatDau, GioKetThuc FROM CaLamViec ORDER BY MaCa");

        public static DataTable DsNhanVien(string trangThai = null)
            => Db.Query(@"SELECT nv.MaNV, nv.HoTen, nv.GioiTinh, nv.NgaySinh, nv.CCCD, nv.DienThoai,
                                 nv.MaPB, pb.TenPB, nv.NgayVaoLam, nv.TrangThai
                          FROM NhanVien nv JOIN PhongBan pb ON nv.MaPB = pb.MaPB
                          WHERE @tt IS NULL OR nv.TrangThai = @tt
                          ORDER BY nv.MaNV", Db.P("@tt", trangThai));

        public static DataTable DsNhanVienDangLam()
            => Db.Query(@"SELECT MaNV, MaNV + ' - ' + HoTen AS HienThi, HoTen, MaPB
                          FROM NhanVien WHERE TrangThai <> @nghi ORDER BY MaNV", Db.P("@nghi", QuyDinh.NghiViec));

        public static KetQuaXuLy ThemPhongBan(string ma, string ten, int nhuCau)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten)) return KetQuaXuLy.Fail("Nhap ma va ten phong ban");
            if (nhuCau < 0) return KetQuaXuLy.Fail("Nhu cau tuyen dung khong hop le");
            return Them("INSERT INTO PhongBan(MaPB, TenPB, NhuCauTuyen) VALUES(@m,@t,@n)",
                Db.P("@m", ma.Trim()), Db.P("@t", ten.Trim()), Db.P("@n", nhuCau));
        }

        public static KetQuaXuLy ThemChucVu(string ma, string ten, decimal phuCap)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten)) return KetQuaXuLy.Fail("Nhap ma va ten chuc vu");
            if (phuCap < 0) return KetQuaXuLy.Fail("Phu cap chuc vu khong hop le");
            return Them("INSERT INTO ChucVu(MaCV, TenCV, PhuCapMacDinh) VALUES(@m,@t,@p)",
                Db.P("@m", ma.Trim()), Db.P("@t", ten.Trim()), Db.P("@p", phuCap));
        }

        public static KetQuaXuLy ThemCaLamViec(string ma, string ten, TimeSpan batDau, TimeSpan ketThuc)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten)) return KetQuaXuLy.Fail("Nhap ma va ten ca");
            if (batDau == ketThuc) return KetQuaXuLy.Fail("Gio bat dau va ket thuc khong duoc trung nhau");
            return Them("INSERT INTO CaLamViec(MaCa, TenCa, GioBatDau, GioKetThuc) VALUES(@m,@t,@b,@k)",
                Db.P("@m", ma.Trim()), Db.P("@t", ten.Trim()), Db.P("@b", batDau), Db.P("@k", ketThuc));
        }

        private static KetQuaXuLy Them(string sql, params System.Data.SqlClient.SqlParameter[] ps)
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
