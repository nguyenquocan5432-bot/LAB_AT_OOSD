using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.Forms
{
    partial class FrmDichVu
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboLuot, cboDV, cboNV;
        private TextBox txtPhong;
        private DateTimePicker dtNgay;
        private NumericUpDown numSL;
        private Button btnGhi, btnDong;
        private DataGridView dgvLichSu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            Label l;

            l = new Label { Text = "Phiếu lưu trú:" }; l.SetBounds(20, 20, 90, 23); this.Controls.Add(l);
            cboLuot = new ComboBox(); cboLuot.DropDownStyle = ComboBoxStyle.DropDownList; cboLuot.SetBounds(120, 18, 150, 23);
            cboLuot.SelectedIndexChanged += new System.EventHandler(this.cboLuot_SelectedIndexChanged);
            this.Controls.Add(cboLuot);

            l = new Label { Text = "Phòng:" }; l.SetBounds(290, 20, 60, 23); this.Controls.Add(l);
            txtPhong = new TextBox(); txtPhong.ReadOnly = true; txtPhong.SetBounds(350, 18, 100, 23); this.Controls.Add(txtPhong);

            l = new Label { Text = "Dịch vụ:" }; l.SetBounds(20, 55, 90, 23); this.Controls.Add(l);
            cboDV = new ComboBox(); cboDV.DropDownStyle = ComboBoxStyle.DropDownList; cboDV.SetBounds(120, 53, 200, 23); this.Controls.Add(cboDV);

            l = new Label { Text = "Ngày sử dụng:" }; l.SetBounds(20, 90, 90, 23); this.Controls.Add(l);
            dtNgay = new DateTimePicker(); dtNgay.Format = DateTimePickerFormat.Short; dtNgay.SetBounds(120, 88, 150, 23); this.Controls.Add(dtNgay);

            l = new Label { Text = "Số lượng:" }; l.SetBounds(290, 90, 70, 23); this.Controls.Add(l);
            numSL = new NumericUpDown(); numSL.Minimum = 1; numSL.Maximum = 1000; numSL.Value = 1; numSL.SetBounds(360, 88, 90, 23); this.Controls.Add(numSL);

            l = new Label { Text = "Nhân viên:" }; l.SetBounds(20, 125, 90, 23); this.Controls.Add(l);
            cboNV = new ComboBox(); cboNV.DropDownStyle = ComboBoxStyle.DropDownList; cboNV.SetBounds(120, 123, 200, 23); this.Controls.Add(cboNV);

            btnGhi = new Button { Text = "Ghi nhận" }; btnGhi.SetBounds(500, 88, 120, 30); btnGhi.Click += new System.EventHandler(this.btnGhi_Click); this.Controls.Add(btnGhi);

            dgvLichSu = new DataGridView(); dgvLichSu.SetBounds(20, 170, 850, 420); dgvLichSu.ReadOnly = true; dgvLichSu.AllowUserToAddRows = false; this.Controls.Add(dgvLichSu);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(750, 605, 120, 32); btnDong.Click += new System.EventHandler(this.btnDong_Click); this.Controls.Add(btnDong);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(900, 660);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmDichVu";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Sử dụng dịch vụ";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.ResumeLayout(false);
        }
    }
}
