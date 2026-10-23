using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmDangKyBenhNhan : Form
    {
        public FrmDangKyBenhNhan()
        {
            InitializeComponent();
            dtpNgaySinh.MaxDate = DateTime.Today;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-20);
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "Hinh anh|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtAnhPath.Text = ofd.FileName;
                    try { picAnh.Image = System.Drawing.Image.FromFile(ofd.FileName); }
                    catch { /* bo qua loi preview */ }
                }
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaBN.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Nhap ma benh nhan va ho ten");
                return;
            }
            decimal soTienLePhi;
            if (!decimal.TryParse(txtLePhi.Text, out soTienLePhi))
            {
                MessageBox.Show("So tien le phi khong hop le");
                return;
            }

            string maHoSoMoi;
            var kq = BenhNhanService.DangKy(txtMaBN.Text, txtHoTen.Text, txtDiaChi.Text, dtpNgaySinh.Value.Date,
                txtDienThoai.Text, txtAnhPath.Text, soTienLePhi, out maHoSoMoi);

            if (FormHelper.Bao(kq))
            {
                txtMaBN.Clear(); txtHoTen.Clear(); txtDiaChi.Clear(); txtDienThoai.Clear();
                txtAnhPath.Clear(); txtLePhi.Clear(); picAnh.Image = null;
            }
        }
    }
}
