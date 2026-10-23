using System;

namespace QuanLyNhanSu.Services
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

    public class NhanVienMoi
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
    }

    public class ThongTinTuyenDung
    {
        public string MaPB { get; set; }
        public DateTime NgayBatDauTV { get; set; }
        public DateTime NgayKetThucTV { get; set; }
        public decimal LuongThuViec { get; set; }
        public string DieuKhoanCongTy { get; set; }
        public string DieuKhoanNhanVien { get; set; }
        public string NguoiKy { get; set; }
    }

    public class ThongTinHopDong
    {
        public string MaNV { get; set; }
        public string LoaiHD { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string NoiDung { get; set; }
        public string NguoiKy { get; set; }

        // Quyet dinh luong (bat buoc khi ky hop dong)
        public decimal LuongCoBan { get; set; }
        public decimal PhuCapLuong { get; set; }
        public decimal PhuCapDocHai { get; set; }

        // Quyet dinh chuc vu (neu co)
        public string MaCV { get; set; }
        public decimal PhuCapChucVu { get; set; }

        // Ho so bao hiem (chi hop dong dai han)
        public string SoBHXH { get; set; }
        public string SoBHYT { get; set; }
        public string NoiKCB { get; set; }
    }

    public class ChiTietPhanCongItem
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgayLam { get; set; }
        public string MaCa { get; set; }
        public string TenCa { get; set; }
    }

    public class ChiTietPhuTroiItem
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public decimal SoGio { get; set; }
    }

    public class ThongTinKhenThuong
    {
        public string LoaiKT { get; set; }
        public string DoiTuong { get; set; }
        public string MaNV { get; set; }
        public string MaPB { get; set; }
        public string LyDo { get; set; }
        public decimal SoTien { get; set; }
        public string HinhThucChi { get; set; }
        public string NguoiKy { get; set; }
        public string NguoiNhan { get; set; }
    }

    public class ThongTinKyLuat
    {
        public string DoiTuong { get; set; }
        public string MaNV { get; set; }
        public string MaPB { get; set; }
        public string LyDo { get; set; }
        public string HinhThucKL { get; set; }
        public decimal SoTienPhat { get; set; }
        public string HinhThucThu { get; set; }
        public string NguoiKy { get; set; }
        public string NguoiNop { get; set; }
    }

    public static class QuyDinh
    {
        public const string ThuViec = "Thu viec";
        public const string ChinhThuc = "Chinh thuc";
        public const string NghiViec = "Nghi viec";

        public const string DaiHan = "DAI_HAN";
        public const string NganHan = "NGAN_HAN";
        public const string HieuLuc = "Hieu luc";

        public const string TangCa = "TANG_CA";
        public const string NgayLe = "NGAY_LE";
        public const string NgayNghi = "NGAY_NGHI";

        public const string CaNhan = "CA_NHAN";
        public const string TapThe = "TAP_THE";
        public const string PhieuChi = "PHIEU_CHI";
        public const string BangLuong = "BANG_LUONG";
        public const string PhieuThu = "PHIEU_THU";
        public const string TruLuong = "TRU_LUONG";

        public static readonly string[] LoaiKhenThuong = { "THANH_TICH", "NGAY_LE", "CUOI_NAM", "LUONG_13" };

        // Cong thuc tinh luong
        public const decimal NgayCongChuan = 26m;
        public const decimal GioMotNgay = 8m;
        public const decimal HeSoPhuTroi = 1.5m;
        public const int SoNgayThuViecToiDa = 60;

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
