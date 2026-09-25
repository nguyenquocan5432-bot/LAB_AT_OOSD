using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabs;
        private TabPage tabKhu, tabNV, tabLoaiTN, tabDV, tabQD;

        private TextBox txtKhuMa, txtKhuTen;
        private DataGridView dgvKhu;
        private Button btnThemKhu;

        private TextBox txtNVMa, txtNVTen, txtNVVaiTro, txtNVSDT;
        private DataGridView dgvNV;
        private Button btnThemNV;

        private TextBox txtLoaiMa, txtLoaiTen;
        private DataGridView dgvLoaiTN;
        private Button btnThemLoaiTN;

        private TextBox txtDVMa, txtDVTen, txtDVDVT;
        private NumericUpDown numDVGia;
        private DataGridView dgvDV;
        private Button btnThemDV;

        private TextBox txtQDMa, txtQDMucDo;
        private ComboBox cboQDLoai;
        private NumericUpDown numQDTien;
        private DataGridView dgvQD;
        private Button btnThemQD;

        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabs = new TabControl();
            this.tabKhu = new TabPage("Khu vực");
            this.tabNV = new TabPage("Nhân viên");
            this.tabLoaiTN = new TabPage("Loại tiện nghi");
            this.tabDV = new TabPage("Dịch vụ");
            this.tabQD = new TabPage("Quy định đền bù");
            this.btnDong = new Button();
            this.SuspendLayout();

            BuildTabKhu(); BuildTabNV(); BuildTabLoaiTN(); BuildTabDV(); BuildTabQD();

            this.tabs.SetBounds(15, 15, 1000, 600);
            this.tabs.TabPages.Add(this.tabKhu);
            this.tabs.TabPages.Add(this.tabNV);
            this.tabs.TabPages.Add(this.tabLoaiTN);
            this.tabs.TabPages.Add(this.tabDV);
            this.tabs.TabPages.Add(this.tabQD);

            this.btnDong.Text = "Đóng";
            this.btnDong.SetBounds(890, 625, 120, 32);
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1030, 680);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnDong);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmDanhMuc";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Danh mục khách sạn";
            this.Load += new System.EventHandler(this.FrmDanhMuc_Load);
            this.ResumeLayout(false);
        }

        private void BuildTabKhu()
        {
            Label l;
            l = new Label { Text = "Mã khu vực:" }; l.SetBounds(20, 20, 100, 23); tabKhu.Controls.Add(l);
            txtKhuMa = new TextBox(); txtKhuMa.SetBounds(130, 18, 150, 23); tabKhu.Controls.Add(txtKhuMa);
            l = new Label { Text = "Tên khu vực:" }; l.SetBounds(20, 55, 100, 23); tabKhu.Controls.Add(l);
            txtKhuTen = new TextBox(); txtKhuTen.SetBounds(130, 53, 200, 23); tabKhu.Controls.Add(txtKhuTen);
            btnThemKhu = new Button { Text = "Thêm" }; btnThemKhu.SetBounds(20, 90, 90, 30); btnThemKhu.Click += new System.EventHandler(this.btnThemKhu_Click); tabKhu.Controls.Add(btnThemKhu);
            dgvKhu = new DataGridView(); dgvKhu.SetBounds(350, 20, 600, 520); dgvKhu.ReadOnly = true; dgvKhu.AllowUserToAddRows = false; tabKhu.Controls.Add(dgvKhu);
        }

        private void BuildTabNV()
        {
            Label l;
            l = new Label { Text = "Mã NV:" }; l.SetBounds(20, 20, 100, 23); tabNV.Controls.Add(l);
            txtNVMa = new TextBox(); txtNVMa.SetBounds(130, 18, 150, 23); tabNV.Controls.Add(txtNVMa);
            l = new Label { Text = "Họ tên:" }; l.SetBounds(20, 55, 100, 23); tabNV.Controls.Add(l);
            txtNVTen = new TextBox(); txtNVTen.SetBounds(130, 53, 200, 23); tabNV.Controls.Add(txtNVTen);
            l = new Label { Text = "Vai trò:" }; l.SetBounds(20, 90, 100, 23); tabNV.Controls.Add(l);
            txtNVVaiTro = new TextBox(); txtNVVaiTro.SetBounds(130, 88, 200, 23); tabNV.Controls.Add(txtNVVaiTro);
            l = new Label { Text = "Điện thoại:" }; l.SetBounds(20, 125, 100, 23); tabNV.Controls.Add(l);
            txtNVSDT = new TextBox(); txtNVSDT.SetBounds(130, 123, 150, 23); tabNV.Controls.Add(txtNVSDT);
            btnThemNV = new Button { Text = "Thêm" }; btnThemNV.SetBounds(20, 160, 90, 30); btnThemNV.Click += new System.EventHandler(this.btnThemNV_Click); tabNV.Controls.Add(btnThemNV);
            dgvNV = new DataGridView(); dgvNV.SetBounds(350, 20, 600, 520); dgvNV.ReadOnly = true; dgvNV.AllowUserToAddRows = false; tabNV.Controls.Add(dgvNV);
        }

        private void BuildTabLoaiTN()
        {
            Label l;
            l = new Label { Text = "Mã loại TN:" }; l.SetBounds(20, 20, 100, 23); tabLoaiTN.Controls.Add(l);
            txtLoaiMa = new TextBox(); txtLoaiMa.SetBounds(130, 18, 150, 23); tabLoaiTN.Controls.Add(txtLoaiMa);
            l = new Label { Text = "Tên loại TN:" }; l.SetBounds(20, 55, 100, 23); tabLoaiTN.Controls.Add(l);
            txtLoaiTen = new TextBox(); txtLoaiTen.SetBounds(130, 53, 200, 23); tabLoaiTN.Controls.Add(txtLoaiTen);
            btnThemLoaiTN = new Button { Text = "Thêm" }; btnThemLoaiTN.SetBounds(20, 90, 90, 30); btnThemLoaiTN.Click += new System.EventHandler(this.btnThemLoaiTN_Click); tabLoaiTN.Controls.Add(btnThemLoaiTN);
            dgvLoaiTN = new DataGridView(); dgvLoaiTN.SetBounds(350, 20, 600, 520); dgvLoaiTN.ReadOnly = true; dgvLoaiTN.AllowUserToAddRows = false; tabLoaiTN.Controls.Add(dgvLoaiTN);
        }

        private void BuildTabDV()
        {
            Label l;
            l = new Label { Text = "Mã DV:" }; l.SetBounds(20, 20, 100, 23); tabDV.Controls.Add(l);
            txtDVMa = new TextBox(); txtDVMa.SetBounds(130, 18, 150, 23); tabDV.Controls.Add(txtDVMa);
            l = new Label { Text = "Tên DV:" }; l.SetBounds(20, 55, 100, 23); tabDV.Controls.Add(l);
            txtDVTen = new TextBox(); txtDVTen.SetBounds(130, 53, 200, 23); tabDV.Controls.Add(txtDVTen);
            l = new Label { Text = "Đơn vị tính:" }; l.SetBounds(20, 90, 100, 23); tabDV.Controls.Add(l);
            txtDVDVT = new TextBox(); txtDVDVT.SetBounds(130, 88, 150, 23); tabDV.Controls.Add(txtDVDVT);
            l = new Label { Text = "Đơn giá:" }; l.SetBounds(20, 125, 100, 23); tabDV.Controls.Add(l);
            numDVGia = new NumericUpDown(); numDVGia.Maximum = 1000000000; numDVGia.SetBounds(130, 123, 150, 23); tabDV.Controls.Add(numDVGia);
            btnThemDV = new Button { Text = "Thêm" }; btnThemDV.SetBounds(20, 160, 90, 30); btnThemDV.Click += new System.EventHandler(this.btnThemDV_Click); tabDV.Controls.Add(btnThemDV);
            dgvDV = new DataGridView(); dgvDV.SetBounds(350, 20, 600, 520); dgvDV.ReadOnly = true; dgvDV.AllowUserToAddRows = false; tabDV.Controls.Add(dgvDV);
        }

        private void BuildTabQD()
        {
            Label l;
            l = new Label { Text = "Mã quy định:" }; l.SetBounds(20, 20, 100, 23); tabQD.Controls.Add(l);
            txtQDMa = new TextBox(); txtQDMa.SetBounds(130, 18, 150, 23); tabQD.Controls.Add(txtQDMa);
            l = new Label { Text = "Loại tiện nghi:" }; l.SetBounds(20, 55, 100, 23); tabQD.Controls.Add(l);
            cboQDLoai = new ComboBox(); cboQDLoai.DropDownStyle = ComboBoxStyle.DropDownList; cboQDLoai.SetBounds(130, 53, 200, 23); tabQD.Controls.Add(cboQDLoai);
            l = new Label { Text = "Mức độ thiệt hại:" }; l.SetBounds(20, 90, 110, 23); tabQD.Controls.Add(l);
            txtQDMucDo = new TextBox(); txtQDMucDo.SetBounds(130, 88, 200, 23); tabQD.Controls.Add(txtQDMucDo);
            l = new Label { Text = "Mức đền bù:" }; l.SetBounds(20, 125, 100, 23); tabQD.Controls.Add(l);
            numQDTien = new NumericUpDown(); numQDTien.Maximum = 1000000000; numQDTien.SetBounds(130, 123, 150, 23); tabQD.Controls.Add(numQDTien);
            btnThemQD = new Button { Text = "Thêm" }; btnThemQD.SetBounds(20, 160, 90, 30); btnThemQD.Click += new System.EventHandler(this.btnThemQD_Click); tabQD.Controls.Add(btnThemQD);
            dgvQD = new DataGridView(); dgvQD.SetBounds(350, 20, 600, 520); dgvQD.ReadOnly = true; dgvQD.AllowUserToAddRows = false; tabQD.Controls.Add(dgvQD);
        }
    }
}
