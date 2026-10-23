using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e) => new FrmDanhMuc().ShowDialog();
        private void btnDangKy_Click(object sender, EventArgs e) => new FrmDangKyBenhNhan().ShowDialog();
        private void btnTraCuu_Click(object sender, EventArgs e) => new FrmTraCuuHoSo().ShowDialog();
        private void btnLapPhieuKham_Click(object sender, EventArgs e) => new FrmLapPhieuKham().ShowDialog();
        private void btnLapToaThuoc_Click(object sender, EventArgs e) => new FrmLapToaThuoc().ShowDialog();
        private void btnDatLich_Click(object sender, EventArgs e) => new FrmDatLichDinhKy().ShowDialog();
        private void btnThanhToan_Click(object sender, EventArgs e) => new FrmThanhToanXuatVien().ShowDialog();
        private void btnThoat_Click(object sender, EventArgs e) => Close();
    }
}
