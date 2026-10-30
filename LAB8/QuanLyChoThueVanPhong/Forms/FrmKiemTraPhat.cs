using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmKiemTraPhat : Form
    {
        public FrmKiemTraPhat()
        {
            InitializeComponent();
            cboTinhTrang.Items.AddRange(QuyDinh.TinhTrangPhong);
            cboTinhTrang.SelectedIndex = 0;
            dtpNgayKT.MaxDate = DateTime.Today;
            txtNguoiKT.Text = "Ky thuat Hoang";
            numNam.Value = DateTime.Today.Year;
            FormHelper.Nap(cboPhongPhat, DanhMucService.DsPhong(), "MaPhong", "MaPhong");
            NapKiemTra();
            NapPhieuPhat();
        }

        private void NapKiemTra()
        {
            int nam = (int)numNam.Value;
            gridKiemTra.DataSource = KiemTraService.DsKiemTra(nam);
            FormHelper.Nap(cboPhongKT, KiemTraService.DsPhongChuaKiemTra(nam), "MaPhong", "MaPhong");
        }

        private void NapPhieuPhat() => gridPhat.DataSource = KiemTraService.DsPhieuPhat();

        private void numNam_ValueChanged(object sender, EventArgs e) => NapKiemTra();

        private void btnGhiNhan_Click(object sender, EventArgs e)
        {
            var kq = KiemTraService.GhiNhanKiemTra(FormHelper.Gia(cboPhongKT), dtpNgayKT.Value.Date, cboTinhTrang.SelectedItem.ToString(),
                txtMoTa.Text, txtKeHoach.Text, txtNguoiKT.Text);
            if (FormHelper.Bao(kq)) { txtMoTa.Clear(); txtKeHoach.Clear(); numNam.Value = dtpNgayKT.Value.Year; NapKiemTra(); }
        }

        private void btnLapPhat_Click(object sender, EventArgs e)
        {
            decimal soTien;
            if (!FormHelper.Tien(txtSoTienPhat, "So tien phat", out soTien)) return;
            var kq = KiemTraService.LapPhieuPhat(FormHelper.Gia(cboPhongPhat), txtNoiDungPhat.Text, soTien);
            if (FormHelper.Bao(kq)) { txtNoiDungPhat.Clear(); txtSoTienPhat.Clear(); NapPhieuPhat(); }
        }

        private void btnNopPhat_Click(object sender, EventArgs e)
        {
            string ma = FormHelper.O(gridPhat, "MaPhieuPhat");
            if (ma == null) { MessageBox.Show("Chon phieu phat"); return; }
            if (FormHelper.Bao(KiemTraService.NopPhat(ma))) NapPhieuPhat();
        }
    }
}
