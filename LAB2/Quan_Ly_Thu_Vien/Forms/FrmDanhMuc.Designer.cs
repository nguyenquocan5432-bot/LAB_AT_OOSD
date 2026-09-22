using System.Windows.Forms;
using System.Drawing;

namespace QuanLyThuVien.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabs;
        private TabPage tabNV, tabTL, tabNXB;

        // Tab Nhân viên
        private TextBox txtNVMa, txtNVHo, txtNVTen, txtNVChucVu, txtNVSDT;
        private ComboBox cboNVPhai;
        private DateTimePicker dtNVNgaySinh;
        private Button btnNVThem, btnNVCapNhat, btnNVXoa, btnNVMoi;
        private DataGridView dgvNV;

        // Tab Thể loại
        private TextBox txtTLMa, txtTLTen;
        private Button btnTLThem, btnTLCapNhat, btnTLXoa, btnTLMoi;
        private DataGridView dgvTL;

        // Tab NXB
        private TextBox txtNXBMa, txtNXBDiaChi, txtNXBSDT;
        private Button btnNXBThem, btnNXBCapNhat, btnNXBXoa, btnNXBMoi;
        private DataGridView dgvNXB;

        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabs = new TabControl();
            this.tabNV = new TabPage("Nhân viên");
            this.tabTL = new TabPage("Thể loại");
            this.tabNXB = new TabPage("Nhà xuất bản");
            this.btnDong = new Button();
            this.SuspendLayout();

            BuildTabNV();
            BuildTabTL();
            BuildTabNXB();

            this.tabs.SetBounds(15, 15, 1020, 630);
            this.tabs.TabPages.Add(this.tabNV);
            this.tabs.TabPages.Add(this.tabTL);
            this.tabs.TabPages.Add(this.tabNXB);

            this.btnDong.Text = "Đóng";
            this.btnDong.SetBounds(900, 655, 130, 35);
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1060, 720);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnDong);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmDanhMuc";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Danh mục và nhân viên";
            this.Load += new System.EventHandler(this.FrmDanhMuc_Load);
            this.ResumeLayout(false);
        }

        private void BuildTabNV()
        {
            Label l;
            l = new Label { Text = "Mã NV:" }; l.SetBounds(20, 20, 90, 23); tabNV.Controls.Add(l);
            txtNVMa = new TextBox(); txtNVMa.SetBounds(120, 18, 150, 23); tabNV.Controls.Add(txtNVMa);

            l = new Label { Text = "Họ:" }; l.SetBounds(20, 55, 90, 23); tabNV.Controls.Add(l);
            txtNVHo = new TextBox(); txtNVHo.SetBounds(120, 53, 150, 23); tabNV.Controls.Add(txtNVHo);

            l = new Label { Text = "Tên:" }; l.SetBounds(20, 90, 90, 23); tabNV.Controls.Add(l);
            txtNVTen = new TextBox(); txtNVTen.SetBounds(120, 88, 150, 23); tabNV.Controls.Add(txtNVTen);

            l = new Label { Text = "Phái:" }; l.SetBounds(20, 125, 90, 23); tabNV.Controls.Add(l);
            cboNVPhai = new ComboBox(); cboNVPhai.DropDownStyle = ComboBoxStyle.DropDownList; cboNVPhai.SetBounds(120, 123, 150, 23); tabNV.Controls.Add(cboNVPhai);

            l = new Label { Text = "Ngày sinh:" }; l.SetBounds(20, 160, 90, 23); tabNV.Controls.Add(l);
            dtNVNgaySinh = new DateTimePicker(); dtNVNgaySinh.Format = DateTimePickerFormat.Short; dtNVNgaySinh.SetBounds(120, 158, 150, 23); tabNV.Controls.Add(dtNVNgaySinh);

            l = new Label { Text = "Chức vụ:" }; l.SetBounds(20, 195, 90, 23); tabNV.Controls.Add(l);
            txtNVChucVu = new TextBox(); txtNVChucVu.SetBounds(120, 193, 150, 23); tabNV.Controls.Add(txtNVChucVu);

            l = new Label { Text = "Điện thoại:" }; l.SetBounds(20, 230, 90, 23); tabNV.Controls.Add(l);
            txtNVSDT = new TextBox(); txtNVSDT.SetBounds(120, 228, 150, 23); tabNV.Controls.Add(txtNVSDT);

            btnNVThem = new Button { Text = "Thêm" }; btnNVThem.SetBounds(20, 270, 90, 30); btnNVThem.Click += new System.EventHandler(this.btnNVThem_Click); tabNV.Controls.Add(btnNVThem);
            btnNVCapNhat = new Button { Text = "Cập nhật" }; btnNVCapNhat.SetBounds(120, 270, 90, 30); btnNVCapNhat.Click += new System.EventHandler(this.btnNVCapNhat_Click); tabNV.Controls.Add(btnNVCapNhat);
            btnNVXoa = new Button { Text = "Xóa" }; btnNVXoa.SetBounds(220, 270, 90, 30); btnNVXoa.Click += new System.EventHandler(this.btnNVXoa_Click); tabNV.Controls.Add(btnNVXoa);
            btnNVMoi = new Button { Text = "Làm mới" }; btnNVMoi.SetBounds(20, 310, 90, 30); btnNVMoi.Click += new System.EventHandler(this.btnNVMoi_Click); tabNV.Controls.Add(btnNVMoi);

            dgvNV = new DataGridView(); dgvNV.SetBounds(320, 20, 660, 550);
            dgvNV.ReadOnly = true; dgvNV.MultiSelect = false; dgvNV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNV.AllowUserToAddRows = false;
            dgvNV.SelectionChanged += new System.EventHandler(this.dgvNV_SelectionChanged);
            tabNV.Controls.Add(dgvNV);
        }

        private void BuildTabTL()
        {
            Label l;
            l = new Label { Text = "Mã thể loại:" }; l.SetBounds(20, 20, 100, 23); tabTL.Controls.Add(l);
            txtTLMa = new TextBox(); txtTLMa.SetBounds(130, 18, 150, 23); tabTL.Controls.Add(txtTLMa);

            l = new Label { Text = "Tên thể loại:" }; l.SetBounds(20, 55, 100, 23); tabTL.Controls.Add(l);
            txtTLTen = new TextBox(); txtTLTen.SetBounds(130, 53, 200, 23); tabTL.Controls.Add(txtTLTen);

            btnTLThem = new Button { Text = "Thêm" }; btnTLThem.SetBounds(20, 95, 90, 30); btnTLThem.Click += new System.EventHandler(this.btnTLThem_Click); tabTL.Controls.Add(btnTLThem);
            btnTLCapNhat = new Button { Text = "Cập nhật" }; btnTLCapNhat.SetBounds(120, 95, 90, 30); btnTLCapNhat.Click += new System.EventHandler(this.btnTLCapNhat_Click); tabTL.Controls.Add(btnTLCapNhat);
            btnTLXoa = new Button { Text = "Xóa" }; btnTLXoa.SetBounds(220, 95, 90, 30); btnTLXoa.Click += new System.EventHandler(this.btnTLXoa_Click); tabTL.Controls.Add(btnTLXoa);
            btnTLMoi = new Button { Text = "Làm mới" }; btnTLMoi.SetBounds(20, 135, 90, 30); btnTLMoi.Click += new System.EventHandler(this.btnTLMoi_Click); tabTL.Controls.Add(btnTLMoi);

            dgvTL = new DataGridView(); dgvTL.SetBounds(350, 20, 620, 550);
            dgvTL.ReadOnly = true; dgvTL.MultiSelect = false; dgvTL.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTL.AllowUserToAddRows = false;
            dgvTL.SelectionChanged += new System.EventHandler(this.dgvTL_SelectionChanged);
            tabTL.Controls.Add(dgvTL);
        }

        private void BuildTabNXB()
        {
            Label l;
            l = new Label { Text = "Mã NXB:" }; l.SetBounds(20, 20, 100, 23); tabNXB.Controls.Add(l);
            txtNXBMa = new TextBox(); txtNXBMa.SetBounds(130, 18, 150, 23); tabNXB.Controls.Add(txtNXBMa);

            l = new Label { Text = "Địa chỉ:" }; l.SetBounds(20, 55, 100, 23); tabNXB.Controls.Add(l);
            txtNXBDiaChi = new TextBox(); txtNXBDiaChi.SetBounds(130, 53, 250, 23); tabNXB.Controls.Add(txtNXBDiaChi);

            l = new Label { Text = "Điện thoại:" }; l.SetBounds(20, 90, 100, 23); tabNXB.Controls.Add(l);
            txtNXBSDT = new TextBox(); txtNXBSDT.SetBounds(130, 88, 150, 23); tabNXB.Controls.Add(txtNXBSDT);

            btnNXBThem = new Button { Text = "Thêm" }; btnNXBThem.SetBounds(20, 130, 90, 30); btnNXBThem.Click += new System.EventHandler(this.btnNXBThem_Click); tabNXB.Controls.Add(btnNXBThem);
            btnNXBCapNhat = new Button { Text = "Cập nhật" }; btnNXBCapNhat.SetBounds(120, 130, 90, 30); btnNXBCapNhat.Click += new System.EventHandler(this.btnNXBCapNhat_Click); tabNXB.Controls.Add(btnNXBCapNhat);
            btnNXBXoa = new Button { Text = "Xóa" }; btnNXBXoa.SetBounds(220, 130, 90, 30); btnNXBXoa.Click += new System.EventHandler(this.btnNXBXoa_Click); tabNXB.Controls.Add(btnNXBXoa);
            btnNXBMoi = new Button { Text = "Làm mới" }; btnNXBMoi.SetBounds(20, 170, 90, 30); btnNXBMoi.Click += new System.EventHandler(this.btnNXBMoi_Click); tabNXB.Controls.Add(btnNXBMoi);

            dgvNXB = new DataGridView(); dgvNXB.SetBounds(400, 20, 570, 550);
            dgvNXB.ReadOnly = true; dgvNXB.MultiSelect = false; dgvNXB.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNXB.AllowUserToAddRows = false;
            dgvNXB.SelectionChanged += new System.EventHandler(this.dgvNXB_SelectionChanged);
            tabNXB.Controls.Add(dgvNXB);
        }
    }
}
