using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmKiemTraPhat
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabMain;
        private TabPage tabKiemTra, tabPhat;

        private NumericUpDown numNam;
        private ComboBox cboPhongKT, cboTinhTrang;
        private DateTimePicker dtpNgayKT;
        private TextBox txtMoTa, txtKeHoach, txtNguoiKT;
        private DataGridView gridKiemTra;

        private ComboBox cboPhongPhat;
        private TextBox txtNoiDungPhat, txtSoTienPhat;
        private DataGridView gridPhat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            tabMain = new TabControl();
            tabMain.SetBounds(10, 10, 980, 540);
            tabKiemTra = new TabPage { Text = "Kiem tra phong hang nam" };
            tabPhat = new TabPage { Text = "Phieu phat thay doi ket cau" };
            tabMain.TabPages.AddRange(new TabPage[] { tabKiemTra, tabPhat });

            // ---- Kiem tra phong ----
            numNam = N(60, 15, 75, 2000, 2100);
            numNam.ValueChanged += new EventHandler(numNam_ValueChanged);
            cboPhongKT = C(285, 15, 100);
            dtpNgayKT = D(480, 15, 120);
            cboTinhTrang = C(700, 15, 130);
            txtMoTa = T(90, 50, 740);
            txtKeHoach = T(90, 85, 500);
            txtNguoiKT = T(680, 85, 150);
            gridKiemTra = G(15, 165, 940, 320);
            tabKiemTra.Controls.AddRange(new Control[] {
                L("Nam:", 15, 15, 40), numNam, L("Phong chua kiem tra:", 150, 15, 130), cboPhongKT,
                L("Ngay KT:", 405, 15, 70), dtpNgayKT, L("Tinh trang:", 620, 15, 75), cboTinhTrang,
                L("Mo ta:", 15, 50, 70), txtMoTa, L("Ke hoach sua:", 15, 85, 75), txtKeHoach, L("Nguoi KT:", 605, 85, 70), txtNguoiKT,
                B("Ghi nhan kiem tra", 15, 122, 180, new EventHandler(btnGhiNhan_Click), 32),
                L("Moi phong kiem tra 1 lan / nam de lap ke hoach tu sua", 210, 129, 400), gridKiemTra });

            // ---- Phieu phat ----
            cboPhongPhat = C(90, 15, 100);
            txtSoTienPhat = T(300, 15, 140);
            txtNoiDungPhat = T(90, 50, 740);
            gridPhat = G(15, 130, 940, 355);
            tabPhat.Controls.AddRange(new Control[] {
                L("Phong:", 15, 15, 70), cboPhongPhat, L("So tien phat:", 205, 15, 90), txtSoTienPhat,
                L("Vi pham:", 15, 50, 70), txtNoiDungPhat,
                B("Lap phieu phat", 15, 88, 160, new EventHandler(btnLapPhat_Click), 32),
                B("Ghi nhan da nop phat", 185, 88, 180, new EventHandler(btnNopPhat_Click), 32), gridPhat });

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(1000, 560);
            Controls.Add(tabMain);
            Text = "Kiem tra phong va phieu phat";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
