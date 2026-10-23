using System;

namespace QuanLyHoSoBenhAn.Services
{
    public class KetQuaXuLy
    {
        public bool ThanhCong { get; set; }
        public string ThongBao { get; set; }
        public object DuLieu { get; set; }

        public static KetQuaXuLy Ok(string thongBao = "Thanh cong", object duLieu = null)
            => new KetQuaXuLy { ThanhCong = true, ThongBao = thongBao, DuLieu = duLieu };

        public static KetQuaXuLy Fail(string thongBao)
            => new KetQuaXuLy { ThanhCong = false, ThongBao = thongBao };
    }

    public class ChiTietToaItem
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string LieuDung { get; set; }
        public int SoLuong { get; set; }
    }

    public static class QuyDinh
    {
        public const string DieuTri = "DIEU_TRI";
        public const string DinhKy = "DINH_KY";

        public const string HoSoHoatDong = "Dang hoat dong";
        public const string DangKham = "Dang kham";
        public const string DaLapToa = "Da lap toa";
        public const string DaXuatVien = "Da xuat vien";

        public const string LichDaDat = "Da dat lich";
        public const string LichDaHuy = "Da huy";
        public const string LichHoanTat = "Da hoan tat";
    }
}
