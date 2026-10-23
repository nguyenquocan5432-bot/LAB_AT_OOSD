using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle, lblCongTy;
        private Button btnDanhMuc, btnTuyenDung, btnKyHopDong, btnQuyetDinh, btnPhanCong, btnChamCong, btnKhenThuong, btnBangLuong, btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            lblTitle = new Label { Text = "QUAN LY NHAN SU", Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold) };
            lblTitle.SetBounds(30, 15, 480, 32);
            lblCongTy = new Label { Text = "Cong ty son Dong A" };
            lblCongTy.SetBounds(32, 48, 300, 20);

            btnDanhMuc    = B("1. Danh muc (PB / chuc vu / ca)", 30, 80, 230, new EventHandler(btnDanhMuc_Click), 40);
            btnTuyenDung  = B("2. Quyet dinh tuyen dung", 30, 130, 230, new EventHandler(btnTuyenDung_Click), 40);
            btnKyHopDong  = B("3. Ky hop dong chinh thuc", 30, 180, 230, new EventHandler(btnKyHopDong_Click), 40);
            btnQuyetDinh  = B("4. QD luong / chuc vu", 30, 230, 230, new EventHandler(btnQuyetDinh_Click), 40);
            btnPhanCong   = B("5. Ke hoach phan cong tuan", 280, 80, 230, new EventHandler(btnPhanCong_Click), 40);
            btnChamCong   = B("6. Cham cong / Phu troi", 280, 130, 230, new EventHandler(btnChamCong_Click), 40);
            btnKhenThuong = B("7. Khen thuong / Ky luat", 280, 180, 230, new EventHandler(btnKhenThuong_Click), 40);
            btnBangLuong  = B("8. Tinh bang luong thang", 280, 230, 230, new EventHandler(btnBangLuong_Click), 40);
            btnThoat      = B("Thoat", 280, 285, 230, new EventHandler(btnThoat_Click), 32);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(545, 335);
            Controls.AddRange(new Control[] { lblTitle, lblCongTy, btnDanhMuc, btnTuyenDung, btnKyHopDong, btnQuyetDinh,
                                              btnPhanCong, btnChamCong, btnKhenThuong, btnBangLuong, btnThoat });
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Quan ly nhan su - Cong ty son Dong A";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
