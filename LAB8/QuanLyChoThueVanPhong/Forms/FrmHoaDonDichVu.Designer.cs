using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmHoaDonDichVu
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboHopDong, cboDichVu;
        private NumericUpDown numThang, numNam, numSoLuong;
        private Label lblTong;
        private DataGridView gridChiTietTam, gridHoaDon, gridChiTietHD;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            cboHopDong = C(90, 15, 380);
            cboHopDong.SelectedIndexChanged += new EventHandler(cboHopDong_SelectedIndexChanged);
            numThang = N(545, 15, 55, 1, 12);
            numNam = N(650, 15, 75, 2000, 2100);
            cboDichVu = C(90, 55, 200);
            numSoLuong = N(370, 55, 100, 0, 1000000, 2);
            gridChiTietTam = G(10, 95, 600, 150);
            lblTong = new Label { Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold) };
            lblTong.SetBounds(10, 252, 400, 24);
            gridHoaDon = G(10, 320, 560, 210);
            gridHoaDon.SelectionChanged += new EventHandler(gridHoaDon_SelectionChanged);
            gridChiTietHD = G(580, 320, 410, 210);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(1000, 580);
            Controls.AddRange(new Control[] {
                L("Hop dong:", 10, 15, 75), cboHopDong, L("Thang:", 490, 15, 50), numThang, L("Nam:", 610, 15, 40), numNam,
                L("Dich vu:", 10, 55, 75), cboDichVu, L("So luong:", 300, 55, 65), numSoLuong,
                B("Them", 480, 53, 80, new EventHandler(btnThemDV_Click)),
                B("Xoa dong", 565, 53, 85, new EventHandler(btnXoaDV_Click)),
                gridChiTietTam, lblTong,
                B("Lap hoa don cuoi thang", 630, 95, 200, new EventHandler(btnLapHoaDon_Click), 40),
                L("Hoa don dich vu cua hop dong:", 10, 295, 300), L("Chi tiet hoa don:", 580, 295, 200),
                gridHoaDon, gridChiTietHD,
                B("Thanh toan hoa don chon", 10, 538, 200, new EventHandler(btnThanhToan_Click), 32) });
            Text = "Hoa don dich vu (dien, nuoc, internet...)";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
