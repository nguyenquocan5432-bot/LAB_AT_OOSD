using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmChamCong
    {
        private System.ComponentModel.IContainer components = null;
        private NumericUpDown numThang, numNam;
        private TabControl tabMain;
        private TabPage tabChamCong, tabPhuTroi;

        private ComboBox cboNhanVienCC;
        private NumericUpDown numNgayCong, numNghiPhep, numNghiKP;
        private DataGridView gridChamCong;

        private DateTimePicker dtpNgayPT;
        private ComboBox cboHinhThuc, cboPhongBan, cboNhanVienPT;
        private TextBox txtNguoiLap, txtGhiChu;
        private NumericUpDown numSoGio;
        private DataGridView gridPhuTroiTam, gridPhuTroi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            numThang = N(70, 12, 55, 1, 12);
            numNam = N(180, 12, 75, 2000, 2100);
            numThang.ValueChanged += new EventHandler(numThangNam_ValueChanged);
            numNam.ValueChanged += new EventHandler(numThangNam_ValueChanged);

            tabMain = new TabControl();
            tabMain.SetBounds(10, 45, 880, 490);
            tabChamCong = new TabPage { Text = "Bang cham cong thang" };
            tabPhuTroi = new TabPage { Text = "Phieu ghi nhan phu troi" };
            tabMain.TabPages.AddRange(new TabPage[] { tabChamCong, tabPhuTroi });

            // ---- Bang cham cong ----
            cboNhanVienCC = C(90, 15, 230);
            numNgayCong = N(420, 15, 65, 0, 31, 1);
            numNghiPhep = N(580, 15, 55, 0, 31, 1);
            numNghiKP = N(730, 15, 55, 0, 31, 1);
            gridChamCong = G(15, 95, 840, 355);
            gridChamCong.CellClick += new DataGridViewCellEventHandler(gridChamCong_CellClick);
            tabChamCong.Controls.AddRange(new Control[] {
                L("Nhan vien:", 15, 15, 75), cboNhanVienCC, L("Ngay cong:", 335, 15, 80), numNgayCong,
                L("Nghi phep:", 500, 15, 75), numNghiPhep, L("Nghi KP:", 655, 15, 70), numNghiKP,
                B("Luu cham cong", 15, 52, 150, new EventHandler(btnLuuCC_Click), 32),
                L("(chon dong trong luoi de sua)", 180, 58, 250), gridChamCong });

            // ---- Phieu phu troi ----
            dtpNgayPT = D(110, 15, 120);
            cboHinhThuc = C(330, 15, 130);
            cboPhongBan = C(560, 15, 200);
            txtNguoiLap = T(110, 50, 200);
            txtGhiChu = T(390, 50, 370);
            cboNhanVienPT = C(110, 90, 230);
            numSoGio = N(420, 90, 65, 0.5m, 12, 1);
            numSoGio.Value = 2;
            gridPhuTroiTam = G(15, 130, 400, 270);
            gridPhuTroi = G(425, 130, 430, 320);
            tabPhuTroi.Controls.AddRange(new Control[] {
                L("Ngay phu troi:", 15, 15, 95), dtpNgayPT, L("Hinh thuc:", 250, 15, 75), cboHinhThuc,
                L("Phong ban:", 480, 15, 75), cboPhongBan,
                L("Nguoi lap:", 15, 50, 95), txtNguoiLap, L("Ghi chu:", 325, 50, 60), txtGhiChu,
                L("Nhan vien:", 15, 90, 95), cboNhanVienPT, L("So gio:", 355, 90, 60), numSoGio,
                B("Them", 500, 88, 80, new EventHandler(btnThemPT_Click)),
                B("Xoa dong", 590, 88, 90, new EventHandler(btnXoaPT_Click)),
                gridPhuTroiTam, gridPhuTroi,
                B("Luu phieu phu troi", 15, 410, 180, new EventHandler(btnLuuPT_Click), 34),
                L("(gio phu troi = 1.5 gio thuong)", 205, 418, 210) });

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 545);
            Controls.AddRange(new Control[] { L("Thang:", 10, 12, 55), numThang, L("Nam:", 135, 12, 40), numNam, tabMain });
            Text = "Cham cong va phieu phu troi";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
