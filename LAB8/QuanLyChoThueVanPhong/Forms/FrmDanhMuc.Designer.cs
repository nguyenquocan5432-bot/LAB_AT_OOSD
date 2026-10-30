using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabDanhMuc;
        private TabPage tabTang, tabPhong, tabKhachHang, tabDichVu;

        private TextBox txtTang_Ma, txtTang_Ten;
        private NumericUpDown numTang_So, numTang_HeSo;
        private DataGridView gridTang;

        private TextBox txtPhong_Ma;
        private ComboBox cboTangPhong, cboPhong_Huong;
        private NumericUpDown numPhong_DienTich;
        private DataGridView gridPhong;

        private TextBox txtKH_Ma, txtKH_Ten, txtKH_MST, txtKH_DaiDien, txtKH_DienThoai, txtKH_Email, txtKH_DiaChi;
        private DataGridView gridKhachHang;

        private TextBox txtDV_Ma, txtDV_Ten, txtDV_DonVi, txtDV_DonGia;
        private DataGridView gridDichVu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabDanhMuc = new TabControl();
            tabTang = new TabPage { Text = "Tang" };
            tabPhong = new TabPage { Text = "Phong" };
            tabKhachHang = new TabPage { Text = "Khach hang" };
            tabDichVu = new TabPage { Text = "Dich vu" };
            SuspendLayout();
            tabDanhMuc.SetBounds(10, 10, 880, 500);
            tabDanhMuc.TabPages.AddRange(new TabPage[] { tabTang, tabPhong, tabKhachHang, tabDichVu });

            // ---- Tang ----
            txtTang_Ma = T(75, 20, 80);
            txtTang_Ten = T(220, 20, 140);
            numTang_So = N(430, 20, 60, 1, 200);
            numTang_HeSo = N(580, 20, 70, 0.1m, 10, 2);
            numTang_HeSo.Increment = 0.05m;
            numTang_HeSo.Value = 1;
            gridTang = G(15, 60, 840, 400);
            tabTang.Controls.AddRange(new Control[] {
                L("Ma tang:", 15, 20, 60), txtTang_Ma, L("Ten:", 175, 20, 40), txtTang_Ten,
                L("So tang:", 370, 20, 60), numTang_So, L("He so gia:", 505, 20, 75), numTang_HeSo,
                B("Them", 670, 18, 100, new EventHandler(btnTang_Them_Click)), gridTang });

            // ---- Phong ----
            txtPhong_Ma = T(80, 20, 80);
            cboTangPhong = C(215, 20, 120);
            numPhong_DienTich = N(445, 20, 80, 1, 5000, 1);
            numPhong_DienTich.Value = 50;
            cboPhong_Huong = new ComboBox();
            cboPhong_Huong.SetBounds(585, 20, 100, 23);
            cboPhong_Huong.Items.AddRange(new object[] { "Dong", "Tay", "Nam", "Bac", "Dong Nam", "Dong Bac", "Tay Nam", "Tay Bac" });
            cboPhong_Huong.SelectedIndex = 0;
            gridPhong = G(15, 60, 840, 400);
            tabPhong.Controls.AddRange(new Control[] {
                L("Ma phong:", 15, 20, 65), txtPhong_Ma, L("Tang:", 170, 20, 40), cboTangPhong,
                L("Dien tich (m2):", 345, 20, 100), numPhong_DienTich, L("Huong:", 535, 20, 50), cboPhong_Huong,
                B("Them", 700, 18, 90, new EventHandler(btnPhong_Them_Click)),
                L("Gia thue tu tinh = dien tich x 250.000 d/m2 x he so tang", 15, 445, 500), gridPhong });
            gridPhong.Height = 375;

            // ---- Khach hang ----
            txtKH_Ma = T(90, 15, 90);
            txtKH_Ten = T(260, 15, 280);
            txtKH_MST = T(640, 15, 130);
            txtKH_DaiDien = T(90, 50, 180);
            txtKH_DienThoai = T(360, 50, 120);
            txtKH_Email = T(540, 50, 230);
            txtKH_DiaChi = T(90, 85, 450);
            gridKhachHang = G(15, 125, 840, 335);
            tabKhachHang.Controls.AddRange(new Control[] {
                L("Ma KH:", 15, 15, 70), txtKH_Ma, L("Cong ty:", 195, 15, 60), txtKH_Ten, L("MST:", 555, 15, 80), txtKH_MST,
                L("Dai dien:", 15, 50, 70), txtKH_DaiDien, L("Dien thoai:", 280, 50, 75), txtKH_DienThoai,
                L("Email:", 490, 50, 45), txtKH_Email, L("Dia chi:", 15, 85, 70), txtKH_DiaChi,
                B("Them", 560, 83, 100, new EventHandler(btnKH_Them_Click)), gridKhachHang });

            // ---- Dich vu ----
            txtDV_Ma = T(75, 20, 80);
            txtDV_Ten = T(220, 20, 160);
            txtDV_DonVi = T(450, 20, 80);
            txtDV_DonGia = T(600, 20, 100);
            gridDichVu = G(15, 60, 840, 400);
            tabDichVu.Controls.AddRange(new Control[] {
                L("Ma DV:", 15, 20, 55), txtDV_Ma, L("Ten:", 175, 20, 40), txtDV_Ten, L("DVT:", 400, 20, 45), txtDV_DonVi,
                L("Don gia:", 545, 20, 55), txtDV_DonGia,
                B("Them", 715, 18, 90, new EventHandler(btnDV_Them_Click)), gridDichVu });

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 520);
            Controls.Add(tabDanhMuc);
            Text = "Danh muc";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
