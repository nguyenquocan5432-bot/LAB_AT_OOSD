using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmGiaHan
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboHopDong;
        private NumericUpDown numSoThang;
        private TextBox txtNguoiLap;
        private Label lblToiDa, lblDieuKien;
        private DataGridView gridPhong, gridPhieu;

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
            lblDieuKien = new Label { Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold) };
            lblDieuKien.SetBounds(10, 50, 880, 24);
            gridPhong = G(10, 105, 880, 140);
            numSoThang = N(130, 260, 60, 1, 60);
            lblToiDa = new Label();
            lblToiDa.SetBounds(200, 263, 330, 23);
            txtNguoiLap = T(620, 260, 170);
            gridPhieu = G(10, 345, 880, 180);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 540);
            Controls.AddRange(new Control[] {
                L("Hop dong:", 10, 15, 75), cboHopDong, lblDieuKien,
                L("Chon phong can gia han:", 10, 80, 300), gridPhong,
                L("So thang gia han:", 10, 260, 115), numSoThang, lblToiDa, L("Nguoi lap:", 545, 260, 70), txtNguoiLap,
                B("Lap phieu gia han", 10, 295, 200, new EventHandler(btnLapPhieu_Click), 34),
                L("Cac phieu gia han da lap:", 230, 322, 300), gridPhieu });
            Text = "Lap phieu gia han thue van phong";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
