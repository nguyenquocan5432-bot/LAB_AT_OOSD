using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmNhanVienTheXe
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabMain;
        private TabPage tabNhanVien, tabThe;

        private TextBox txtHoTen, txtCCCD, txtChucVu, txtDienThoai, txtHinhAnh;
        private ComboBox cboCongTy, cboPhong;
        private PictureBox picAnh;
        private DataGridView gridNhanVien;

        private ComboBox cboNhanVienThe, cboLoaiXe;
        private TextBox txtBienSo, txtHieuXe, txtMauXe;
        private DataGridView gridThe;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            tabMain = new TabControl();
            tabMain.SetBounds(10, 10, 980, 560);
            tabNhanVien = new TabPage { Text = "Nhan vien lam viec tai cao oc" };
            tabThe = new TabPage { Text = "The giu xe" };
            tabMain.TabPages.AddRange(new TabPage[] { tabNhanVien, tabThe });

            // ---- Nhan vien ----
            txtHoTen = T(90, 15, 220);
            txtCCCD = T(400, 15, 140);
            cboCongTy = C(90, 50, 300);
            cboCongTy.SelectedIndexChanged += new EventHandler(cboCongTy_SelectedIndexChanged);
            cboPhong = C(470, 50, 100);
            txtChucVu = T(90, 85, 220);
            txtDienThoai = T(400, 85, 140);
            txtHinhAnh = T(90, 120, 380);
            txtHinhAnh.ReadOnly = true;
            picAnh = new PictureBox { BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            picAnh.SetBounds(820, 15, 120, 140);
            gridNhanVien = G(15, 200, 940, 280);
            gridNhanVien.SelectionChanged += new EventHandler(gridNhanVien_SelectionChanged);
            tabNhanVien.Controls.AddRange(new Control[] {
                L("Ho ten:", 15, 15, 70), txtHoTen, L("CCCD:", 325, 15, 70), txtCCCD,
                L("Cong ty:", 15, 50, 70), cboCongTy, L("Phong:", 405, 50, 60), cboPhong,
                L("Chuc vu:", 15, 85, 70), txtChucVu, L("Dien thoai:", 325, 85, 75), txtDienThoai,
                L("Hinh anh:", 15, 120, 70), txtHinhAnh, B("Chon anh...", 480, 118, 100, new EventHandler(btnChonAnh_Click)),
                picAnh,
                B("Bao cao nhan vien moi", 15, 155, 200, new EventHandler(btnBaoCao_Click), 34),
                B("Nhan vien nghi viec", 230, 155, 180, new EventHandler(btnNghiViec_Click), 34),
                gridNhanVien });

            // ---- The giu xe ----
            cboNhanVienThe = C(90, 15, 300);
            cboLoaiXe = C(480, 15, 100);
            txtBienSo = T(90, 50, 140);
            txtHieuXe = T(300, 50, 150);
            txtMauXe = T(510, 50, 100);
            gridThe = G(15, 130, 940, 350);
            tabThe.Controls.AddRange(new Control[] {
                L("Nhan vien:", 15, 15, 70), cboNhanVienThe, L("Loai xe:", 410, 15, 65), cboLoaiXe,
                L("Bien so:", 15, 50, 70), txtBienSo, L("Hieu xe:", 240, 50, 60), txtHieuXe, L("Mau:", 465, 50, 40), txtMauXe,
                B("Cap the giu xe", 15, 88, 160, new EventHandler(btnCapThe_Click), 32),
                B("Huy the chon", 185, 88, 140, new EventHandler(btnHuyThe_Click), 32), gridThe });

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(1000, 580);
            Controls.Add(tabMain);
            Text = "Nhan vien cong ty thue va the giu xe";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
