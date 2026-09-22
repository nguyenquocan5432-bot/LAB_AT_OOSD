using System.Windows.Forms;
using System.Drawing;

namespace QuanLyThuVien.Forms
{
    partial class FrmThongKe
    {
        private System.ComponentModel.IContainer components = null;
        private DateTimePicker dtTu, dtDen;
        private Button btnThongKe, btnDong;
        private Label lblMuon, lblQuaHan, lblMat, lblHuHong, lblPhiPhat;
        private DataGridView dgvPhat;

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

            btnThongKe = new Button { Text = "Thống kê" }; btnThongKe.SetBounds(510, 16, 110, 28); btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click); this.Controls.Add(btnThongKe);

            lblMuon = new Label(); lblMuon.SetBounds(20, 60, 300, 23); this.Controls.Add(lblMuon);
            lblQuaHan = new Label(); lblQuaHan.SetBounds(20, 90, 300, 23); this.Controls.Add(lblQuaHan);
            lblMat = new Label(); lblMat.SetBounds(20, 120, 300, 23); this.Controls.Add(lblMat);
            lblHuHong = new Label(); lblHuHong.SetBounds(20, 150, 300, 23); this.Controls.Add(lblHuHong);
            lblPhiPhat = new Label(); lblPhiPhat.Font = new Font("Segoe UI", 10F, FontStyle.Bold); lblPhiPhat.SetBounds(20, 180, 300, 23); this.Controls.Add(lblPhiPhat);

            dgvPhat = new DataGridView(); dgvPhat.SetBounds(20, 220, 1030, 420);
            dgvPhat.ReadOnly = true; dgvPhat.AllowUserToAddRows = false;
            this.Controls.Add(dgvPhat);

            btnDong = new Button { Text = "Đóng" }; btnDong.SetBounds(960, 650, 90, 30); btnDong.Click += new System.EventHandler(this.btnDong_Click); this.Controls.Add(btnDong);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1070, 700);
            this.Font = new Font("Segoe UI", 9.5F);
            this.Name = "FrmThongKe";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Thống kê";
            this.Load += new System.EventHandler(this.FrmThongKe_Load);
            this.ResumeLayout(false);
        }
    }
}
