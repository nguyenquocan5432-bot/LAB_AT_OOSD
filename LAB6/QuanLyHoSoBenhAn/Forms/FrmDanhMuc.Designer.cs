using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabDanhMuc;
        private TabPage tabKhoa, tabBacSi, tabThuoc;

        private Label lblKhoa_Ma, lblKhoa_Ten;
        private TextBox txtKhoa_Ma, txtKhoa_Ten;
        private Button btnKhoa_Them;
        private DataGridView gridKhoa;

        private Label lblBS_Ma, lblBS_HoTen, lblBS_Khoa, lblBS_ChuyenMon;
        private TextBox txtBS_Ma, txtBS_HoTen, txtBS_ChuyenMon;
        private ComboBox cboKhoaBS;
        private Button btnBacSi_Them;
        private DataGridView gridBacSi;

        private Label lblThuoc_Ma, lblThuoc_Ten, lblThuoc_DonVi, lblThuoc_GhiChu;
        private TextBox txtThuoc_Ma, txtThuoc_Ten, txtThuoc_DonVi, txtThuoc_GhiChu;
        private Button btnThuoc_Them;
        private DataGridView gridThuoc;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabDanhMuc = new TabControl();
            tabKhoa = new TabPage();
            tabBacSi = new TabPage();
            tabThuoc = new TabPage();
            SuspendLayout();

            tabDanhMuc.SetBounds(10, 10, 760, 480);
            tabDanhMuc.TabPages.AddRange(new TabPage[] { tabKhoa, tabBacSi, tabThuoc });

            // ---- Tab Khoa ----
            tabKhoa.Text = "Khoa";
            lblKhoa_Ma = new Label { Text = "Ma khoa:" }; lblKhoa_Ma.SetBounds(15, 20, 70, 23);
            txtKhoa_Ma = new TextBox(); txtKhoa_Ma.SetBounds(90, 20, 120, 23);
            lblKhoa_Ten = new Label { Text = "Ten khoa:" }; lblKhoa_Ten.SetBounds(230, 20, 70, 23);
            txtKhoa_Ten = new TextBox(); txtKhoa_Ten.SetBounds(305, 20, 250, 23);
            btnKhoa_Them = new Button { Text = "Them" }; btnKhoa_Them.SetBounds(570, 18, 100, 27);
            btnKhoa_Them.Click += new EventHandler(this.btnKhoa_Them_Click);
            gridKhoa = new DataGridView(); gridKhoa.SetBounds(15, 60, 710, 385);
            gridKhoa.ReadOnly = true; gridKhoa.AllowUserToAddRows = false;
            tabKhoa.Controls.AddRange(new Control[] { lblKhoa_Ma, txtKhoa_Ma, lblKhoa_Ten, txtKhoa_Ten, btnKhoa_Them, gridKhoa });

            // ---- Tab Bac si ----
            tabBacSi.Text = "Bac si";
            lblBS_Ma = new Label { Text = "Ma BS:" }; lblBS_Ma.SetBounds(15, 20, 60, 23);
            txtBS_Ma = new TextBox(); txtBS_Ma.SetBounds(80, 20, 100, 23);
            lblBS_HoTen = new Label { Text = "Ho ten:" }; lblBS_HoTen.SetBounds(195, 20, 55, 23);
            txtBS_HoTen = new TextBox(); txtBS_HoTen.SetBounds(255, 20, 180, 23);
            lblBS_Khoa = new Label { Text = "Khoa:" }; lblBS_Khoa.SetBounds(15, 55, 60, 23);
            cboKhoaBS = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; cboKhoaBS.SetBounds(80, 55, 180, 23);
            lblBS_ChuyenMon = new Label { Text = "Chuyen mon:" }; lblBS_ChuyenMon.SetBounds(275, 55, 80, 23);
            txtBS_ChuyenMon = new TextBox(); txtBS_ChuyenMon.SetBounds(360, 55, 200, 23);
            btnBacSi_Them = new Button { Text = "Them" }; btnBacSi_Them.SetBounds(570, 20, 100, 55);
            btnBacSi_Them.Click += new EventHandler(this.btnBacSi_Them_Click);
            gridBacSi = new DataGridView(); gridBacSi.SetBounds(15, 95, 710, 350);
            gridBacSi.ReadOnly = true; gridBacSi.AllowUserToAddRows = false;
            tabBacSi.Controls.AddRange(new Control[] { lblBS_Ma, txtBS_Ma, lblBS_HoTen, txtBS_HoTen, lblBS_Khoa, cboKhoaBS, lblBS_ChuyenMon, txtBS_ChuyenMon, btnBacSi_Them, gridBacSi });

            // ---- Tab Thuoc ----
            tabThuoc.Text = "Thuoc";
            lblThuoc_Ma = new Label { Text = "Ma thuoc:" }; lblThuoc_Ma.SetBounds(15, 20, 70, 23);
            txtThuoc_Ma = new TextBox(); txtThuoc_Ma.SetBounds(90, 20, 100, 23);
            lblThuoc_Ten = new Label { Text = "Ten thuoc:" }; lblThuoc_Ten.SetBounds(200, 20, 70, 23);
            txtThuoc_Ten = new TextBox(); txtThuoc_Ten.SetBounds(275, 20, 200, 23);
            lblThuoc_DonVi = new Label { Text = "Don vi:" }; lblThuoc_DonVi.SetBounds(15, 55, 70, 23);
            txtThuoc_DonVi = new TextBox(); txtThuoc_DonVi.SetBounds(90, 55, 100, 23);
            lblThuoc_GhiChu = new Label { Text = "Ghi chu:" }; lblThuoc_GhiChu.SetBounds(200, 55, 70, 23);
            txtThuoc_GhiChu = new TextBox(); txtThuoc_GhiChu.SetBounds(275, 55, 280, 23);
            btnThuoc_Them = new Button { Text = "Them" }; btnThuoc_Them.SetBounds(580, 20, 100, 55);
            btnThuoc_Them.Click += new EventHandler(this.btnThuoc_Them_Click);
            gridThuoc = new DataGridView(); gridThuoc.SetBounds(15, 95, 710, 350);
            gridThuoc.ReadOnly = true; gridThuoc.AllowUserToAddRows = false;
            tabThuoc.Controls.AddRange(new Control[] { lblThuoc_Ma, txtThuoc_Ma, lblThuoc_Ten, txtThuoc_Ten, lblThuoc_DonVi, txtThuoc_DonVi, lblThuoc_GhiChu, txtThuoc_GhiChu, btnThuoc_Them, gridThuoc });

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(790, 510);
            this.Controls.Add(this.tabDanhMuc);
            this.Text = "Danh muc";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
