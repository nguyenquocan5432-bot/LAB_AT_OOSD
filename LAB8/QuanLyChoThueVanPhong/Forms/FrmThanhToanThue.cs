using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmThanhToanThue : Form
    {
        public FrmThanhToanThue()
        {
            InitializeComponent();
            txtNguoiThu.Text = "Ke toan Mai";
            FormHelper.Nap(cboHopDong, HopDongService.DsHopDong(true), "HienThi", "SoHD");
        }

        private void NapThongTin()
        {
            string soHD = FormHelper.Gia(cboHopDong);
            if (soHD == null) return;
            lblTinhTrang.Text = ThanhToanService.MoTaTinhTrang(soHD);
            gridPhieuThu.DataSource = ThanhToanService.DsPhieuThu(soHD);
            gridPhong.DataSource = HopDongService.ChiTietHopDong(soHD);
        }

        private void cboHopDong_SelectedIndexChanged(object sender, EventArgs e) => NapThongTin();

        private void btnThu_Click(object sender, EventArgs e)
        {
            var kq = ThanhToanService.ThuTienThueThang(FormHelper.Gia(cboHopDong), txtNguoiThu.Text, DateTime.Today);
            if (FormHelper.Bao(kq)) NapThongTin();
        }
    }
}
