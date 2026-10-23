using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmTraCuuHoSo : Form
    {
        public FrmTraCuuHoSo()
        {
            InitializeComponent();
        }

        private void btnTimTheoMa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHoSo.Text)) { MessageBox.Show("Nhap ma ho so"); return; }
            gridKetQua.DataSource = BenhNhanService.TraCuuTheoMaHoSo(txtMaHoSo.Text.Trim());
        }

        private void btnTimTheoTuKhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTuKhoa.Text)) { MessageBox.Show("Nhap ten hoac so dien thoai"); return; }
            gridKetQua.DataSource = BenhNhanService.TraCuuTheoTenHoacSdt(txtTuKhoa.Text.Trim());
        }

        private void btnXemLichSu_Click(object sender, EventArgs e)
        {
            string maHoSo = FormHelper.O(gridKetQua, "MaHoSo");
            if (maHoSo == null) { MessageBox.Show("Chon mot ho so trong danh sach"); return; }
            gridLichSu.DataSource = BenhNhanService.LichSuDieuTri(maHoSo);
        }
    }
}
