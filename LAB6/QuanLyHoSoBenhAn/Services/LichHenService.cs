using System;
using System.Data;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public static class LichHenService
    {
        public static DataTable DsLichHen(string maHoSo)
            => Db.Query(@"SELECT MaLichHen, NgayHen, GioHen, TrangThai, MaPhieuKham
                          FROM LichHenDinhKy WHERE MaHoSo=@m ORDER BY NgayHen DESC",
                Db.P("@m", maHoSo));

        // Benh nhan da co ho so duoc kham dinh ky mien phi theo quy
        public static KetQuaXuLy DatLich(string maHoSo, DateTime ngayHen, TimeSpan gioHen)
        {
            object trangThai = Db.Scalar("SELECT TrangThai FROM HoSoBenhAn WHERE MaHoSo=@m", Db.P("@m", maHoSo));
            if (trangThai == null) return KetQuaXuLy.Fail("Ho so benh an khong ton tai");
            if (trangThai.ToString() != QuyDinh.HoSoHoatDong) return KetQuaXuLy.Fail("Ho so benh an da ngung hoat dong");
            if (ngayHen.Date < DateTime.Today) return KetQuaXuLy.Fail("Ngay hen khong hop le (phai tu hom nay tro di)");

            string maLichHen = "LH" + DateTime.Now.ToString("yyMMddHHmmss");
            Db.Execute("INSERT INTO LichHenDinhKy(MaLichHen, MaHoSo, NgayHen, GioHen, TrangThai) VALUES(@l,@h,@n,@g,@t)",
                Db.P("@l", maLichHen), Db.P("@h", maHoSo), Db.P("@n", ngayHen.Date), Db.P("@g", gioHen), Db.P("@t", QuyDinh.LichDaDat));
            return KetQuaXuLy.Ok("Dat lich kham dinh ky thanh cong");
        }

        public static KetQuaXuLy HuyLich(string maLichHen)
        {
            int n = Db.Execute("UPDATE LichHenDinhKy SET TrangThai=@t WHERE MaLichHen=@m",
                Db.P("@t", QuyDinh.LichDaHuy), Db.P("@m", maLichHen));
            if (n == 0) return KetQuaXuLy.Fail("Khong tim thay lich hen");
            return KetQuaXuLy.Ok("Da huy lich hen");
        }

        // Gan ket qua phieu kham dinh ky vao lich hen da dat (hoan tat lich)
        public static KetQuaXuLy HoanTatLich(string maLichHen, string maPhieuKham)
        {
            int n = Db.Execute("UPDATE LichHenDinhKy SET TrangThai=@t, MaPhieuKham=@p WHERE MaLichHen=@m",
                Db.P("@t", QuyDinh.LichHoanTat), Db.P("@p", maPhieuKham), Db.P("@m", maLichHen));
            if (n == 0) return KetQuaXuLy.Fail("Khong tim thay lich hen");
            return KetQuaXuLy.Ok("Da ghi nhan hoan tat kham dinh ky");
        }
    }
}
