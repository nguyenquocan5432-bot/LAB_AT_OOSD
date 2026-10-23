using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmQuyetDinh
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboNhanVien, cboChucVu;
        private TextBox txtNguoiKy;
        private GroupBox grpLuong, grpChucVu;
        private DateTimePicker dtpHieuLucLuong, dtpHieuLucCV;
        private TextBox txtLuongCoBan, txtPhuCapLuong, txtPhuCapDocHai, txtGhiChu, txtPhuCapCV;
        private DataGridView gridLichSu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            cboNhanVien = C(130, 15, 300);
            cboNhanVien.SelectedIndexChanged += new EventHandler(cboNhanVien_SelectedIndexChanged);
            txtNguoiKy = T(530, 15, 260);

            // ---- Quyet dinh luong (tang luong) ----
            grpLuong = new GroupBox { Text = "Quyet dinh luong (tang luong)" };
            grpLuong.SetBounds(10, 50, 430, 225);
            dtpHieuLucLuong = D(130, 25, 130);
            txtLuongCoBan = T(130, 60, 150);
            txtPhuCapLuong = T(130, 95, 150);
            txtPhuCapDocHai = T(130, 130, 150);
            txtGhiChu = T(130, 165, 280);
            grpLuong.Controls.AddRange(new Control[] {
                L("Ngay hieu luc:", 15, 25, 110), dtpHieuLucLuong,
                L("Luong co ban:", 15, 60, 110), txtLuongCoBan,
                L("Phu cap luong:", 15, 95, 110), txtPhuCapLuong,
                L("Phu cap doc hai:", 15, 130, 110), txtPhuCapDocHai,
                L("Ghi chu:", 15, 165, 110), txtGhiChu,
                B("Lap QD luong", 290, 58, 120, new EventHandler(btnLapQDLuong_Click), 32) });

            // ---- Quyet dinh chuc vu (bo nhiem) ----
            grpChucVu = new GroupBox { Text = "Quyet dinh chuc vu (bo nhiem)" };
            grpChucVu.SetBounds(450, 50, 440, 225);
            cboChucVu = C(130, 25, 290);
            cboChucVu.SelectedIndexChanged += new EventHandler(cboChucVu_SelectedIndexChanged);
            dtpHieuLucCV = D(130, 60, 130);
            txtPhuCapCV = T(130, 95, 150);
            grpChucVu.Controls.AddRange(new Control[] {
                L("Chuc vu:", 15, 25, 110), cboChucVu,
                L("Ngay hieu luc:", 15, 60, 110), dtpHieuLucCV,
                L("Phu cap chuc vu:", 15, 95, 110), txtPhuCapCV,
                B("Lap QD chuc vu", 130, 135, 150, new EventHandler(btnLapQDChucVu_Click), 32) });

            gridLichSu = G(10, 305, 880, 230);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 550);
            Controls.AddRange(new Control[] {
                L("NV chinh thuc:", 10, 15, 115), cboNhanVien, L("Nguoi ky:", 455, 15, 70), txtNguoiKy,
                grpLuong, grpChucVu, L("Qua trinh tang luong / bo nhiem cua nhan vien:", 10, 280, 400), gridLichSu });
            Text = "Quyet dinh luong / chuc vu";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
