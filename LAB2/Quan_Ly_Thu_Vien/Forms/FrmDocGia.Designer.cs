using System.Windows.Forms;
using System.Drawing;

namespace QuanLyThuVien.Forms
{
    partial class FrmDocGia
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtMa, txtHo, txtTen, txtSDT, txtDiaChi, txtEmail, txtAnh;
        private DateTimePicker dtNgaySinh, dtNgayCap, dtHan;
        private ComboBox cboPhai;
        private CheckBox chkLePhi;
        private Button btnThem, btnCapNhat, btnCapThe, btnGiaHan, btnLamMoi, btnDong;
        private DataGridView dgvDocGia;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            Label l;
            int y = 20, gap = 35;

            l = new Label { Text = "Mã độc giả:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtMa = new TextBox(); txtMa.SetBounds(140, y, 180, 23); this.Controls.Add(txtMa); y += gap;

            l = new Label { Text = "Họ:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtHo = new TextBox(); txtHo.SetBounds(140, y, 180, 23); this.Controls.Add(txtHo); y += gap;

            l = new Label { Text = "Tên:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtTen = new TextBox(); txtTen.SetBounds(140, y, 180, 23); this.Controls.Add(txtTen); y += gap;

            l = new Label { Text = "Ngày sinh:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            dtNgaySinh = new DateTimePicker(); dtNgaySinh.Format = DateTimePickerFormat.Short; dtNgaySinh.SetBounds(140, y, 180, 23); this.Controls.Add(dtNgaySinh); y += gap;

            l = new Label { Text = "Phái:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            cboPhai = new ComboBox(); cboPhai.DropDownStyle = ComboBoxStyle.DropDownList; cboPhai.SetBounds(140, y, 180, 23); this.Controls.Add(cboPhai); y += gap;

            l = new Label { Text = "Điện thoại:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtSDT = new TextBox(); txtSDT.SetBounds(140, y, 180, 23); this.Controls.Add(txtSDT); y += gap;

            l = new Label { Text = "Địa chỉ:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtDiaChi = new TextBox(); txtDiaChi.SetBounds(140, y, 180, 23); this.Controls.Add(txtDiaChi); y += gap;

            l = new Label { Text = "Email:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtEmail = new TextBox(); txtEmail.SetBounds(140, y, 180, 23); this.Controls.Add(txtEmail); y += gap;

            l = new Label { Text = "Ảnh 3x4:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            txtAnh = new TextBox(); txtAnh.SetBounds(140, y, 180, 23); this.Controls.Add(txtAnh); y += gap;

            l = new Label { Text = "Ngày cấp thẻ:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            dtNgayCap = new DateTimePicker(); dtNgayCap.Format = DateTimePickerFormat.Short; dtNgayCap.SetBounds(140, y, 180, 23); this.Controls.Add(dtNgayCap); y += gap;

            l = new Label { Text = "Hạn sử dụng:" }; l.SetBounds(20, y, 100, 23); this.Controls.Add(l);
            dtHan = new DateTimePicker(); dtHan.Format = DateTimePickerFormat.Short; dtHan.SetBounds(140, y, 180, 23); this.Controls.Add(dtHan); y += gap;

            chkLePhi = new CheckBox { Text = "Đã đóng lệ phí năm" }; chkLePhi.SetBounds(140, y, 200, 23); this.Controls.Add(chkLePhi); y += gap;

            btnThem = new Button { Text = "Thêm độc giả" }; btnThem.SetBounds(20, y, 150, 30); btnThem.Click += new System.EventHandler(this.btnThem_Click); this.Controls.Add(btnThem); y += 40;
            btnCapNhat = new Button { Text = "Cập nhật độc giả" }; btnCapNhat.SetBounds(20, y, 150, 30); btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click); this.Controls.Add(btnCapNhat); y += 40;
            btnCapThe = new Button { Text = "Cấp thẻ" }; btnCapThe.SetBounds(20, y, 150, 30); btnCapThe.Click += new System.EventHandler(this.btnCapThe_Click); this.Controls.Add(btnCapThe); y += 40;
            btnGiaHan = new Button { Text = "Gia hạn" }; btnGiaHan.SetBounds(20, y, 150, 30); btnGiaHan.Click += new System.EventHandler(this.btnGiaHan_Click); this.Controls.Add(btnGiaHan); y += 40;
            btnLamMoi = new Button { Text = "Làm mới" }; btnLamMoi.SetBounds(20, y, 150, 30); btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click); this.Controls.Add(btnLamMoi);

            dgvDocGia = new DataGridView(); dgvDocGia.SetBounds(360, 20, 800, 660);
            dgvDocGia.ReadOnly = true; dgvDocGia.MultiSelect = false; dgvDocGia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.SelectionChanged += new System.EventHandler(this.dgvDocGia_SelectionChanged);
            this.Controls.Add(dgvDocGia);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(1070, 695, 90, 30); btnDong.Click += new System.EventHandler(this.btnDong_Click); this.Controls.Add(btnDong);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1190, 790);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmDocGia";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Độc giả và thẻ";
            this.Load += new System.EventHandler(this.FrmDocGia_Load);
            this.ResumeLayout(false);
        }
    }
}
