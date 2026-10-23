using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabDanhMuc;
        private TabPage tabPhongBan, tabChucVu, tabCa, tabNhanVien;

        private TextBox txtPB_Ma, txtPB_Ten;
        private NumericUpDown numPB_NhuCau;
        private DataGridView gridPhongBan;

        private TextBox txtCV_Ma, txtCV_Ten, txtCV_PhuCap;
        private DataGridView gridChucVu;

        private TextBox txtCa_Ma, txtCa_Ten;
        private DateTimePicker dtpCa_BatDau, dtpCa_KetThuc;
        private DataGridView gridCa;

        private ComboBox cboLocTrangThai;
        private DataGridView gridNhanVien;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabDanhMuc = new TabControl();
            tabPhongBan = new TabPage { Text = "Phong ban" };
            tabChucVu = new TabPage { Text = "Chuc vu" };
            tabCa = new TabPage { Text = "Ca lam viec" };
            tabNhanVien = new TabPage { Text = "Nhan vien" };
            SuspendLayout();
            tabDanhMuc.SetBounds(10, 10, 860, 480);
            tabDanhMuc.TabPages.AddRange(new TabPage[] { tabPhongBan, tabChucVu, tabCa, tabNhanVien });

            // ---- Phong ban ----
            txtPB_Ma = T(70, 20, 90);
            txtPB_Ten = T(265, 20, 220);
            numPB_NhuCau = N(605, 20, 60, 0, 100);
            gridPhongBan = G(15, 60, 820, 380);
            tabPhongBan.Controls.AddRange(new Control[] {
                L("Ma PB:", 15, 20, 55), txtPB_Ma, L("Ten phong ban:", 170, 20, 95), txtPB_Ten,
                L("Nhu cau tuyen:", 500, 20, 100), numPB_NhuCau,
                B("Them", 690, 18, 100, new EventHandler(btnPB_Them_Click)), gridPhongBan });

            // ---- Chuc vu ----
            txtCV_Ma = T(70, 20, 90);
            txtCV_Ten = T(255, 20, 200);
            txtCV_PhuCap = T(580, 20, 100);
            gridChucVu = G(15, 60, 820, 380);
            tabChucVu.Controls.AddRange(new Control[] {
                L("Ma CV:", 15, 20, 55), txtCV_Ma, L("Ten chuc vu:", 170, 20, 85), txtCV_Ten,
                L("Phu cap mac dinh:", 465, 20, 115), txtCV_PhuCap,
                B("Them", 700, 18, 100, new EventHandler(btnCV_Them_Click)), gridChucVu });

            // ---- Ca lam viec ----
            txtCa_Ma = T(70, 20, 80);
            txtCa_Ten = T(220, 20, 140);
            dtpCa_BatDau = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true };
            dtpCa_BatDau.SetBounds(440, 20, 90, 23);
            dtpCa_KetThuc = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true };
            dtpCa_KetThuc.SetBounds(580, 20, 90, 23);
            gridCa = G(15, 60, 820, 380);
            tabCa.Controls.AddRange(new Control[] {
                L("Ma ca:", 15, 20, 55), txtCa_Ma, L("Ten ca:", 165, 20, 55), txtCa_Ten,
                L("Bat dau:", 375, 20, 60), dtpCa_BatDau, L("Den:", 540, 20, 35), dtpCa_KetThuc,
                B("Them", 690, 18, 100, new EventHandler(btnCa_Them_Click)), gridCa });

            // ---- Nhan vien ----
            cboLocTrangThai = C(100, 20, 160);
            cboLocTrangThai.SelectedIndexChanged += new EventHandler(cboLocTrangThai_SelectedIndexChanged);
            gridNhanVien = G(15, 60, 820, 380);
            tabNhanVien.Controls.AddRange(new Control[] { L("Trang thai:", 15, 20, 80), cboLocTrangThai, gridNhanVien });

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(880, 500);
            Controls.Add(tabDanhMuc);
            Text = "Danh muc";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
