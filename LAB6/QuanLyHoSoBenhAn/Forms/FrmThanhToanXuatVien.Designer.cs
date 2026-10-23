using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmThanhToanXuatVien
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView gridPhieu;
        private Label lblSoTien;
        private TextBox txtSoTien;
        private Button btnThanhToan, btnXacNhanMienPhi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            gridPhieu = new DataGridView(); gridPhieu.SetBounds(15, 15, 700, 300);
            gridPhieu.ReadOnly = true; gridPhieu.AllowUserToAddRows = false;

            lblSoTien = new Label { Text = "So tien vien phi (kham dieu tri):" }; lblSoTien.SetBounds(15, 330, 200, 23);
            txtSoTien = new TextBox(); txtSoTien.SetBounds(220, 328, 150, 23);
            btnThanhToan = new Button { Text = "Thanh toan & Xuat vien" }; btnThanhToan.SetBounds(390, 325, 180, 30);
            btnThanhToan.Click += new EventHandler(this.btnThanhToan_Click);

            btnXacNhanMienPhi = new Button { Text = "Xac nhan hoan tat (dinh ky - mien phi)" };
            btnXacNhanMienPhi.SetBounds(15, 370, 260, 30);
            btnXacNhanMienPhi.Click += new EventHandler(this.btnXacNhanMienPhi_Click);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(730, 430);
            this.Controls.Add(gridPhieu);
            this.Controls.Add(lblSoTien); this.Controls.Add(txtSoTien); this.Controls.Add(btnThanhToan);
            this.Controls.Add(btnXacNhanMienPhi);
            this.Text = "Thanh toan / Xuat vien";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
