using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.Forms
{
    partial class FrmDatPhong
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabs;
        private TabPage tabKhach, tabDat, tabNhan;

        // Tab Khách hàng
        private TextBox txtMaKH, txtTenKH, txtCMND, txtQT, txtSDT;
        private DataGridView dgvKhach;
        private Button btnThemKhach;

        // Tab Đặt phòng
        private TextBox txtSoPhieu;
        private ComboBox cboKhach, cboNV, cboKenh;
        private DateTimePicker dtLap, dtNhan, dtTra;
        private NumericUpDown numCoc, numSoNguoi;
        private DataGridView dgvPhong, dgvChon, dgvPhieu;
        private Button btnThemPhong, btnBoPhong, btnLapPhieu;

        // Tab Nhận phòng / Người lưu trú
        private TextBox txtPhieuChon, txtNguoiPhong, txtNguoiTen, txtNguoiCMND, txtNguoiQT;
        private DataGridView dgvCT, dgvNguoi;
        private Button btnThemNguoi, btnNhanPhong, btnNoShow;

        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabs = new TabControl();
            this.tabKhach = new TabPage("Khách hàng");
            this.tabDat = new TabPage("Đặt phòng");
            this.tabNhan = new TabPage("Nhận phòng / Người lưu trú");
            this.btnDong = new Button();
            this.SuspendLayout();

            BuildTabKhach(); BuildTabDat(); BuildTabNhan();

            this.tabs.SetBounds(15, 15, 1180, 680);
            this.tabs.TabPages.Add(this.tabKhach);
            this.tabs.TabPages.Add(this.tabDat);
            this.tabs.TabPages.Add(this.tabNhan);

            this.btnDong.Text = "Đóng";
            this.btnDong.SetBounds(1060, 705, 120, 32);
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1210, 760);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnDong);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmDatPhong";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Khách hàng - Đặt phòng - Nhận phòng";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.ResumeLayout(false);
        }

        private void BuildTabKhach()
        {
            Label l;
            l = new Label { Text = "Mã khách:" }; l.SetBounds(20, 20, 90, 23); tabKhach.Controls.Add(l);
            txtMaKH = new TextBox(); txtMaKH.SetBounds(120, 18, 150, 23); tabKhach.Controls.Add(txtMaKH);
            l = new Label { Text = "Họ tên:" }; l.SetBounds(20, 55, 90, 23); tabKhach.Controls.Add(l);
            txtTenKH = new TextBox(); txtTenKH.SetBounds(120, 53, 200, 23); tabKhach.Controls.Add(txtTenKH);
            l = new Label { Text = "CCCD/CMND:" }; l.SetBounds(20, 90, 90, 23); tabKhach.Controls.Add(l);
            txtCMND = new TextBox(); txtCMND.SetBounds(120, 88, 150, 23); tabKhach.Controls.Add(txtCMND);
            l = new Label { Text = "Quốc tịch:" }; l.SetBounds(20, 125, 90, 23); tabKhach.Controls.Add(l);
            txtQT = new TextBox(); txtQT.SetBounds(120, 123, 150, 23); tabKhach.Controls.Add(txtQT);
            l = new Label { Text = "Điện thoại:" }; l.SetBounds(20, 160, 90, 23); tabKhach.Controls.Add(l);
            txtSDT = new TextBox(); txtSDT.SetBounds(120, 158, 150, 23); tabKhach.Controls.Add(txtSDT);
            btnThemKhach = new Button { Text = "Thêm khách" }; btnThemKhach.SetBounds(20, 200, 120, 32); btnThemKhach.Click += new System.EventHandler(this.btnThemKhach_Click); tabKhach.Controls.Add(btnThemKhach);
            dgvKhach = new DataGridView(); dgvKhach.SetBounds(350, 20, 780, 590); dgvKhach.ReadOnly = true; dgvKhach.AllowUserToAddRows = false; tabKhach.Controls.Add(dgvKhach);
        }

        private void BuildTabDat()
        {
            Label l;
            l = new Label { Text = "Số phiếu đặt:" }; l.SetBounds(20, 15, 90, 23); tabDat.Controls.Add(l);
            txtSoPhieu = new TextBox(); txtSoPhieu.SetBounds(115, 13, 140, 23); tabDat.Controls.Add(txtSoPhieu);
            l = new Label { Text = "Khách:" }; l.SetBounds(265, 15, 55, 23); tabDat.Controls.Add(l);
            cboKhach = new ComboBox(); cboKhach.DropDownStyle = ComboBoxStyle.DropDownList; cboKhach.SetBounds(325, 13, 180, 23); tabDat.Controls.Add(cboKhach);
            l = new Label { Text = "Lễ tân:" }; l.SetBounds(515, 15, 55, 23); tabDat.Controls.Add(l);
            cboNV = new ComboBox(); cboNV.DropDownStyle = ComboBoxStyle.DropDownList; cboNV.SetBounds(575, 13, 160, 23); tabDat.Controls.Add(cboNV);
            l = new Label { Text = "Kênh:" }; l.SetBounds(745, 15, 50, 23); tabDat.Controls.Add(l);
            cboKenh = new ComboBox(); cboKenh.DropDownStyle = ComboBoxStyle.DropDownList; cboKenh.SetBounds(800, 13, 160, 23); tabDat.Controls.Add(cboKenh);

            l = new Label { Text = "Ngày lập:" }; l.SetBounds(20, 45, 80, 23); tabDat.Controls.Add(l);
            dtLap = new DateTimePicker(); dtLap.Format = DateTimePickerFormat.Short; dtLap.SetBounds(105, 43, 140, 23); tabDat.Controls.Add(dtLap);
            l = new Label { Text = "Ngày nhận:" }; l.SetBounds(255, 45, 80, 23); tabDat.Controls.Add(l);
            dtNhan = new DateTimePicker(); dtNhan.Format = DateTimePickerFormat.Short; dtNhan.SetBounds(340, 43, 140, 23); tabDat.Controls.Add(dtNhan);
            l = new Label { Text = "Ngày trả DK:" }; l.SetBounds(490, 45, 90, 23); tabDat.Controls.Add(l);
            dtTra = new DateTimePicker(); dtTra.Format = DateTimePickerFormat.Short; dtTra.SetBounds(585, 43, 140, 23); tabDat.Controls.Add(dtTra);
            l = new Label { Text = "Tiền cọc:" }; l.SetBounds(735, 45, 70, 23); tabDat.Controls.Add(l);
            numCoc = new NumericUpDown(); numCoc.Maximum = 1000000000; numCoc.SetBounds(810, 43, 150, 23); tabDat.Controls.Add(numCoc);

            l = new Label { Text = "Danh sách phòng" }; l.SetBounds(20, 80, 200, 20); tabDat.Controls.Add(l);
            dgvPhong = new DataGridView(); dgvPhong.SetBounds(20, 105, 470, 240);
            dgvPhong.ReadOnly = true; dgvPhong.AllowUserToAddRows = false; dgvPhong.MultiSelect = false; dgvPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabDat.Controls.Add(dgvPhong);

            l = new Label { Text = "Số người:" }; l.SetBounds(20, 350, 70, 23); tabDat.Controls.Add(l);
            numSoNguoi = new NumericUpDown(); numSoNguoi.Minimum = 1; numSoNguoi.Maximum = 20; numSoNguoi.Value = 1; numSoNguoi.SetBounds(100, 348, 80, 23); tabDat.Controls.Add(numSoNguoi);
            btnThemPhong = new Button { Text = "Thêm >>" }; btnThemPhong.SetBounds(190, 346, 100, 28); btnThemPhong.Click += new System.EventHandler(this.btnThemPhong_Click); tabDat.Controls.Add(btnThemPhong);
            btnBoPhong = new Button { Text = "<< Bỏ" }; btnBoPhong.SetBounds(300, 346, 100, 28); btnBoPhong.Click += new System.EventHandler(this.btnBoPhong_Click); tabDat.Controls.Add(btnBoPhong);

            l = new Label { Text = "Phòng chọn" }; l.SetBounds(510, 80, 200, 20); tabDat.Controls.Add(l);
            dgvChon = new DataGridView(); dgvChon.SetBounds(510, 105, 460, 240); dgvChon.ReadOnly = true; dgvChon.AllowUserToAddRows = false; tabDat.Controls.Add(dgvChon);

            btnLapPhieu = new Button { Text = "Lập phiếu đặt" }; btnLapPhieu.SetBounds(510, 350, 140, 32); btnLapPhieu.Click += new System.EventHandler(this.btnLapPhieu_Click); tabDat.Controls.Add(btnLapPhieu);

            l = new Label { Text = "Phiếu đặt phòng:" }; l.SetBounds(20, 395, 200, 20); tabDat.Controls.Add(l);
            dgvPhieu = new DataGridView(); dgvPhieu.SetBounds(20, 420, 950, 200);
            dgvPhieu.ReadOnly = true; dgvPhieu.AllowUserToAddRows = false; dgvPhieu.MultiSelect = false; dgvPhieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieu.SelectionChanged += new System.EventHandler(this.dgvPhieu_SelectionChanged);
            tabDat.Controls.Add(dgvPhieu);
        }

        private void BuildTabNhan()
        {
            Label l;
            l = new Label { Text = "Phiếu đã chọn:" }; l.SetBounds(20, 15, 100, 23); tabNhan.Controls.Add(l);
            txtPhieuChon = new TextBox(); txtPhieuChon.ReadOnly = true; txtPhieuChon.SetBounds(130, 13, 150, 23); tabNhan.Controls.Add(txtPhieuChon);

            btnNhanPhong = new Button { Text = "Nhận phòng" }; btnNhanPhong.SetBounds(300, 11, 120, 28); btnNhanPhong.Click += new System.EventHandler(this.btnNhanPhong_Click); tabNhan.Controls.Add(btnNhanPhong);
            btnNoShow = new Button { Text = "Đánh dấu No-show" }; btnNoShow.SetBounds(430, 11, 150, 28); btnNoShow.Click += new System.EventHandler(this.btnNoShow_Click); tabNhan.Controls.Add(btnNoShow);

            l = new Label { Text = "Chi tiết phòng đã đặt" }; l.SetBounds(20, 50, 200, 20); tabNhan.Controls.Add(l);
            dgvCT = new DataGridView(); dgvCT.SetBounds(20, 75, 950, 150); dgvCT.ReadOnly = true; dgvCT.AllowUserToAddRows = false; tabNhan.Controls.Add(dgvCT);

            l = new Label { Text = "Phòng:" }; l.SetBounds(20, 240, 60, 23); tabNhan.Controls.Add(l);
            txtNguoiPhong = new TextBox(); txtNguoiPhong.SetBounds(90, 238, 100, 23); tabNhan.Controls.Add(txtNguoiPhong);
            l = new Label { Text = "Họ tên:" }; l.SetBounds(210, 240, 60, 23); tabNhan.Controls.Add(l);
            txtNguoiTen = new TextBox(); txtNguoiTen.SetBounds(280, 238, 180, 23); tabNhan.Controls.Add(txtNguoiTen);
            l = new Label { Text = "CCCD/CMND:" }; l.SetBounds(480, 240, 90, 23); tabNhan.Controls.Add(l);
            txtNguoiCMND = new TextBox(); txtNguoiCMND.SetBounds(575, 238, 150, 23); tabNhan.Controls.Add(txtNguoiCMND);
            l = new Label { Text = "Quốc tịch:" }; l.SetBounds(745, 240, 70, 23); tabNhan.Controls.Add(l);
            txtNguoiQT = new TextBox(); txtNguoiQT.SetBounds(815, 238, 120, 23); tabNhan.Controls.Add(txtNguoiQT);
            btnThemNguoi = new Button { Text = "Thêm người lưu trú" }; btnThemNguoi.SetBounds(20, 275, 160, 30); btnThemNguoi.Click += new System.EventHandler(this.btnThemNguoi_Click); tabNhan.Controls.Add(btnThemNguoi);

            l = new Label { Text = "Danh sách người lưu trú" }; l.SetBounds(20, 320, 250, 20); tabNhan.Controls.Add(l);
            dgvNguoi = new DataGridView(); dgvNguoi.SetBounds(20, 345, 950, 230); dgvNguoi.ReadOnly = true; dgvNguoi.AllowUserToAddRows = false; tabNhan.Controls.Add(dgvNguoi);
        }
    }
}
