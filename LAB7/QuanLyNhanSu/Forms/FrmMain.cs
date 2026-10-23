using System;
using System.Windows.Forms;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e) => new FrmDanhMuc().ShowDialog();
        private void btnTuyenDung_Click(object sender, EventArgs e) => new FrmTuyenDung().ShowDialog();
        private void btnKyHopDong_Click(object sender, EventArgs e) => new FrmKyHopDong().ShowDialog();
        private void btnQuyetDinh_Click(object sender, EventArgs e) => new FrmQuyetDinh().ShowDialog();
        private void btnPhanCong_Click(object sender, EventArgs e) => new FrmPhanCong().ShowDialog();
        private void btnChamCong_Click(object sender, EventArgs e) => new FrmChamCong().ShowDialog();
        private void btnKhenThuong_Click(object sender, EventArgs e) => new FrmKhenThuongKyLuat().ShowDialog();
        private void btnBangLuong_Click(object sender, EventArgs e) => new FrmBangLuong().ShowDialog();
        private void btnThoat_Click(object sender, EventArgs e) => Close();
    }
}
