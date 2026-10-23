using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmLapPhieuKham
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpLapPhieu, grpXetNghiem, grpKetLuan;

        private Label lblMaHoSo, lblBacSi, lblKhoa, lblLoaiKham, lblTrieuChung, lblThanNhiet, lblHuyetAp, lblMoTaThem;
        private TextBox txtMaHoSo, txtTrieuChung, txtThanNhiet, txtHuyetAp, txtMoTaThem;
        private ComboBox cboBacSi, cboKhoa, cboLoaiKham;
        private Button btnLapPhieu;

        private Label lblMaPhieuThaoTac, lblLoaiXetNghiem, lblKetQuaXN;
        private TextBox txtMaPhieuThaoTac, txtLoaiXetNghiem, txtKetQuaXN;
        private Button btnChiDinhXN, btnXemXN, btnCapNhatKetQuaXN;
        private DataGridView gridXetNghiem;

        private Label lblKetLuan;
        private TextBox txtKetLuan;
        private Button btnGhiKetLuan;

        private DataGridView gridPhieuKham;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            grpLapPhieu = new GroupBox { Text = "Lap phieu kham benh" };
            grpXetNghiem = new GroupBox { Text = "Xet nghiem" };
            grpKetLuan = new GroupBox { Text = "Ket luan" };

            lblMaHoSo = new Label { Text = "Ma ho so:" }; lblMaHoSo.SetBounds(15, 25, 70, 23);
            txtMaHoSo = new TextBox(); txtMaHoSo.SetBounds(90, 25, 130, 23);
            lblBacSi = new Label { Text = "Bac si:" }; lblBacSi.SetBounds(235, 25, 50, 23);
            cboBacSi = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; cboBacSi.SetBounds(290, 25, 170, 23);
            lblKhoa = new Label { Text = "Khoa:" }; lblKhoa.SetBounds(475, 25, 45, 23);
            cboKhoa = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; cboKhoa.SetBounds(520, 25, 150, 23);
            lblLoaiKham = new Label { Text = "Loai kham:" }; lblLoaiKham.SetBounds(15, 58, 70, 23);
            cboLoaiKham = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; cboLoaiKham.SetBounds(90, 58, 120, 23);

            lblTrieuChung = new Label { Text = "Trieu chung:" }; lblTrieuChung.SetBounds(15, 91, 70, 23);
            txtTrieuChung = new TextBox(); txtTrieuChung.SetBounds(90, 91, 230, 23);
            lblThanNhiet = new Label { Text = "Than nhiet:" }; lblThanNhiet.SetBounds(330, 91, 65, 23);
            txtThanNhiet = new TextBox(); txtThanNhiet.SetBounds(400, 91, 60, 23);
            lblHuyetAp = new Label { Text = "Huyet ap:" }; lblHuyetAp.SetBounds(470, 91, 60, 23);
            txtHuyetAp = new TextBox(); txtHuyetAp.SetBounds(535, 91, 80, 23);

            lblMoTaThem = new Label { Text = "Mo ta them:" }; lblMoTaThem.SetBounds(15, 124, 70, 23);
            txtMoTaThem = new TextBox(); txtMoTaThem.SetBounds(90, 124, 400, 23);
            btnLapPhieu = new Button { Text = "Lap phieu kham" }; btnLapPhieu.SetBounds(520, 120, 150, 30);
            btnLapPhieu.Click += new EventHandler(this.btnLapPhieu_Click);

            grpLapPhieu.SetBounds(10, 10, 900, 165);
            grpLapPhieu.Controls.AddRange(new Control[] {
                lblMaHoSo, txtMaHoSo, lblBacSi, cboBacSi, lblKhoa, cboKhoa, lblLoaiKham, cboLoaiKham,
                lblTrieuChung, txtTrieuChung, lblThanNhiet, txtThanNhiet, lblHuyetAp, txtHuyetAp,
                lblMoTaThem, txtMoTaThem, btnLapPhieu });

            // ---- Xet nghiem ----
            lblMaPhieuThaoTac = new Label { Text = "Ma phieu kham thao tac:" }; lblMaPhieuThaoTac.SetBounds(15, 25, 150, 23);
            txtMaPhieuThaoTac = new TextBox(); txtMaPhieuThaoTac.SetBounds(170, 25, 150, 23);
            lblLoaiXetNghiem = new Label { Text = "Loai xet nghiem:" }; lblLoaiXetNghiem.SetBounds(340, 25, 100, 23);
            txtLoaiXetNghiem = new TextBox(); txtLoaiXetNghiem.SetBounds(445, 25, 180, 23);
            btnChiDinhXN = new Button { Text = "Chi dinh" }; btnChiDinhXN.SetBounds(635, 23, 90, 27);
            btnChiDinhXN.Click += new EventHandler(this.btnChiDinhXN_Click);
            btnXemXN = new Button { Text = "Xem DS xet nghiem" }; btnXemXN.SetBounds(735, 23, 140, 27);
            btnXemXN.Click += new EventHandler(this.btnXemXN_Click);

            gridXetNghiem = new DataGridView(); gridXetNghiem.SetBounds(15, 60, 860, 110);
            gridXetNghiem.ReadOnly = true; gridXetNghiem.AllowUserToAddRows = false;

            lblKetQuaXN = new Label { Text = "Ket qua (chon dong tren):" }; lblKetQuaXN.SetBounds(15, 180, 150, 23);
            txtKetQuaXN = new TextBox(); txtKetQuaXN.SetBounds(170, 180, 455, 23);
            btnCapNhatKetQuaXN = new Button { Text = "Cap nhat ket qua" }; btnCapNhatKetQuaXN.SetBounds(635, 178, 150, 27);
            btnCapNhatKetQuaXN.Click += new EventHandler(this.btnCapNhatKetQuaXN_Click);

            grpXetNghiem.SetBounds(10, 185, 900, 225);
            grpXetNghiem.Controls.AddRange(new Control[] {
                lblMaPhieuThaoTac, txtMaPhieuThaoTac, lblLoaiXetNghiem, txtLoaiXetNghiem, btnChiDinhXN, btnXemXN,
                gridXetNghiem, lblKetQuaXN, txtKetQuaXN, btnCapNhatKetQuaXN });

            // ---- Ket luan ----
            lblKetLuan = new Label { Text = "Ket luan kham (ap dung cho ma phieu o tren):" }; lblKetLuan.SetBounds(15, 25, 280, 23);
            txtKetLuan = new TextBox(); txtKetLuan.SetBounds(15, 50, 650, 23);
            btnGhiKetLuan = new Button { Text = "Ghi ket luan" }; btnGhiKetLuan.SetBounds(680, 48, 150, 27);
            btnGhiKetLuan.Click += new EventHandler(this.btnGhiKetLuan_Click);
            grpKetLuan.SetBounds(10, 420, 900, 90);
            grpKetLuan.Controls.AddRange(new Control[] { lblKetLuan, txtKetLuan, btnGhiKetLuan });

            gridPhieuKham = new DataGridView(); gridPhieuKham.SetBounds(10, 520, 900, 220);
            gridPhieuKham.ReadOnly = true; gridPhieuKham.AllowUserToAddRows = false;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(930, 760);
            this.Controls.Add(grpLapPhieu);
            this.Controls.Add(grpXetNghiem);
            this.Controls.Add(grpKetLuan);
            this.Controls.Add(gridPhieuKham);
            this.Text = "Lap phieu kham benh";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
