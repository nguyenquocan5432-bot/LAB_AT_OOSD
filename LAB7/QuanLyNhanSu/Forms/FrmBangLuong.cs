using System;
using System.Data;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmBangLuong : Form
    {
        public FrmBangLuong()
        {
            InitializeComponent();
            var thangTruoc = DateTime.Today.AddMonths(-1);
            numThang.Value = thangTruoc.Month;
            numNam.Value = thangTruoc.Year;
            NapBangLuong();
        }

        private void NapBangLuong()
        {
            var dt = LuongService.DsBangLuong((int)numThang.Value, (int)numNam.Value);
            gridBangLuong.DataSource = dt;
            decimal tong = 0;
            foreach (DataRow r in dt.Rows) tong += (decimal)r["ThucLanh"];
            lblTong.Text = "So nhan vien: " + dt.Rows.Count + "    Tong thuc lanh: " + tong.ToString("N0") + " d";
        }

        private void btnXem_Click(object sender, EventArgs e) => NapBangLuong();

        private void btnTinhLuong_Click(object sender, EventArgs e)
        {
            int t = (int)numThang.Value, n = (int)numNam.Value;
            if (LuongService.DsBangLuong(t, n).Rows.Count > 0 &&
                MessageBox.Show("Thang " + t + "/" + n + " da co bang luong. Tinh lai?", "Xac nhan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (FormHelper.Bao(LuongService.TinhLuongThang(t, n))) NapBangLuong();
        }
    }
}
