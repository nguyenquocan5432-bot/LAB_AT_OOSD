using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmDatLichDinhKy : Form
    {
        public FrmDatLichDinhKy()
        {
            InitializeComponent();
            dtpNgayHen.MinDate = DateTime.Today;
            dtpNgayHen.Value = DateTime.Today.AddDays(7);
        }

        private void btnXemLich_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHoSo.Text)) { MessageBox.Show("Nhap ma ho so"); return; }
            gridLichHen.DataSource = LichHenService.DsLichHen(txtMaHoSo.Text.Trim());
        }

        private void btnDatLich_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHoSo.Text)) { MessageBox.Show("Nhap ma ho so"); return; }
            var kq = LichHenService.DatLich(txtMaHoSo.Text.Trim(), dtpNgayHen.Value.Date, dtpGioHen.Value.TimeOfDay);
            if (FormHelper.Bao(kq)) btnXemLich_Click(sender, e);
        }

        private void btnHuyLich_Click(object sender, EventArgs e)
        {
            string maLich = FormHelper.O(gridLichHen, "MaLichHen");
            if (maLich == null) { MessageBox.Show("Chon lich hen can huy"); return; }
            var kq = LichHenService.HuyLich(maLich);
            if (FormHelper.Bao(kq)) btnXemLich_Click(sender, e);
        }

        private void btnHoanTat_Click(object sender, EventArgs e)
        {
            string maLich = FormHelper.O(gridLichHen, "MaLichHen");
            if (maLich == null) { MessageBox.Show("Chon lich hen can hoan tat"); return; }
            if (string.IsNullOrWhiteSpace(txtMaPhieuKhamGan.Text)) { MessageBox.Show("Nhap ma phieu kham dinh ky da lap"); return; }
            var kq = LichHenService.HoanTatLich(maLich, txtMaPhieuKhamGan.Text.Trim());
            if (FormHelper.Bao(kq)) btnXemLich_Click(sender, e);
        }
    }
}
