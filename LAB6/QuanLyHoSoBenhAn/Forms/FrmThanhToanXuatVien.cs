using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmThanhToanXuatVien : Form
    {
        public FrmThanhToanXuatVien()
        {
            InitializeComponent();
            NapDsPhieuChuaXuatVien();
        }

        private void NapDsPhieuChuaXuatVien()
        {
            gridPhieu.DataSource = Db.Query(
                @"SELECT pk.MaPhieuKham, bn.HoTen, pk.NgayGioKham, pk.LoaiKham, pk.TrangThai
                  FROM PhieuKhamBenh pk
                  JOIN HoSoBenhAn hs ON pk.MaHoSo = hs.MaHoSo
                  JOIN BenhNhan bn ON hs.MaBN = bn.MaBN
                  WHERE pk.TrangThai <> 'Da xuat vien'
                  ORDER BY pk.NgayGioKham DESC");
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string maPhieu = FormHelper.O(gridPhieu, "MaPhieuKham");
            if (maPhieu == null) { MessageBox.Show("Chon phieu kham trong danh sach"); return; }
            decimal soTien;
            if (!decimal.TryParse(txtSoTien.Text, out soTien)) { MessageBox.Show("So tien khong hop le"); return; }

            var kq = ThanhToanService.XuatVienThanhToan(maPhieu, soTien);
            if (FormHelper.Bao(kq)) { txtSoTien.Clear(); NapDsPhieuChuaXuatVien(); }
        }

        private void btnXacNhanMienPhi_Click(object sender, EventArgs e)
        {
            string maPhieu = FormHelper.O(gridPhieu, "MaPhieuKham");
            if (maPhieu == null) { MessageBox.Show("Chon phieu kham trong danh sach"); return; }

            var kq = ThanhToanService.XacNhanXuatVienMienPhi(maPhieu);
            if (FormHelper.Bao(kq)) NapDsPhieuChuaXuatVien();
        }
    }
}
