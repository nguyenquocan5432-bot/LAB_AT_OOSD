using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmLapPhieuKham : Form
    {
        public FrmLapPhieuKham()
        {
            InitializeComponent();
            FormHelper.Nap(cboBacSi, DanhMucService.DsBacSi(), "HoTen", "MaBacSi");
            FormHelper.Nap(cboKhoa, DanhMucService.DsKhoa(), "TenKhoa", "MaKhoa");
            cboLoaiKham.Items.AddRange(new object[] { QuyDinh.DieuTri, QuyDinh.DinhKy });
            cboLoaiKham.SelectedIndex = 0;
            NapDsPhieu();
        }

        private void NapDsPhieu() => gridPhieuKham.DataSource = PhieuKhamService.DsPhieuKham();

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHoSo.Text)) { MessageBox.Show("Nhap ma ho so benh an"); return; }

            decimal? thanNhiet = null;
            if (!string.IsNullOrWhiteSpace(txtThanNhiet.Text))
            {
                decimal tn;
                if (decimal.TryParse(txtThanNhiet.Text, out tn)) thanNhiet = tn;
            }

            string maPhieuMoi;
            var kq = PhieuKhamService.LapPhieuKham(txtMaHoSo.Text.Trim(), FormHelper.Gia(cboBacSi), FormHelper.Gia(cboKhoa),
                cboLoaiKham.SelectedItem.ToString(), txtTrieuChung.Text, thanNhiet, txtHuyetAp.Text, txtMoTaThem.Text,
                out maPhieuMoi);

            if (FormHelper.Bao(kq))
            {
                NapDsPhieu();
                txtTrieuChung.Clear(); txtThanNhiet.Clear(); txtHuyetAp.Clear(); txtMoTaThem.Clear();
                txtMaPhieuThaoTac.Text = maPhieuMoi;
            }
        }

        private void btnChiDinhXN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhieuThaoTac.Text)) { MessageBox.Show("Nhap ma phieu kham"); return; }
            var kq = PhieuKhamService.ChiDinhXetNghiem(txtMaPhieuThaoTac.Text.Trim(), txtLoaiXetNghiem.Text);
            if (FormHelper.Bao(kq)) { txtLoaiXetNghiem.Clear(); NapXetNghiem(); }
        }

        private void btnXemXN_Click(object sender, EventArgs e) => NapXetNghiem();

        private void NapXetNghiem()
        {
            if (string.IsNullOrWhiteSpace(txtMaPhieuThaoTac.Text)) return;
            gridXetNghiem.DataSource = PhieuKhamService.DsXetNghiem(txtMaPhieuThaoTac.Text.Trim());
        }

        private void btnCapNhatKetQuaXN_Click(object sender, EventArgs e)
        {
            string maXN = FormHelper.O(gridXetNghiem, "MaXetNghiem");
            if (maXN == null) { MessageBox.Show("Chon xet nghiem can cap nhat"); return; }
            var kq = PhieuKhamService.CapNhatKetQuaXetNghiem(maXN, txtKetQuaXN.Text);
            if (FormHelper.Bao(kq)) { txtKetQuaXN.Clear(); NapXetNghiem(); }
        }

        private void btnGhiKetLuan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhieuThaoTac.Text)) { MessageBox.Show("Nhap ma phieu kham"); return; }
            var kq = PhieuKhamService.GhiKetLuan(txtMaPhieuThaoTac.Text.Trim(), txtKetLuan.Text);
            if (FormHelper.Bao(kq)) { NapDsPhieu(); }
        }
    }
}
