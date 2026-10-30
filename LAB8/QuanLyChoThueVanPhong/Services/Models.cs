using System;

namespace QuanLyChoThueVanPhong.Services
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

    public class PhongChonItem
    {
        public string MaPhong { get; set; }
        public string Tang { get; set; }
        public decimal DienTich { get; set; }
        public string Huong { get; set; }
        public decimal GiaThue { get; set; }
    }

    public class ChiTietDichVuItem
    {
        public string MaDV { get; set; }
        public string TenDV { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }

    public class NhanVienMoi
    {
        public string HoTen { get; set; }
        public string CCCD { get; set; }
        public string HinhAnh { get; set; }
        public string MaKH { get; set; }
        public string ChucVu { get; set; }
        public string MaPhong { get; set; }
        public string DienThoai { get; set; }
    }

    // Tinh trang thanh toan tien thue cua mot hop dong tai mot ngay
    public class TinhTrangThue
    {
        public int KyHienTai { get; set; }      // ky (thang thu may cua hop dong) dang dien ra
        public int TongSoKy { get; set; }       // tong so ky den ngay ket thuc hop dong
        public int DaTraDenKy { get; set; }     // ky lon nhat da thu tien
        public int SoKyNo => Math.Max(0, Math.Min(KyHienTai, TongSoKy) - DaTraDenKy);
        public bool ConNo => SoKyNo > 0;
    }

    public static class QuyDinh
    {
        public const int SoThangToiThieu = 6;
        public const int SoThangTraTruoc = 6;
        public const decimal GiaCoBanM2 = 250000m;   // d/m2/thang, nhan he so tang

        public const string PhongTrong = "Trong";
        public const string PhongDangThue = "Dang thue";
        public const string PhongSuaChua = "Sua chua";

        public const string HDHieuLuc = "Hieu luc";
        public const string HDDaThanhLy = "Da thanh ly";

        public const string TraTruoc = "TRA_TRUOC";
        public const string HangThang = "HANG_THANG";

        public const string ChuaThanhToan = "Chua thanh toan";
        public const string DaThanhToan = "Da thanh toan";

        public const string DangLam = "Dang lam";
        public const string DaNghi = "Da nghi";
        public const string TheDangDung = "Dang dung";
        public const string TheDaHuy = "Da huy";
        public static readonly string[] LoaiXe = { "Xe may", "O to", "Xe dap" };
        public static readonly string[] TinhTrangPhong = { "Tot", "Can sua chua", "Hu hong" };

        public const string ChuaNop = "Chua nop";
        public const string DaNop = "Da nop";

        public static decimal TinhGiaThue(decimal dienTich, decimal heSoTang)
            => Math.Round(dienTich * GiaCoBanM2 * heSoTang / 1000m, 0) * 1000m;

        // So thang tron giua hai ngay (dung de xac dinh ky thanh toan)
        public static int SoThangGiua(DateTime tu, DateTime den)
        {
            int n = (den.Year - tu.Year) * 12 + den.Month - tu.Month;
            if (den.Day < tu.Day) n--;
            return n;
        }

        // Ma = tien to + yyMMddHHmmss + phan tram giay; tang dan, khong trung khi thao tac lien tiep.
        private static long _soCuoi;
        private static readonly object _khoa = new object();

        public static string SinhMa(string tienTo)
        {
            lock (_khoa)
            {
                long so = long.Parse(DateTime.Now.ToString("yyMMddHHmmssff"));
                _soCuoi = Math.Max(so, _soCuoi + 1);
                return tienTo + _soCuoi;
            }
        }
    }
}
