using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmThanhToanThue
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboHopDong;
        private TextBox txtNguoiThu;
        private Label lblTinhTrang;
        private DataGridView gridPhieuThu, gridPhong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            cboHopDong = C(100, 15, 380);
            cboHopDong.SelectedIndexChanged += new EventHandler(cboHopDong_SelectedIndexChanged);
            txtNguoiThu = T(580, 15, 160);
            lblTinhTrang = new Label { Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.DarkBlue };
            lblTinhTrang.SetBounds(10, 50, 880, 24);
            gridPhong = G(10, 105, 880, 120);
            gridPhieuThu = G(10, 260, 880, 230);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 545);
            Controls.AddRange(new Control[] {
                L("Hop dong:", 10, 15, 85), cboHopDong, L("Nguoi thu:", 500, 15, 75), txtNguoiThu, lblTinhTrang,
                L("Phong dang thue (gia thang):", 10, 80, 300), gridPhong,
                L("Cac phieu thu tien thue:", 10, 235, 300), gridPhieuThu,
                B("Thu tien thue ky ke tiep", 10, 500, 220, new EventHandler(btnThu_Click), 34),
                L("Ky 1-6 tra truoc khi ky hop dong; tu ky 7 tra vao dau moi thang", 245, 508, 450) });
            Text = "Thu tien thue hang thang";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
