using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.Forms
{
    partial class FrmThongKe
    {
        private System.ComponentModel.IContainer components = null;
        private DateTimePicker dtTu, dtDen;
        private Button btnTK, btnDong;
        private DataGridView dgvTongHop, dgvDV;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            Label l;

            l = new Label { Text = "Từ ngày:" }; l.SetBounds(20, 20, 70, 23); this.Controls.Add(l);
            dtTu = new DateTimePicker(); dtTu.Format = DateTimePickerFormat.Short; dtTu.SetBounds(100, 18, 140, 23); this.Controls.Add(dtTu);

            l = new Label { Text = "Đến ngày:" }; l.SetBounds(260, 20, 70, 23); this.Controls.Add(l);
            dtDen = new DateTimePicker(); dtDen.Format = DateTimePickerFormat.Short; dtDen.SetBounds(340, 18, 140, 23); this.Controls.Add(dtDen);

            btnTK = new Button { Text = "Thống kê" }; btnTK.SetBounds(510, 16, 110, 28); btnTK.Click += new System.EventHandler(this.btnTK_Click); this.Controls.Add(btnTK);

            Label lh = new Label { Text = "Tổng hợp" }; lh.SetBounds(20, 60, 200, 20); this.Controls.Add(lh);
            dgvTongHop = new DataGridView(); dgvTongHop.SetBounds(20, 85, 990, 90); dgvTongHop.ReadOnly = true; dgvTongHop.AllowUserToAddRows = false; this.Controls.Add(dgvTongHop);

            Label ld = new Label { Text = "Dịch vụ sử dụng" }; ld.SetBounds(20, 195, 250, 20); this.Controls.Add(ld);
            dgvDV = new DataGridView(); dgvDV.SetBounds(20, 220, 990, 400); dgvDV.ReadOnly = true; dgvDV.AllowUserToAddRows = false; this.Controls.Add(dgvDV);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(910, 635, 100, 30); btnDong.Click += new System.EventHandler(this.btnDong_Click); this.Controls.Add(btnDong);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1030, 690);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmThongKe";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Thống kê khách sạn";
            this.ResumeLayout(false);
        }
    }
}
