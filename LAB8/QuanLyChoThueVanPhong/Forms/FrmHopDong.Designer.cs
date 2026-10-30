using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmHopDong
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpLap;
        private ComboBox cboKhachHang, cboPhong;
        private DateTimePicker dtpBatDau;
        private NumericUpDown numSoThang;
        private TextBox txtNoiDung, txtNguoiThu;
        private Label lblTongTien;
        private DataGridView gridPhongChon, gridHopDong, gridChiTiet;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            grpLap = new GroupBox { Text = "Lap hop dong thue van phong" };
            grpLap.SetBounds(10, 10, 980, 300);
            cboKhachHang = C(110, 25, 330);
            dtpBatDau = D(560, 25, 120);
            numSoThang = N(800, 25, 60, 6, 120);
            txtNoiDung = new TextBox { Multiline = true };
            txtNoiDung.SetBounds(110, 60, 570, 45);
            txtNguoiThu = T(800, 60, 160);
            cboPhong = C(110, 120, 330);
            gridPhongChon = G(15, 155, 665, 100);
            lblTongTien = new Label { Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold) };
            lblTongTien.SetBounds(15, 265, 660, 22);
            grpLap.Controls.AddRange(new Control[] {
                L("Khach hang:", 15, 25, 90), cboKhachHang, L("Bat dau:", 470, 25, 85), dtpBatDau,
                L("So thang:", 700, 25, 95), numSoThang,
                L("Noi dung:", 15, 60, 90), txtNoiDung, L("Nguoi thu tien:", 700, 60, 100), txtNguoiThu,
                L("Phong trong:", 15, 120, 90), cboPhong,
                B("Them phong", 450, 118, 110, new EventHandler(btnThemPhong_Click)),
                B("Xoa dong", 570, 118, 110, new EventHandler(btnXoaPhong_Click)),
                gridPhongChon, lblTongTien,
                B("Lap hop dong", 700, 155, 260, new EventHandler(btnLapHopDong_Click), 45),
                L("(thue it nhat 6 thang, tra truoc 6 thang khi ky)", 700, 205, 270) });

            gridHopDong = G(10, 345, 600, 230);
            gridHopDong.SelectionChanged += new EventHandler(gridHopDong_SelectionChanged);
            gridChiTiet = G(620, 345, 370, 230);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(1000, 625);
            Controls.AddRange(new Control[] { grpLap, L("Danh sach hop dong:", 10, 320, 200), L("Phong cua hop dong:", 620, 320, 200),
                                              gridHopDong, gridChiTiet,
                                              B("Thanh ly hop dong chon", 10, 583, 200, new EventHandler(btnThanhLy_Click), 32) });
            Text = "Hop dong thue van phong";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
