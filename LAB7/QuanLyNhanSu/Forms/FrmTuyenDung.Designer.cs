using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmTuyenDung
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpNhanVien, grpQuyetDinh;
        private TextBox txtHoTen, txtCCCD, txtDiaChi, txtDienThoai;
        private DateTimePicker dtpNgaySinh;
        private ComboBox cboGioiTinh, cboPhongBan;
        private DateTimePicker dtpBatDauTV, dtpKetThucTV;
        private TextBox txtLuongTV, txtDieuKhoanCT, txtDieuKhoanNV, txtNguoiKy;
        private Button btnLapQD, btnChamDut;
        private DataGridView gridThuViec;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // ---- Thong tin nguoi duoc tuyen dung ----
            grpNhanVien = new GroupBox { Text = "Nguoi duoc tuyen dung" };
            grpNhanVien.SetBounds(10, 10, 400, 215);
            txtHoTen = T(110, 25, 270);
            dtpNgaySinh = D(110, 60, 130);
            cboGioiTinh = C(300, 60, 80);
            txtCCCD = T(110, 95, 270);
            txtDiaChi = T(110, 130, 270);
            txtDienThoai = T(110, 165, 270);
            grpNhanVien.Controls.AddRange(new Control[] {
                L("Ho ten:", 15, 25, 90), txtHoTen,
                L("Ngay sinh:", 15, 60, 90), dtpNgaySinh, L("Gioi tinh:", 245, 60, 55), cboGioiTinh,
                L("So CCCD:", 15, 95, 90), txtCCCD,
                L("Dia chi:", 15, 130, 90), txtDiaChi,
                L("Dien thoai:", 15, 165, 90), txtDienThoai });

            // ---- Noi dung quyet dinh tuyen dung ----
            grpQuyetDinh = new GroupBox { Text = "Quyet dinh tuyen dung" };
            grpQuyetDinh.SetBounds(420, 10, 470, 215);
            cboPhongBan = C(150, 25, 300);
            dtpBatDauTV = D(150, 60, 120);
            dtpKetThucTV = D(330, 60, 120);
            txtLuongTV = T(150, 95, 140);
            txtDieuKhoanCT = T(150, 130, 300);
            txtDieuKhoanNV = T(150, 160, 300);
            txtNguoiKy = T(150, 188, 300);
            grpQuyetDinh.Controls.AddRange(new Control[] {
                L("Phong ban can tuyen:", 15, 25, 135), cboPhongBan,
                L("Thu viec tu ngay:", 15, 60, 135), dtpBatDauTV, L("den:", 290, 60, 35), dtpKetThucTV,
                L("Luong thu viec:", 15, 95, 135), txtLuongTV,
                L("Dieu khoan cong ty:", 15, 130, 135), txtDieuKhoanCT,
                L("Dieu khoan NV:", 15, 160, 135), txtDieuKhoanNV,
                L("Nguoi ky:", 15, 188, 135), txtNguoiKy });

            btnLapQD = B("Lap quyet dinh tuyen dung", 10, 235, 220, new EventHandler(btnLapQD_Click), 34);
            btnChamDut = B("Danh gia khong dat - cham dut thu viec", 600, 235, 290, new EventHandler(btnChamDut_Click), 34);
            gridThuViec = G(10, 280, 880, 250);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 545);
            Controls.AddRange(new Control[] { grpNhanVien, grpQuyetDinh, btnLapQD, btnChamDut,
                                              L("Nhan vien dang thu viec:", 250, 242, 200), gridThuViec });
            Text = "Lap quyet dinh tuyen dung";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
