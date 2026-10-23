using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmLapToaThuoc
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMaPhieuKham, lblBacSi, lblGhiChu;
        private TextBox txtMaPhieuKham, txtGhiChu;
        private ComboBox cboBacSi;

        private Label lblThuoc, lblLieuDung, lblSoLuong;
        private ComboBox cboThuoc;
        private TextBox txtLieuDung, txtSoLuong;
        private Button btnThemThuoc, btnXoaThuoc, btnLuuToa;
        private DataGridView gridChiTiet;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMaPhieuKham = new Label { Text = "Ma phieu kham:" }; lblMaPhieuKham.SetBounds(15, 20, 100, 23);
            txtMaPhieuKham = new TextBox(); txtMaPhieuKham.SetBounds(120, 20, 150, 23);
            lblBacSi = new Label { Text = "Bac si lap toa:" }; lblBacSi.SetBounds(290, 20, 90, 23);
            cboBacSi = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; cboBacSi.SetBounds(385, 20, 180, 23);
            lblGhiChu = new Label { Text = "Ghi chu:" }; lblGhiChu.SetBounds(15, 55, 100, 23);
            txtGhiChu = new TextBox(); txtGhiChu.SetBounds(120, 55, 445, 23);

            lblThuoc = new Label { Text = "Thuoc:" }; lblThuoc.SetBounds(15, 95, 60, 23);
            cboThuoc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; cboThuoc.SetBounds(80, 95, 200, 23);
            lblLieuDung = new Label { Text = "Lieu dung:" }; lblLieuDung.SetBounds(295, 95, 70, 23);
            txtLieuDung = new TextBox(); txtLieuDung.SetBounds(370, 95, 180, 23);
            lblSoLuong = new Label { Text = "So luong:" }; lblSoLuong.SetBounds(565, 95, 65, 23);
            txtSoLuong = new TextBox(); txtSoLuong.SetBounds(630, 95, 60, 23);
            btnThemThuoc = new Button { Text = "Them vao toa" }; btnThemThuoc.SetBounds(15, 128, 120, 27);
            btnThemThuoc.Click += new EventHandler(this.btnThemThuoc_Click);
            btnXoaThuoc = new Button { Text = "Xoa dong chon" }; btnXoaThuoc.SetBounds(145, 128, 120, 27);
            btnXoaThuoc.Click += new EventHandler(this.btnXoaThuoc_Click);

            gridChiTiet = new DataGridView(); gridChiTiet.SetBounds(15, 165, 680, 220);
            gridChiTiet.ReadOnly = true; gridChiTiet.AllowUserToAddRows = false;

            btnLuuToa = new Button { Text = "Luu toa thuoc" }; btnLuuToa.SetBounds(15, 400, 180, 35);
            btnLuuToa.Click += new EventHandler(this.btnLuuToa_Click);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(720, 460);
            this.Controls.Add(lblMaPhieuKham); this.Controls.Add(txtMaPhieuKham);
            this.Controls.Add(lblBacSi); this.Controls.Add(cboBacSi);
            this.Controls.Add(lblGhiChu); this.Controls.Add(txtGhiChu);
            this.Controls.Add(lblThuoc); this.Controls.Add(cboThuoc);
            this.Controls.Add(lblLieuDung); this.Controls.Add(txtLieuDung);
            this.Controls.Add(lblSoLuong); this.Controls.Add(txtSoLuong);
            this.Controls.Add(btnThemThuoc); this.Controls.Add(btnXoaThuoc);
            this.Controls.Add(gridChiTiet);
            this.Controls.Add(btnLuuToa);
            this.Text = "Lap toa thuoc";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
