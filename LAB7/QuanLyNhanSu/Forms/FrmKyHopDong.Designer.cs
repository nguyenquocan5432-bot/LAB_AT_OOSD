using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmKyHopDong
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpHopDong, grpLuong, grpBaoHiem;
        private ComboBox cboNhanVien, cboLoaiHD, cboChucVu;
        private Label lblThuViec;
        private DateTimePicker dtpBatDau, dtpKetThuc;
        private TextBox txtNoiDung, txtNguoiKy;
        private TextBox txtLuongCoBan, txtPhuCapLuong, txtPhuCapDocHai, txtPhuCapCV;
        private TextBox txtSoBHXH, txtSoBHYT, txtNoiKCB;
        private Button btnKyHopDong;
        private DataGridView gridHopDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // ---- Hop dong ----
            grpHopDong = new GroupBox { Text = "Hop dong lao dong" };
            grpHopDong.SetBounds(10, 10, 450, 225);
            cboNhanVien = C(120, 25, 310);
            cboNhanVien.SelectedIndexChanged += new EventHandler(cboNhanVien_SelectedIndexChanged);
            lblThuViec = new Label { ForeColor = System.Drawing.Color.DarkBlue };
            lblThuViec.SetBounds(120, 52, 310, 20);
            cboLoaiHD = C(120, 78, 150);
            cboLoaiHD.SelectedIndexChanged += new EventHandler(cboLoaiHD_SelectedIndexChanged);
            dtpBatDau = D(120, 113, 120);
            dtpKetThuc = D(310, 113, 120);
            txtNoiDung = T(120, 148, 310);
            txtNguoiKy = T(120, 183, 310);
            grpHopDong.Controls.AddRange(new Control[] {
                L("NV thu viec:", 15, 25, 100), cboNhanVien, lblThuViec,
                L("Loai hop dong:", 15, 78, 100), cboLoaiHD,
                L("Bat dau:", 15, 113, 100), dtpBatDau, L("Ket thuc:", 250, 113, 60), dtpKetThuc,
                L("Noi dung:", 15, 148, 100), txtNoiDung,
                L("Nguoi ky:", 15, 183, 100), txtNguoiKy });

            // ---- Quyet dinh luong + chuc vu ----
            grpLuong = new GroupBox { Text = "Quyet dinh luong / chuc vu" };
            grpLuong.SetBounds(470, 10, 420, 225);
            txtLuongCoBan = T(140, 25, 150);
            txtPhuCapLuong = T(140, 60, 150);
            txtPhuCapDocHai = T(140, 95, 150);
            cboChucVu = C(140, 140, 260);
            cboChucVu.SelectedIndexChanged += new EventHandler(cboChucVu_SelectedIndexChanged);
            txtPhuCapCV = T(140, 175, 150);
            grpLuong.Controls.AddRange(new Control[] {
                L("Luong co ban:", 15, 25, 120), txtLuongCoBan,
                L("Phu cap luong:", 15, 60, 120), txtPhuCapLuong,
                L("Phu cap doc hai:", 15, 95, 120), txtPhuCapDocHai,
                L("Chuc vu (neu co):", 15, 140, 120), cboChucVu,
                L("Phu cap chuc vu:", 15, 175, 120), txtPhuCapCV });

            // ---- Ho so bao hiem ----
            grpBaoHiem = new GroupBox { Text = "Ho so BHXH / BHYT (hop dong dai han)" };
            grpBaoHiem.SetBounds(10, 240, 880, 65);
            txtSoBHXH = T(80, 25, 150);
            txtSoBHYT = T(310, 25, 170);
            txtNoiKCB = T(590, 25, 270);
            grpBaoHiem.Controls.AddRange(new Control[] {
                L("So BHXH:", 15, 25, 65), txtSoBHXH, L("So BHYT:", 245, 25, 65), txtSoBHYT,
                L("Noi KCB:", 520, 25, 65), txtNoiKCB });

            btnKyHopDong = B("Ky hop dong chinh thuc", 10, 315, 220, new EventHandler(btnKyHopDong_Click), 34);
            gridHopDong = G(10, 360, 880, 200);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 575);
            Controls.AddRange(new Control[] { grpHopDong, grpLuong, grpBaoHiem, btnKyHopDong, gridHopDong });
            Text = "Ky hop dong chinh thuc";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
