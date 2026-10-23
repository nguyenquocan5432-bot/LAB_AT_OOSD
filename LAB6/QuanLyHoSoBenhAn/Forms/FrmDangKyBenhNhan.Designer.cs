using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmDangKyBenhNhan
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMaBN, lblHoTen, lblDiaChi, lblNgaySinh, lblDienThoai, lblAnh, lblLePhi;
        private TextBox txtMaBN, txtHoTen, txtDiaChi, txtDienThoai, txtAnhPath, txtLePhi;
        private DateTimePicker dtpNgaySinh;
        private Button btnChonAnh, btnDangKy;
        private PictureBox picAnh;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMaBN = new Label { Text = "Ma benh nhan:" }; lblMaBN.SetBounds(15, 20, 100, 23);
            txtMaBN = new TextBox(); txtMaBN.SetBounds(130, 20, 150, 23);

            lblHoTen = new Label { Text = "Ho ten:" }; lblHoTen.SetBounds(15, 55, 100, 23);
            txtHoTen = new TextBox(); txtHoTen.SetBounds(130, 55, 250, 23);

            lblDiaChi = new Label { Text = "Dia chi:" }; lblDiaChi.SetBounds(15, 90, 100, 23);
            txtDiaChi = new TextBox(); txtDiaChi.SetBounds(130, 90, 350, 23);

            lblNgaySinh = new Label { Text = "Ngay sinh:" }; lblNgaySinh.SetBounds(15, 125, 100, 23);
            dtpNgaySinh = new DateTimePicker { Format = DateTimePickerFormat.Short }; dtpNgaySinh.SetBounds(130, 125, 150, 23);

            lblDienThoai = new Label { Text = "Dien thoai:" }; lblDienThoai.SetBounds(15, 160, 100, 23);
            txtDienThoai = new TextBox(); txtDienThoai.SetBounds(130, 160, 150, 23);

            lblLePhi = new Label { Text = "Le phi nam (VND):" }; lblLePhi.SetBounds(15, 195, 110, 23);
            txtLePhi = new TextBox { Text = "200000" }; txtLePhi.SetBounds(130, 195, 150, 23);

            lblAnh = new Label { Text = "Anh 3x4:" }; lblAnh.SetBounds(15, 230, 100, 23);
            txtAnhPath = new TextBox { ReadOnly = true }; txtAnhPath.SetBounds(130, 230, 250, 23);
            btnChonAnh = new Button { Text = "Chon anh..." }; btnChonAnh.SetBounds(390, 228, 90, 27);
            btnChonAnh.Click += new EventHandler(this.btnChonAnh_Click);

            picAnh = new PictureBox { BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            picAnh.SetBounds(500, 20, 110, 140);

            btnDangKy = new Button { Text = "Dang ky va cap ho so" }; btnDangKy.SetBounds(130, 275, 200, 35);
            btnDangKy.Click += new EventHandler(this.btnDangKy_Click);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(650, 340);
            this.Controls.Add(lblMaBN); this.Controls.Add(txtMaBN);
            this.Controls.Add(lblHoTen); this.Controls.Add(txtHoTen);
            this.Controls.Add(lblDiaChi); this.Controls.Add(txtDiaChi);
            this.Controls.Add(lblNgaySinh); this.Controls.Add(dtpNgaySinh);
            this.Controls.Add(lblDienThoai); this.Controls.Add(txtDienThoai);
            this.Controls.Add(lblLePhi); this.Controls.Add(txtLePhi);
            this.Controls.Add(lblAnh); this.Controls.Add(txtAnhPath); this.Controls.Add(btnChonAnh);
            this.Controls.Add(picAnh);
            this.Controls.Add(btnDangKy);
            this.Text = "Dang ky benh nhan moi";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
