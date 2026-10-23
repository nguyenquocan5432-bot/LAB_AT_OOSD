using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmDatLichDinhKy
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMaHoSo, lblNgayHen, lblGioHen, lblMaPhieuKhamGan;
        private TextBox txtMaHoSo, txtMaPhieuKhamGan;
        private DateTimePicker dtpNgayHen, dtpGioHen;
        private Button btnXemLich, btnDatLich, btnHuyLich, btnHoanTat;
        private DataGridView gridLichHen;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMaHoSo = new Label { Text = "Ma ho so:" }; lblMaHoSo.SetBounds(15, 20, 70, 23);
            txtMaHoSo = new TextBox(); txtMaHoSo.SetBounds(90, 20, 150, 23);
            btnXemLich = new Button { Text = "Xem lich hen" }; btnXemLich.SetBounds(250, 18, 110, 27);
            btnXemLich.Click += new EventHandler(this.btnXemLich_Click);

            lblNgayHen = new Label { Text = "Ngay hen:" }; lblNgayHen.SetBounds(15, 55, 70, 23);
            dtpNgayHen = new DateTimePicker { Format = DateTimePickerFormat.Short }; dtpNgayHen.SetBounds(90, 55, 130, 23);
            lblGioHen = new Label { Text = "Gio hen:" }; lblGioHen.SetBounds(235, 55, 55, 23);
            dtpGioHen = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true }; dtpGioHen.SetBounds(295, 55, 100, 23);
            btnDatLich = new Button { Text = "Dat lich kham dinh ky" }; btnDatLich.SetBounds(410, 53, 170, 27);
            btnDatLich.Click += new EventHandler(this.btnDatLich_Click);

            gridLichHen = new DataGridView(); gridLichHen.SetBounds(15, 95, 700, 250);
            gridLichHen.ReadOnly = true; gridLichHen.AllowUserToAddRows = false;

            btnHuyLich = new Button { Text = "Huy lich da chon" }; btnHuyLich.SetBounds(15, 355, 150, 30);
            btnHuyLich.Click += new EventHandler(this.btnHuyLich_Click);

            lblMaPhieuKhamGan = new Label { Text = "Ma phieu kham dinh ky:" }; lblMaPhieuKhamGan.SetBounds(185, 360, 150, 23);
            txtMaPhieuKhamGan = new TextBox(); txtMaPhieuKhamGan.SetBounds(335, 358, 130, 23);
            btnHoanTat = new Button { Text = "Hoan tat lich da chon" }; btnHoanTat.SetBounds(475, 355, 170, 30);
            btnHoanTat.Click += new EventHandler(this.btnHoanTat_Click);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(730, 420);
            this.Controls.Add(lblMaHoSo); this.Controls.Add(txtMaHoSo); this.Controls.Add(btnXemLich);
            this.Controls.Add(lblNgayHen); this.Controls.Add(dtpNgayHen);
            this.Controls.Add(lblGioHen); this.Controls.Add(dtpGioHen);
            this.Controls.Add(btnDatLich);
            this.Controls.Add(gridLichHen);
            this.Controls.Add(btnHuyLich);
            this.Controls.Add(lblMaPhieuKhamGan); this.Controls.Add(txtMaPhieuKhamGan);
            this.Controls.Add(btnHoanTat);
            this.Text = "Dat lich kham dinh ky";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
