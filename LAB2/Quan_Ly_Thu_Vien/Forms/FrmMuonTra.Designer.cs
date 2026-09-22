using System.Windows.Forms;
using System.Drawing;

namespace QuanLyThuVien.Forms
{
    partial class FrmMuonTra
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabs;
        private TabPage tabMuon, tabTra;

        private ComboBox cboDocGia, cboNhanVienMuon;
        private Button btnKiemTra, btnThemSach, btnBoSach, btnLapPhieu;
        private Label lblTrangThai;
        private DateTimePicker dtNgayMuon, dtHenTra;
        private DataGridView dgvSachCon, dgvSachChon;

        private ComboBox cboDocGiaTra, cboNhanVienTra, cboTinhTrang;
        private Button btnTaiSachMuon, btnTraSach, btnDong;
        private DataGridView dgvDangMuon;
        private DateTimePicker dtNgayTra;
        private NumericUpDown numPhiPhat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabs = new TabControl();
            this.tabMuon = new TabPage("Mượn sách");
            this.tabTra = new TabPage("Trả sách");
            this.SuspendLayout();

            BuildTabMuon();
            BuildTabTra();

            this.tabs.SetBounds(15, 15, 1180, 685);
            this.tabs.TabPages.Add(this.tabMuon);
            this.tabs.TabPages.Add(this.tabTra);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1210, 765);
            this.Controls.Add(this.tabs);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmMuonTra";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Mượn - Trả sách";
            this.Load += new System.EventHandler(this.FrmMuonTra_Load);
            this.ResumeLayout(false);
        }

        private void BuildTabMuon()
        {
            Label l;
            l = new Label { Text = "Độc giả:" }; l.SetBounds(20, 20, 80, 23); tabMuon.Controls.Add(l);
            cboDocGia = new ComboBox(); cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList; cboDocGia.SetBounds(110, 18, 220, 23); tabMuon.Controls.Add(cboDocGia);

            btnKiemTra = new Button { Text = "Kiểm tra điều kiện" }; btnKiemTra.SetBounds(340, 16, 150, 28); btnKiemTra.Click += new System.EventHandler(this.btnKiemTra_Click); tabMuon.Controls.Add(btnKiemTra);

            lblTrangThai = new Label(); lblTrangThai.SetBounds(20, 50, 700, 23); tabMuon.Controls.Add(lblTrangThai);

            l = new Label { Text = "Nhân viên lập phiếu:" }; l.SetBounds(20, 85, 130, 23); tabMuon.Controls.Add(l);
            cboNhanVienMuon = new ComboBox(); cboNhanVienMuon.DropDownStyle = ComboBoxStyle.DropDownList; cboNhanVienMuon.SetBounds(160, 83, 150, 23); tabMuon.Controls.Add(cboNhanVienMuon);

            l = new Label { Text = "Ngày mượn:" }; l.SetBounds(330, 85, 90, 23); tabMuon.Controls.Add(l);
            dtNgayMuon = new DateTimePicker(); dtNgayMuon.Format = DateTimePickerFormat.Short; dtNgayMuon.SetBounds(420, 83, 130, 23); tabMuon.Controls.Add(dtNgayMuon);

            l = new Label { Text = "Hẹn trả:" }; l.SetBounds(560, 85, 60, 23); tabMuon.Controls.Add(l);
            dtHenTra = new DateTimePicker(); dtHenTra.Format = DateTimePickerFormat.Short; dtHenTra.SetBounds(620, 83, 130, 23); tabMuon.Controls.Add(dtHenTra);

            l = new Label { Text = "Sách còn kho" }; l.SetBounds(20, 120, 200, 20); tabMuon.Controls.Add(l);
            dgvSachCon = new DataGridView(); dgvSachCon.SetBounds(20, 145, 480, 430);
            dgvSachCon.ReadOnly = true; dgvSachCon.MultiSelect = false; dgvSachCon.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvSachCon.AllowUserToAddRows = false;
            tabMuon.Controls.Add(dgvSachCon);

            btnThemSach = new Button { Text = "Thêm >>" }; btnThemSach.SetBounds(520, 250, 90, 30); btnThemSach.Click += new System.EventHandler(this.btnThemSach_Click); tabMuon.Controls.Add(btnThemSach);
            btnBoSach = new Button { Text = "<< Bỏ" }; btnBoSach.SetBounds(520, 290, 90, 30); btnBoSach.Click += new System.EventHandler(this.btnBoSach_Click); tabMuon.Controls.Add(btnBoSach);

            l = new Label { Text = "Sách chọn (tối đa 3)" }; l.SetBounds(630, 120, 200, 20); tabMuon.Controls.Add(l);
            dgvSachChon = new DataGridView(); dgvSachChon.SetBounds(630, 145, 500, 200);
            dgvSachChon.ReadOnly = true; dgvSachChon.MultiSelect = false; dgvSachChon.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvSachChon.AllowUserToAddRows = false;
            tabMuon.Controls.Add(dgvSachChon);

            btnLapPhieu = new Button { Text = "Lập phiếu mượn" }; btnLapPhieu.SetBounds(630, 360, 150, 35); btnLapPhieu.Click += new System.EventHandler(this.btnLapPhieu_Click); tabMuon.Controls.Add(btnLapPhieu);
        }

        private void BuildTabTra()
        {
            Label l;
            l = new Label { Text = "Độc giả:" }; l.SetBounds(20, 20, 80, 23); tabTra.Controls.Add(l);
            cboDocGiaTra = new ComboBox(); cboDocGiaTra.DropDownStyle = ComboBoxStyle.DropDownList; cboDocGiaTra.SetBounds(110, 18, 220, 23);
            cboDocGiaTra.SelectedIndexChanged += new System.EventHandler(this.cboDocGiaTra_SelectedIndexChanged);
            tabTra.Controls.Add(cboDocGiaTra);

            btnTaiSachMuon = new Button { Text = "Tải sách đang mượn" }; btnTaiSachMuon.SetBounds(340, 16, 160, 28); btnTaiSachMuon.Click += new System.EventHandler(this.btnTaiSachMuon_Click); tabTra.Controls.Add(btnTaiSachMuon);

            l = new Label { Text = "Nhân viên nhận trả:" }; l.SetBounds(20, 55, 130, 23); tabTra.Controls.Add(l);
            cboNhanVienTra = new ComboBox(); cboNhanVienTra.DropDownStyle = ComboBoxStyle.DropDownList; cboNhanVienTra.SetBounds(160, 53, 150, 23); tabTra.Controls.Add(cboNhanVienTra);

            dgvDangMuon = new DataGridView(); dgvDangMuon.SetBounds(20, 90, 1140, 350);
            dgvDangMuon.ReadOnly = true; dgvDangMuon.MultiSelect = false; dgvDangMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvDangMuon.AllowUserToAddRows = false;
            tabTra.Controls.Add(dgvDangMuon);

            l = new Label { Text = "Ngày trả:" }; l.SetBounds(20, 455, 70, 23); tabTra.Controls.Add(l);
            dtNgayTra = new DateTimePicker(); dtNgayTra.Format = DateTimePickerFormat.Short; dtNgayTra.SetBounds(100, 453, 130, 23); tabTra.Controls.Add(dtNgayTra);

            l = new Label { Text = "Tình trạng:" }; l.SetBounds(260, 455, 80, 23); tabTra.Controls.Add(l);
            cboTinhTrang = new ComboBox(); cboTinhTrang.DropDownStyle = ComboBoxStyle.DropDownList; cboTinhTrang.SetBounds(350, 453, 150, 23); tabTra.Controls.Add(cboTinhTrang);

            l = new Label { Text = "Phí phạt:" }; l.SetBounds(530, 455, 70, 23); tabTra.Controls.Add(l);
            numPhiPhat = new NumericUpDown(); numPhiPhat.Maximum = 1000000000; numPhiPhat.SetBounds(610, 453, 130, 23); tabTra.Controls.Add(numPhiPhat);

            btnTraSach = new Button { Text = "Xác nhận trả sách" }; btnTraSach.SetBounds(20, 495, 160, 35); btnTraSach.Click += new System.EventHandler(this.btnTraSach_Click); tabTra.Controls.Add(btnTraSach);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(1050, 495, 90, 35); btnDong.Click += new System.EventHandler(this.btnDong_Click); tabTra.Controls.Add(btnDong);
        }
    }
}
