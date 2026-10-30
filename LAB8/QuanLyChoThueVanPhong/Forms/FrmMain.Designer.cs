using System;
using System.Windows.Forms;
using static QuanLyChoThueVanPhong.Forms.FormHelper;

namespace QuanLyChoThueVanPhong.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle, lblPhu;
        private Button btnDanhMuc, btnHopDong, btnThanhToan, btnDichVu, btnGiaHan, btnNhanVien, btnKiemTra, btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            lblTitle = new Label { Text = "QUAN LY CHO THUE VAN PHONG", Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold) };
            lblTitle.SetBounds(30, 15, 480, 32);
            lblPhu = new Label { Text = "Bo phan quan ly cao oc" };
            lblPhu.SetBounds(32, 48, 300, 20);

            btnDanhMuc   = B("1. Danh muc (tang, phong, KH, DV)", 30, 80, 230, new EventHandler(btnDanhMuc_Click), 40);
            btnHopDong   = B("2. Hop dong thue / thanh ly", 30, 130, 230, new EventHandler(btnHopDong_Click), 40);
            btnThanhToan = B("3. Thu tien thue hang thang", 30, 180, 230, new EventHandler(btnThanhToan_Click), 40);
            btnDichVu    = B("4. Hoa don dich vu", 30, 230, 230, new EventHandler(btnDichVu_Click), 40);
            btnGiaHan    = B("5. Lap phieu gia han", 280, 80, 230, new EventHandler(btnGiaHan_Click), 40);
            btnNhanVien  = B("6. Nhan vien / the giu xe", 280, 130, 230, new EventHandler(btnNhanVien_Click), 40);
            btnKiemTra   = B("7. Kiem tra phong / phieu phat", 280, 180, 230, new EventHandler(btnKiemTra_Click), 40);
            btnThoat     = B("Thoat", 280, 230, 230, new EventHandler(btnThoat_Click), 40);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(545, 295);
            Controls.AddRange(new Control[] { lblTitle, lblPhu, btnDanhMuc, btnHopDong, btnThanhToan, btnDichVu,
                                              btnGiaHan, btnNhanVien, btnKiemTra, btnThoat });
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Quan ly cho thue van phong cao oc";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
