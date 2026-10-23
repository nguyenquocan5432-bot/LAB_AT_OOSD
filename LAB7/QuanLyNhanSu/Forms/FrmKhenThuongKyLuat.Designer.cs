using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmKhenThuongKyLuat
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabMain;
        private TabPage tabKhenThuong, tabKyLuat;

        private RadioButton radCaNhanKT, radTapTheKT;
        private ComboBox cboNhanVienKT, cboPhongBanKT, cboLoaiKT, cboHinhThucChi;
        private TextBox txtLyDoKT, txtSoTienKT, txtNguoiKyKT, txtNguoiNhan;
        private DataGridView gridKhenThuong;

        private RadioButton radCaNhanKL, radTapTheKL;
        private ComboBox cboNhanVienKL, cboPhongBanKL, cboHinhThucKL, cboHinhThucThu;
        private TextBox txtLyDoKL, txtSoTienKL, txtNguoiKyKL, txtNguoiNop;
        private DataGridView gridKyLuat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            tabMain = new TabControl();
            tabMain.SetBounds(10, 10, 900, 540);
            tabKhenThuong = new TabPage { Text = "Quyet dinh khen thuong" };
            tabKyLuat = new TabPage { Text = "Quyet dinh ky luat" };
            tabMain.TabPages.AddRange(new TabPage[] { tabKhenThuong, tabKyLuat });

            // ---- Khen thuong ----
            radCaNhanKT = new RadioButton { Text = "Ca nhan" }; radCaNhanKT.SetBounds(110, 15, 80, 23);
            radTapTheKT = new RadioButton { Text = "Tap the phong ban" }; radTapTheKT.SetBounds(200, 15, 150, 23);
            radCaNhanKT.CheckedChanged += new EventHandler(radDoiTuongKT_CheckedChanged);
            cboNhanVienKT = C(110, 45, 260);
            cboPhongBanKT = C(500, 45, 240);
            cboLoaiKT = C(110, 80, 160);
            txtSoTienKT = T(370, 80, 130);
            cboHinhThucChi = C(610, 80, 130);
            txtLyDoKT = T(110, 115, 630);
            txtNguoiKyKT = T(110, 150, 260);
            txtNguoiNhan = T(500, 150, 240);
            gridKhenThuong = G(15, 235, 860, 265);
            tabKhenThuong.Controls.AddRange(new Control[] {
                L("Doi tuong:", 15, 15, 90), radCaNhanKT, radTapTheKT,
                L("Nhan vien:", 15, 45, 90), cboNhanVienKT, L("Phong ban:", 400, 45, 90), cboPhongBanKT,
                L("Loai:", 15, 80, 90), cboLoaiKT, L("So tien:", 290, 80, 75), txtSoTienKT,
                L("Hinh thuc chi:", 515, 80, 95), cboHinhThucChi,
                L("Ly do:", 15, 115, 90), txtLyDoKT,
                L("Nguoi ky:", 15, 150, 90), txtNguoiKyKT, L("Nguoi nhan:", 400, 150, 90), txtNguoiNhan,
                B("Lap quyet dinh khen thuong", 15, 190, 230, new EventHandler(btnLapKT_Click), 34),
                L("PHIEU_CHI: lap phieu chi ngay / BANG_LUONG: cong vao luong thang", 260, 198, 480) , gridKhenThuong });

            // ---- Ky luat ----
            radCaNhanKL = new RadioButton { Text = "Ca nhan" }; radCaNhanKL.SetBounds(110, 15, 80, 23);
            radTapTheKL = new RadioButton { Text = "Tap the phong ban" }; radTapTheKL.SetBounds(200, 15, 150, 23);
            radCaNhanKL.CheckedChanged += new EventHandler(radDoiTuongKL_CheckedChanged);
            cboNhanVienKL = C(110, 45, 260);
            cboPhongBanKL = C(500, 45, 240);
            cboHinhThucKL = new ComboBox(); cboHinhThucKL.SetBounds(110, 80, 160, 23);
            txtSoTienKL = T(370, 80, 130);
            cboHinhThucThu = C(610, 80, 130);
            txtLyDoKL = T(110, 115, 630);
            txtNguoiKyKL = T(110, 150, 260);
            txtNguoiNop = T(500, 150, 240);
            gridKyLuat = G(15, 235, 860, 265);
            tabKyLuat.Controls.AddRange(new Control[] {
                L("Doi tuong:", 15, 15, 90), radCaNhanKL, radTapTheKL,
                L("Nhan vien:", 15, 45, 90), cboNhanVienKL, L("Phong ban:", 400, 45, 90), cboPhongBanKL,
                L("Hinh thuc KL:", 15, 80, 90), cboHinhThucKL, L("Tien phat:", 290, 80, 75), txtSoTienKL,
                L("Cach thu:", 515, 80, 95), cboHinhThucThu,
                L("Ly do:", 15, 115, 90), txtLyDoKL,
                L("Nguoi ky:", 15, 150, 90), txtNguoiKyKL, L("Nguoi nop:", 400, 150, 90), txtNguoiNop,
                B("Lap quyet dinh ky luat", 15, 190, 230, new EventHandler(btnLapKL_Click), 34), gridKyLuat });

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(920, 560);
            Controls.Add(tabMain);
            Text = "Khen thuong / Ky luat";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
