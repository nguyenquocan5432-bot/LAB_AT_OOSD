using System;

namespace QuanLyKhachSan
{
    public class KetQuaXuLy
    {
        public bool ThanhCong { get; private set; }
        public string ThongBao { get; private set; }

        private KetQuaXuLy(bool thanhCong, string thongBao)
        {
            ThanhCong = thanhCong;
            ThongBao = thongBao;
        }

        public static KetQuaXuLy Ok(string thongBao) { return new KetQuaXuLy(true, thongBao); }
        public static KetQuaXuLy Fail(string thongBao) { return new KetQuaXuLy(false, thongBao); }
    }

    /// <summary>Một dòng phòng được chọn khi lập phiếu đặt phòng (chưa lưu CSDL).</summary>
    public class PhongDatItem
    {
        public string SoPhong { get; set; }
        public int SoNguoi { get; set; }
        public decimal DonGiaNgay { get; set; }
    }

    /// <summary>Một dòng tiện nghi hư hỏng/mất được chọn khi lập phiếu đền bù (chưa lưu CSDL).</summary>
    public class DenBuItem
    {
        public string MaTienNghi { get; set; }
        public string TenLoaiTN { get; set; }
        public string MucDoThietHai { get; set; }
        public decimal SoTien { get; set; }
    }
}
