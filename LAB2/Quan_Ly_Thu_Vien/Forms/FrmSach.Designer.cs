using System.Windows.Forms;
using System.Drawing;

namespace QuanLyThuVien.Forms
{
    partial class FrmSach
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtMa, txtTen, txtTim;
        private NumericUpDown numNam, numSoLuong;
        private ComboBox cboTheLoai, cboNXB;
        private Button btnTim, btnThem, btnCapNhat, btnXoa, btnLamMoi, btnDong;
        private DataGridView dgvSach;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            Label l;

            l = new Label { Text = "Mã đầu sách:" }; l.SetBounds(20, 20, 100, 23); this.Controls.Add(l);
            txtMa = new TextBox(); txtMa.SetBounds(130, 18, 160, 23); this.Controls.Add(txtMa);

            l = new Label { Text = "Tên sách:" }; l.SetBounds(20, 55, 100, 23); this.Controls.Add(l);
            txtTen = new TextBox(); txtTen.SetBounds(130, 53, 220, 23); this.Controls.Add(txtTen);

            l = new Label { Text = "Năm XB:" }; l.SetBounds(20, 90, 100, 23); this.Controls.Add(l);
            numNam = new NumericUpDown(); numNam.Minimum = 1000; numNam.Maximum = 3000; numNam.SetBounds(130, 88, 100, 23); this.Controls.Add(numNam);

            l = new Label { Text = "Số lượng:" }; l.SetBounds(20, 125, 100, 23); this.Controls.Add(l);
            numSoLuong = new NumericUpDown(); numSoLuong.Maximum = 100000; numSoLuong.SetBounds(130, 123, 100, 23); this.Controls.Add(numSoLuong);

            l = new Label { Text = "Thể loại:" }; l.SetBounds(20, 160, 100, 23); this.Controls.Add(l);
            cboTheLoai = new ComboBox(); cboTheLoai.DropDownStyle = ComboBoxStyle.DropDownList; cboTheLoai.SetBounds(130, 158, 220, 23); this.Controls.Add(cboTheLoai);

            l = new Label { Text = "Mã NXB:" }; l.SetBounds(20, 195, 100, 23); this.Controls.Add(l);
            cboNXB = new ComboBox(); cboNXB.DropDownStyle = ComboBoxStyle.DropDownList; cboNXB.SetBounds(130, 193, 220, 23); this.Controls.Add(cboNXB);

            btnThem = new Button { Text = "Thêm" }; btnThem.SetBounds(20, 235, 90, 30); btnThem.Click += new System.EventHandler(this.btnThem_Click); this.Controls.Add(btnThem);
            btnCapNhat = new Button { Text = "Cập nhật" }; btnCapNhat.SetBounds(120, 235, 90, 30); btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click); this.Controls.Add(btnCapNhat);
            btnXoa = new Button { Text = "Xóa" }; btnXoa.SetBounds(220, 235, 90, 30); btnXoa.Click += new System.EventHandler(this.btnXoa_Click); this.Controls.Add(btnXoa);
            btnLamMoi = new Button { Text = "Làm mới" }; btnLamMoi.SetBounds(20, 275, 90, 30); btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click); this.Controls.Add(btnLamMoi);

            l = new Label { Text = "Tìm:" }; l.SetBounds(20, 320, 40, 23); this.Controls.Add(l);
            txtTim = new TextBox(); txtTim.SetBounds(60, 318, 160, 23); this.Controls.Add(txtTim);
            btnTim = new Button { Text = "Tìm" }; btnTim.SetBounds(230, 316, 80, 28); btnTim.Click += new System.EventHandler(this.btnTim_Click); this.Controls.Add(btnTim);

            dgvSach = new DataGridView(); dgvSach.SetBounds(380, 20, 700, 620);
            dgvSach.ReadOnly = true; dgvSach.MultiSelect = false; dgvSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSach.AllowUserToAddRows = false;
            dgvSach.SelectionChanged += new System.EventHandler(this.dgvSach_SelectionChanged);
            this.Controls.Add(dgvSach);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(990, 655, 90, 30); btnDong.Click += new System.EventHandler(this.btnDong_Click); this.Controls.Add(btnDong);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1120, 740);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmSach";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý đầu sách";
            this.Load += new System.EventHandler(this.FrmSach_Load);
            this.ResumeLayout(false);
        }
    }
}
