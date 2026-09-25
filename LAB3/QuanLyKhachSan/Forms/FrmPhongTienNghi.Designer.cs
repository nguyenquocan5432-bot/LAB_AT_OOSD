using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.Forms
{
    partial class FrmPhongTienNghi
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabs;
        private TabPage tabPhong, tabTN, tabLD;

        private TextBox txtPhong;
        private ComboBox cboKhu;
        private NumericUpDown numMax;
        private NumericUpDown numGia;
        private DataGridView dgvPhong;
        private Button btnThemPhong;

        private TextBox txtMaTN, txtTinhTrang;
        private ComboBox cboLoai;
        private NumericUpDown numSTT;
        private DataGridView dgvTN;
        private Button btnThemTN;

        private TextBox txtSoLD, txtTTLD, txtGhiChu;
        private ComboBox cboTN, cboPhong, cboNV;
        private DateTimePicker dtNgay;
        private DataGridView dgvLD;
        private Button btnLapDat;

        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabs = new TabControl();
            this.tabPhong = new TabPage("Phòng");
            this.tabTN = new TabPage("Tiện nghi");
            this.tabLD = new TabPage("Lắp đặt / luân chuyển");
            this.btnDong = new Button();
            this.SuspendLayout();

            BuildTabPhong(); BuildTabTN(); BuildTabLD();

            this.tabs.SetBounds(15, 15, 1000, 600);
            this.tabs.TabPages.Add(this.tabPhong);
            this.tabs.TabPages.Add(this.tabTN);
            this.tabs.TabPages.Add(this.tabLD);

            this.btnDong.Text = "Đóng";
            this.btnDong.SetBounds(890, 625, 120, 32);
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1030, 680);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnDong);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmPhongTienNghi";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Phòng - Tiện nghi - Phiếu lắp đặt";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.ResumeLayout(false);
        }

        private void BuildTabPhong()
        {
            Label l;
            l = new Label { Text = "Số phòng:" }; l.SetBounds(20, 20, 90, 23); tabPhong.Controls.Add(l);
            txtPhong = new TextBox(); txtPhong.SetBounds(120, 18, 150, 23); tabPhong.Controls.Add(txtPhong);
            l = new Label { Text = "Khu vực:" }; l.SetBounds(20, 55, 90, 23); tabPhong.Controls.Add(l);
            cboKhu = new ComboBox(); cboKhu.DropDownStyle = ComboBoxStyle.DropDownList; cboKhu.SetBounds(120, 53, 150, 23); tabPhong.Controls.Add(cboKhu);
            l = new Label { Text = "Số người tối đa:" }; l.SetBounds(20, 90, 100, 23); tabPhong.Controls.Add(l);
            numMax = new NumericUpDown(); numMax.Maximum = 100; numMax.SetBounds(120, 88, 100, 23); tabPhong.Controls.Add(numMax);
            l = new Label { Text = "Đơn giá/ngày:" }; l.SetBounds(20, 125, 100, 23); tabPhong.Controls.Add(l);
            numGia = new NumericUpDown(); numGia.Maximum = 1000000000; numGia.SetBounds(120, 123, 150, 23); tabPhong.Controls.Add(numGia);
            btnThemPhong = new Button { Text = "Thêm phòng" }; btnThemPhong.SetBounds(20, 165, 120, 30); btnThemPhong.Click += new System.EventHandler(this.btnThemPhong_Click); tabPhong.Controls.Add(btnThemPhong);
            dgvPhong = new DataGridView(); dgvPhong.SetBounds(350, 20, 600, 520); dgvPhong.ReadOnly = true; dgvPhong.AllowUserToAddRows = false; tabPhong.Controls.Add(dgvPhong);
        }

        private void BuildTabTN()
        {
            Label l;
            l = new Label { Text = "Mã tiện nghi:" }; l.SetBounds(20, 20, 100, 23); tabTN.Controls.Add(l);
            txtMaTN = new TextBox(); txtMaTN.SetBounds(130, 18, 150, 23); tabTN.Controls.Add(txtMaTN);
            l = new Label { Text = "Loại tiện nghi:" }; l.SetBounds(20, 55, 100, 23); tabTN.Controls.Add(l);
            cboLoai = new ComboBox(); cboLoai.DropDownStyle = ComboBoxStyle.DropDownList; cboLoai.SetBounds(130, 53, 200, 23); tabTN.Controls.Add(cboLoai);
            l = new Label { Text = "Số thứ tự:" }; l.SetBounds(20, 90, 100, 23); tabTN.Controls.Add(l);
            numSTT = new NumericUpDown(); numSTT.Maximum = 1000; numSTT.SetBounds(130, 88, 100, 23); tabTN.Controls.Add(numSTT);
            l = new Label { Text = "Tình trạng:" }; l.SetBounds(20, 125, 100, 23); tabTN.Controls.Add(l);
            txtTinhTrang = new TextBox(); txtTinhTrang.SetBounds(130, 123, 200, 23); tabTN.Controls.Add(txtTinhTrang);
            btnThemTN = new Button { Text = "Thêm tiện nghi" }; btnThemTN.SetBounds(20, 165, 130, 30); btnThemTN.Click += new System.EventHandler(this.btnThemTN_Click); tabTN.Controls.Add(btnThemTN);
            dgvTN = new DataGridView(); dgvTN.SetBounds(350, 20, 600, 520); dgvTN.ReadOnly = true; dgvTN.AllowUserToAddRows = false; tabTN.Controls.Add(dgvTN);
        }

        private void BuildTabLD()
        {
            Label l;
            l = new Label { Text = "Phiếu lắp đặt:" }; l.SetBounds(20, 20, 100, 23); tabLD.Controls.Add(l);
            txtSoLD = new TextBox(); txtSoLD.SetBounds(130, 18, 150, 23); tabLD.Controls.Add(txtSoLD);
            l = new Label { Text = "Tiện nghi:" }; l.SetBounds(20, 55, 100, 23); tabLD.Controls.Add(l);
            cboTN = new ComboBox(); cboTN.DropDownStyle = ComboBoxStyle.DropDownList; cboTN.SetBounds(130, 53, 150, 23); tabLD.Controls.Add(cboTN);
            l = new Label { Text = "Phòng:" }; l.SetBounds(300, 55, 60, 23); tabLD.Controls.Add(l);
            cboPhong = new ComboBox(); cboPhong.DropDownStyle = ComboBoxStyle.DropDownList; cboPhong.SetBounds(360, 53, 120, 23); tabLD.Controls.Add(cboPhong);
            l = new Label { Text = "Ngày:" }; l.SetBounds(20, 90, 100, 23); tabLD.Controls.Add(l);
            dtNgay = new DateTimePicker(); dtNgay.Format = DateTimePickerFormat.Short; dtNgay.SetBounds(130, 88, 150, 23); tabLD.Controls.Add(dtNgay);
            l = new Label { Text = "Tình trạng:" }; l.SetBounds(20, 125, 100, 23); tabLD.Controls.Add(l);
            txtTTLD = new TextBox(); txtTTLD.SetBounds(130, 123, 200, 23); tabLD.Controls.Add(txtTTLD);
            l = new Label { Text = "Nhân viên:" }; l.SetBounds(20, 160, 100, 23); tabLD.Controls.Add(l);
            cboNV = new ComboBox(); cboNV.DropDownStyle = ComboBoxStyle.DropDownList; cboNV.SetBounds(130, 158, 200, 23); tabLD.Controls.Add(cboNV);
            l = new Label { Text = "Ghi chú:" }; l.SetBounds(20, 195, 100, 23); tabLD.Controls.Add(l);
            txtGhiChu = new TextBox(); txtGhiChu.SetBounds(130, 193, 250, 23); tabLD.Controls.Add(txtGhiChu);
            btnLapDat = new Button { Text = "Lập phiếu" }; btnLapDat.SetBounds(20, 235, 110, 32); btnLapDat.Click += new System.EventHandler(this.btnLapDat_Click); tabLD.Controls.Add(btnLapDat);
            dgvLD = new DataGridView(); dgvLD.SetBounds(420, 20, 530, 520); dgvLD.ReadOnly = true; dgvLD.AllowUserToAddRows = false; tabLD.Controls.Add(dgvLD);
        }
    }
}
