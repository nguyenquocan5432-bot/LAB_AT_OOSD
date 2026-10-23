using System;
using System.Windows.Forms;
using static QuanLyNhanSu.Forms.FormHelper;

namespace QuanLyNhanSu.Forms
{
    partial class FrmPhanCong
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboPhongBan, cboNhanVien, cboCa;
        private DateTimePicker dtpTuNgay, dtpNgayLam;
        private Label lblDenNgay;
        private TextBox txtNguoiLap;
        private DataGridView gridTam, gridKeHoach;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            cboPhongBan = C(110, 15, 220);
            cboPhongBan.SelectedIndexChanged += new EventHandler(cboPhongBan_SelectedIndexChanged);
            dtpTuNgay = D(450, 15, 120);
            dtpTuNgay.ValueChanged += new EventHandler(dtpTuNgay_ValueChanged);
            lblDenNgay = new Label();
            lblDenNgay.SetBounds(580, 18, 130, 23);
            txtNguoiLap = T(110, 50, 220);
            txtNguoiLap.Text = "Truong phong";

            cboNhanVien = C(110, 95, 220);
            dtpNgayLam = D(410, 95, 120);
            cboCa = C(580, 95, 130);
            gridTam = G(10, 170, 430, 300);
            gridKeHoach = G(450, 170, 440, 340);

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            ClientSize = new System.Drawing.Size(900, 525);
            Controls.AddRange(new Control[] {
                L("Phong ban:", 10, 15, 95), cboPhongBan, L("Tuan tu ngay:", 350, 15, 95), dtpTuNgay, lblDenNgay,
                L("Nguoi lap:", 10, 50, 95), txtNguoiLap,
                L("Nhan vien:", 10, 95, 95), cboNhanVien, L("Ngay:", 350, 95, 55), dtpNgayLam, L("Ca:", 545, 95, 30), cboCa,
                B("Them dong", 720, 93, 80, new EventHandler(btnThem_Click)),
                B("Xoa dong", 805, 93, 85, new EventHandler(btnXoa_Click)),
                L("Phan cong dang lap:", 10, 145, 200), L("Cac ke hoach da lap cua phong ban:", 450, 145, 300),
                gridTam, gridKeHoach,
                B("Luu ke hoach phan cong", 10, 478, 200, new EventHandler(btnLuu_Click), 34) });
            Text = "Ke hoach phan cong theo tuan";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
