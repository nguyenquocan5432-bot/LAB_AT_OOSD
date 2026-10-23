using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmDanhMuc : Form
    {
        public FrmDanhMuc()
        {
            InitializeComponent();
            NapKhoa();
            NapBacSi();
            NapThuoc();
        }

        private void NapKhoa()
        {
            var dt = DanhMucService.DsKhoa();
            gridKhoa.DataSource = dt;
            FormHelper.Nap(cboKhoaBS, dt, "TenKhoa", "MaKhoa");
        }

        private void btnKhoa_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemKhoa(txtKhoa_Ma.Text, txtKhoa_Ten.Text);
            if (FormHelper.Bao(kq)) { NapKhoa(); txtKhoa_Ma.Clear(); txtKhoa_Ten.Clear(); }
        }

        private void NapBacSi() => gridBacSi.DataSource = DanhMucService.DsBacSi();

        private void btnBacSi_Them_Click(object sender, EventArgs e)
        {
            if (cboKhoaBS.SelectedValue == null) { MessageBox.Show("Chon khoa"); return; }
            var kq = DanhMucService.ThemBacSi(txtBS_Ma.Text, txtBS_HoTen.Text, FormHelper.Gia(cboKhoaBS), txtBS_ChuyenMon.Text);
            if (FormHelper.Bao(kq)) { NapBacSi(); txtBS_Ma.Clear(); txtBS_HoTen.Clear(); txtBS_ChuyenMon.Clear(); }
        }

        private void NapThuoc() => gridThuoc.DataSource = DanhMucService.DsThuoc();

        private void btnThuoc_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemThuoc(txtThuoc_Ma.Text, txtThuoc_Ten.Text, txtThuoc_DonVi.Text, txtThuoc_GhiChu.Text);
            if (FormHelper.Bao(kq)) { NapThuoc(); txtThuoc_Ma.Clear(); txtThuoc_Ten.Clear(); txtThuoc_DonVi.Clear(); txtThuoc_GhiChu.Clear(); }
        }
    }
}
