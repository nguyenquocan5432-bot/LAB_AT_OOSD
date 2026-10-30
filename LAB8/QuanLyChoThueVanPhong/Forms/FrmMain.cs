using System;
using System.Windows.Forms;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e) => new FrmDanhMuc().ShowDialog();
        private void btnHopDong_Click(object sender, EventArgs e) => new FrmHopDong().ShowDialog();
        private void btnThanhToan_Click(object sender, EventArgs e) => new FrmThanhToanThue().ShowDialog();
        private void btnDichVu_Click(object sender, EventArgs e) => new FrmHoaDonDichVu().ShowDialog();
        private void btnGiaHan_Click(object sender, EventArgs e) => new FrmGiaHan().ShowDialog();
        private void btnNhanVien_Click(object sender, EventArgs e) => new FrmNhanVienTheXe().ShowDialog();
        private void btnKiemTra_Click(object sender, EventArgs e) => new FrmKiemTraPhat().ShowDialog();
        private void btnThoat_Click(object sender, EventArgs e) => Close();
    }
}
