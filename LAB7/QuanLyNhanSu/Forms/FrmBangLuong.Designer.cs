using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmBangLuong
    {
        private System.ComponentModel.IContainer components = null;
        private NumericUpDown numThang, numNam;
        private Label lblTong, lblCongThuc;
        private DataGridView gridBangLuong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            numThang = N(70, 15, 55, 1, 12);
            numNam = N(180, 15, 75, 2000, 2100);
            lblCongThuc = new Label
            {
                Text = "Thuc lanh = LCB/26 x cong + phu cap + gio phu troi x (LCB/26/8) x 1.5 + thuong - phat - BHXH 8% - BHYT 1.5%",
                ForeColor = System.Drawing.Color.DimGray
            };
            lblCongThuc.SetBounds(10, 55, 960, 20);
            gridBangLuong = G(10, 80, 960, 400);
            lblTong = new Label { Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold) };
            lblTong.SetBounds(10, 490, 600, 25);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(980, 525);
            Controls.AddRange(new Control[] {
                L("Thang:", 10, 15, 55), numThang, L("Nam:", 135, 15, 40), numNam,
                B("Xem bang luong", 275, 13, 130, new EventHandler(btnXem_Click)),
                B("Tinh luong thang", 415, 13, 140, new EventHandler(btnTinhLuong_Click)),
                lblCongThuc, gridBangLuong, lblTong });
            Text = "Bang luong thang";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
