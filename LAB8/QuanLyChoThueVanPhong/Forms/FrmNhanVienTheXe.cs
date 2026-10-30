using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmNhanVienTheXe : Form
    {
        public FrmNhanVienTheXe()
        {
            InitializeComponent();
            cboLoaiXe.Items.AddRange(QuyDinh.LoaiXe);
            cboLoaiXe.SelectedIndex = 0;
            FormHelper.Nap(cboCongTy, DanhMucService.DsKhachHang(), "HienThi", "MaKH");
            NapNhanVien();
            NapThe();
        }

        private void NapNhanVien()
        {
            var dt = NhanVienService.DsNhanVien();
            gridNhanVien.DataSource = dt;
            var dangLam = dt.Copy();
            dangLam.DefaultView.RowFilter = "TrangThai = '" + QuyDinh.DangLam + "'";
            FormHelper.Nap(cboNhanVienThe, dangLam.DefaultView.ToTable(), "HienThi", "MaNV");
        }

        private void NapThe() => gridThe.DataSource = NhanVienService.DsTheGiuXe();

        // Chi cho chon phong thuoc hop dong dang hieu luc cua cong ty.
        private void cboCongTy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maKH = FormHelper.Gia(cboCongTy);
            if (maKH != null) FormHelper.Nap(cboPhong, NhanVienService.DsPhongDangThueCuaKhach(maKH), "MaPhong", "MaPhong");
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Filter = "Hinh anh|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                txtHinhAnh.Text = dlg.FileName;
                picAnh.ImageLocation = dlg.FileName;
            }
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            var kq = NhanVienService.BaoCaoNhanVienMoi(new NhanVienMoi
            {
                HoTen = txtHoTen.Text, CCCD = txtCCCD.Text, HinhAnh = txtHinhAnh.Text, MaKH = FormHelper.Gia(cboCongTy),
                ChucVu = txtChucVu.Text, MaPhong = FormHelper.Gia(cboPhong), DienThoai = txtDienThoai.Text
            });
            if (FormHelper.Bao(kq))
            {
                foreach (var t in new[] { txtHoTen, txtCCCD, txtHinhAnh, txtChucVu, txtDienThoai }) t.Clear();
                picAnh.Image = null;
                NapNhanVien();
            }
        }

        private void btnNghiViec_Click(object sender, EventArgs e)
        {
            string maNV = FormHelper.O(gridNhanVien, "MaNV");
            if (maNV == null) { MessageBox.Show("Chon nhan vien"); return; }
            if (FormHelper.Bao(NhanVienService.NghiViec(maNV))) { NapNhanVien(); NapThe(); }
        }

        private void gridNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            string anh = FormHelper.O(gridNhanVien, "HinhAnh");
            picAnh.ImageLocation = string.IsNullOrEmpty(anh) || !System.IO.File.Exists(anh) ? null : anh;
        }

        private void btnCapThe_Click(object sender, EventArgs e)
        {
            var kq = NhanVienService.CapTheGiuXe(FormHelper.Gia(cboNhanVienThe), cboLoaiXe.SelectedItem.ToString(),
                txtBienSo.Text, txtHieuXe.Text, txtMauXe.Text);
            if (FormHelper.Bao(kq)) { txtBienSo.Clear(); txtHieuXe.Clear(); txtMauXe.Clear(); NapThe(); }
        }

        private void btnHuyThe_Click(object sender, EventArgs e)
        {
            string ma = FormHelper.O(gridThe, "MaThe");
            if (ma == null) { MessageBox.Show("Chon the"); return; }
            if (FormHelper.Bao(NhanVienService.HuyThe(ma))) NapThe();
        }
    }
}
