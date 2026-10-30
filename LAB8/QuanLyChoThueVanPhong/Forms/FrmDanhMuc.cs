using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmDanhMuc : Form
    {
        public FrmDanhMuc()
        {
            InitializeComponent();
            NapTang();
            NapPhong();
            NapKhachHang();
            NapDichVu();
        }

        private void NapTang()
        {
            var dt = DanhMucService.DsTang();
            gridTang.DataSource = dt;
            FormHelper.Nap(cboTangPhong, dt.Copy(), "TenTang", "MaTang");
        }

        private void NapPhong() => gridPhong.DataSource = DanhMucService.DsPhong();
        private void NapKhachHang() => gridKhachHang.DataSource = DanhMucService.DsKhachHang();
        private void NapDichVu() => gridDichVu.DataSource = DanhMucService.DsDichVu();

        private void btnTang_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemTang(txtTang_Ma.Text, txtTang_Ten.Text, (int)numTang_So.Value, numTang_HeSo.Value);
            if (FormHelper.Bao(kq)) { NapTang(); txtTang_Ma.Clear(); txtTang_Ten.Clear(); }
        }

        private void btnPhong_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemPhong(txtPhong_Ma.Text, FormHelper.Gia(cboTangPhong), numPhong_DienTich.Value, cboPhong_Huong.Text);
            if (FormHelper.Bao(kq)) { NapPhong(); txtPhong_Ma.Clear(); }
        }

        private void btnKH_Them_Click(object sender, EventArgs e)
        {
            var kq = DanhMucService.ThemKhachHang(txtKH_Ma.Text, txtKH_Ten.Text, txtKH_MST.Text, txtKH_DaiDien.Text,
                txtKH_DienThoai.Text, txtKH_Email.Text, txtKH_DiaChi.Text);
            if (FormHelper.Bao(kq))
            {
                NapKhachHang();
                foreach (var t in new[] { txtKH_Ma, txtKH_Ten, txtKH_MST, txtKH_DaiDien, txtKH_DienThoai, txtKH_Email, txtKH_DiaChi }) t.Clear();
            }
        }

        private void btnDV_Them_Click(object sender, EventArgs e)
        {
            decimal donGia;
            if (!FormHelper.Tien(txtDV_DonGia, "Don gia", out donGia)) return;
            var kq = DanhMucService.ThemDichVu(txtDV_Ma.Text, txtDV_Ten.Text, txtDV_DonVi.Text, donGia);
            if (FormHelper.Bao(kq)) { NapDichVu(); txtDV_Ma.Clear(); txtDV_Ten.Clear(); txtDV_DonVi.Clear(); txtDV_DonGia.Clear(); }
        }
    }
}
