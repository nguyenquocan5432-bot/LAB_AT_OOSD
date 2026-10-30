using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmGiaHan : Form
    {
        public FrmGiaHan()
        {
            InitializeComponent();
            txtNguoiLap.Text = "NV quan ly Lan";
            FormHelper.Nap(cboHopDong, HopDongService.DsHopDong(true), "HienThi", "SoHD");
        }

        // Hien thi phong, so thang toi da va ket qua kiem tra dieu kien gia han.
        private void NapThongTin()
        {
            string soHD = FormHelper.Gia(cboHopDong);
            if (soHD == null) return;
            gridPhong.DataSource = HopDongService.ChiTietHopDong(soHD);
            gridPhieu.DataSource = GiaHanService.DsPhieuGiaHan(soHD);
            if (cboHopDong.SelectedItem is System.Data.DataRowView r)
            {
                int toiDa = Convert.ToInt32(r["SoThang"]) / 2;
                numSoThang.Maximum = Math.Max(1, toiDa);
                lblToiDa.Text = "(toi da " + toiDa + " thang = 1/2 thoi gian thue " + r["SoThang"] + " thang)";
            }
            var dk = GiaHanService.KiemTraDieuKien(soHD);
            lblDieuKien.Text = dk.ThongBao + "   |   " + ThanhToanService.MoTaTinhTrang(soHD);
            lblDieuKien.ForeColor = dk.ThanhCong ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Firebrick;
        }

        private void cboHopDong_SelectedIndexChanged(object sender, EventArgs e) => NapThongTin();

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            string maPhong = FormHelper.O(gridPhong, "MaPhong");
            if (maPhong == null) { MessageBox.Show("Chon phong can gia han trong danh sach"); return; }
            var kq = GiaHanService.LapPhieuGiaHan(FormHelper.Gia(cboHopDong), maPhong, (int)numSoThang.Value, txtNguoiLap.Text);
            if (FormHelper.Bao(kq)) NapThongTin();
        }
    }
}
