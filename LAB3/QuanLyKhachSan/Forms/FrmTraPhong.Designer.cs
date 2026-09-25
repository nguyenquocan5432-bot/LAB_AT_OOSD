using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.Forms
{
    partial class FrmTraPhong
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboDat, cboNV, cboNV2, cboHT;
        private DataGridView dgvPhong, dgvTN, dgvDBChon, dgvHD;
        private TextBox txtPhong, txtSoDB, txtMucDo, txtSoHD, txtHDChon, txtMaTT;
        private NumericUpDown numDenBu, numSoNgay, numTienTT;
        private Button btnThemDB, btnLapDB, btnLapHD, btnThanhToan, btnTraPhong, btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            Label l;

            l = new Label { Text = "Phiếu đang ở:" }; l.SetBounds(20, 15, 90, 23); this.Controls.Add(l);
            cboDat = new ComboBox(); cboDat.DropDownStyle = ComboBoxStyle.DropDownList; cboDat.SetBounds(120, 13, 160, 23); this.Controls.Add(cboDat);

            l = new Label { Text = "Phòng" }; l.SetBounds(20, 50, 300, 20); this.Controls.Add(l);
            dgvPhong = new DataGridView(); dgvPhong.SetBounds(20, 75, 260, 220);
            dgvPhong.ReadOnly = true; dgvPhong.AllowUserToAddRows = false; dgvPhong.MultiSelect = false; dgvPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhong.SelectionChanged += new System.EventHandler(this.dgvPhong_SelectionChanged);
            this.Controls.Add(dgvPhong);
            txtPhong = new TextBox(); txtPhong.ReadOnly = true; txtPhong.Visible = false; this.Controls.Add(txtPhong);

            l = new Label { Text = "Tiện nghi" }; l.SetBounds(300, 50, 300, 20); this.Controls.Add(l);
            dgvTN = new DataGridView(); dgvTN.SetBounds(300, 75, 320, 220);
            dgvTN.ReadOnly = true; dgvTN.AllowUserToAddRows = false; dgvTN.MultiSelect = false; dgvTN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvTN);

            l = new Label { Text = "Tiện nghi đền bù" }; l.SetBounds(640, 50, 300, 20); this.Controls.Add(l);
            dgvDBChon = new DataGridView(); dgvDBChon.SetBounds(640, 75, 350, 220); dgvDBChon.ReadOnly = true; dgvDBChon.AllowUserToAddRows = false; this.Controls.Add(dgvDBChon);

            l = new Label { Text = "Số phiếu đền bù:" }; l.SetBounds(20, 305, 110, 23); this.Controls.Add(l);
            txtSoDB = new TextBox(); txtSoDB.SetBounds(140, 303, 130, 23); this.Controls.Add(txtSoDB);
            l = new Label { Text = "Mức độ:" }; l.SetBounds(290, 305, 60, 23); this.Controls.Add(l);
            txtMucDo = new TextBox(); txtMucDo.SetBounds(355, 303, 150, 23); this.Controls.Add(txtMucDo);
            l = new Label { Text = "Số tiền:" }; l.SetBounds(520, 305, 60, 23); this.Controls.Add(l);
            numDenBu = new NumericUpDown(); numDenBu.Maximum = 1000000000; numDenBu.SetBounds(585, 303, 130, 23); this.Controls.Add(numDenBu);
            btnThemDB = new Button { Text = "Thêm vào phiếu" }; btnThemDB.SetBounds(730, 301, 130, 28); btnThemDB.Click += new System.EventHandler(this.btnThemDB_Click); this.Controls.Add(btnThemDB);
            btnLapDB = new Button { Text = "Lập phiếu đền bù" }; btnLapDB.SetBounds(870, 301, 140, 28); btnLapDB.Click += new System.EventHandler(this.btnLapDB_Click); this.Controls.Add(btnLapDB);

            l = new Label { Text = "Số hóa đơn:" }; l.SetBounds(20, 345, 90, 23); this.Controls.Add(l);
            txtSoHD = new TextBox(); txtSoHD.SetBounds(120, 343, 130, 23); this.Controls.Add(txtSoHD);
            l = new Label { Text = "Số ngày tính tiền:" }; l.SetBounds(270, 345, 110, 23); this.Controls.Add(l);
            numSoNgay = new NumericUpDown(); numSoNgay.Minimum = 1; numSoNgay.Maximum = 365; numSoNgay.Value = 1; numSoNgay.SetBounds(385, 343, 80, 23); this.Controls.Add(numSoNgay);
            l = new Label { Text = "Nhân viên:" }; l.SetBounds(480, 345, 80, 23); this.Controls.Add(l);
            cboNV2 = new ComboBox(); cboNV2.DropDownStyle = ComboBoxStyle.DropDownList; cboNV2.SetBounds(560, 343, 160, 23); this.Controls.Add(cboNV2);
            btnLapHD = new Button { Text = "Lập hóa đơn" }; btnLapHD.SetBounds(740, 341, 120, 28); btnLapHD.Click += new System.EventHandler(this.btnLapHD_Click); this.Controls.Add(btnLapHD);

            l = new Label { Text = "Hóa đơn" }; l.SetBounds(20, 380, 300, 20); this.Controls.Add(l);
            dgvHD = new DataGridView(); dgvHD.SetBounds(20, 405, 970, 190);
            dgvHD.ReadOnly = true; dgvHD.AllowUserToAddRows = false; dgvHD.MultiSelect = false; dgvHD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHD.SelectionChanged += new System.EventHandler(this.dgvHD_SelectionChanged);
            this.Controls.Add(dgvHD);
            txtHDChon = new TextBox(); txtHDChon.ReadOnly = true; txtHDChon.Visible = false; this.Controls.Add(txtHDChon);

            l = new Label { Text = "Mã thanh toán:" }; l.SetBounds(20, 605, 100, 23); this.Controls.Add(l);
            txtMaTT = new TextBox(); txtMaTT.SetBounds(130, 603, 130, 23); this.Controls.Add(txtMaTT);
            l = new Label { Text = "Hình thức:" }; l.SetBounds(280, 605, 80, 23); this.Controls.Add(l);
            cboHT = new ComboBox(); cboHT.DropDownStyle = ComboBoxStyle.DropDownList; cboHT.SetBounds(360, 603, 130, 23); this.Controls.Add(cboHT);
            l = new Label { Text = "Số tiền:" }; l.SetBounds(510, 605, 60, 23); this.Controls.Add(l);
            numTienTT = new NumericUpDown(); numTienTT.Maximum = 1000000000; numTienTT.SetBounds(575, 603, 130, 23); this.Controls.Add(numTienTT);
            btnThanhToan = new Button { Text = "Thanh toán" }; btnThanhToan.SetBounds(730, 601, 120, 28); btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click); this.Controls.Add(btnThanhToan);
            btnTraPhong = new Button { Text = "Hoàn tất trả phòng" }; btnTraPhong.SetBounds(860, 601, 150, 28); btnTraPhong.Click += new System.EventHandler(this.btnTraPhong_Click); this.Controls.Add(btnTraPhong);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(890, 645, 120, 32); btnDong.Click += new System.EventHandler(this.btnDong_Click); this.Controls.Add(btnDong);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1030, 700);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmTraPhong";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Trả phòng - Đền bù - Hóa đơn - Thanh toán";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.ResumeLayout(false);
        }
    }
}
