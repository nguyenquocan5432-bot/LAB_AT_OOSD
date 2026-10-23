using System;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmDanhMuc : Form
    {
        public FrmDanhMuc()
        {
            InitializeComponent();
            cboLocTrangThai.Items.AddRange(new object[] { "(Tat ca)", QuyDinh.ThuViec, QuyDinh.ChinhThuc, QuyDinh.NghiViec });
            cboLocTrangThai.SelectedIndex = 0;
            NapPhongBan();
            NapChucVu();
            NapCa();
        }

        private void NapPhongBan() => gridPhongBan.DataSource = DanhMucService.DsPhongBan();
        private void NapChucVu() => gridChucVu.DataSource = DanhMucService.DsChucVu();
        private void NapCa() => gridCa.DataSource = DanhMucService.DsCaLamViec();

        private void btnPB_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemPhongBan(txtPB_Ma.Text, txtPB_Ten.Text, (int)numPB_NhuCau.Value);
            if (FormHelper.Bao(kq)) { NapPhongBan(); txtPB_Ma.Clear(); txtPB_Ten.Clear(); }
        }

        private void btnCV_Them_Click(object sender, EventArgs e)
        {
            decimal phuCap;
            if (!FormHelper.Tien(txtCV_PhuCap, "Phu cap", out phuCap)) return;
            var kq = DanhMucService.ThemChucVu(txtCV_Ma.Text, txtCV_Ten.Text, phuCap);
            if (FormHelper.Bao(kq)) { NapChucVu(); txtCV_Ma.Clear(); txtCV_Ten.Clear(); txtCV_PhuCap.Clear(); }
        }

        private void btnCa_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemCaLamViec(txtCa_Ma.Text, txtCa_Ten.Text, dtpCa_BatDau.Value.TimeOfDay, dtpCa_KetThuc.Value.TimeOfDay);
            if (FormHelper.Bao(kq)) { NapCa(); txtCa_Ma.Clear(); txtCa_Ten.Clear(); }
        }

        private void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tt = cboLocTrangThai.SelectedIndex <= 0 ? null : cboLocTrangThai.SelectedItem.ToString();
            gridNhanVien.DataSource = DanhMucService.DsNhanVien(tt);
        }
    }
}
