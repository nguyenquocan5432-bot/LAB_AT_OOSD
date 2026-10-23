using System;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmTraCuuHoSo
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMaHoSo, lblTuKhoa;
        private TextBox txtMaHoSo, txtTuKhoa;
        private Button btnTimTheoMa, btnTimTheoTuKhoa, btnXemLichSu;
        private DataGridView gridKetQua, gridLichSu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMaHoSo = new Label { Text = "Ma ho so:" }; lblMaHoSo.SetBounds(15, 20, 70, 23);
            txtMaHoSo = new TextBox(); txtMaHoSo.SetBounds(90, 20, 150, 23);
            btnTimTheoMa = new Button { Text = "Tim theo ma" }; btnTimTheoMa.SetBounds(250, 18, 110, 27);
            btnTimTheoMa.Click += new EventHandler(this.btnTimTheoMa_Click);

            lblTuKhoa = new Label { Text = "Ten / SDT:" }; lblTuKhoa.SetBounds(400, 20, 70, 23);
            txtTuKhoa = new TextBox(); txtTuKhoa.SetBounds(475, 20, 180, 23);
            btnTimTheoTuKhoa = new Button { Text = "Tim kiem" }; btnTimTheoTuKhoa.SetBounds(665, 18, 90, 27);
            btnTimTheoTuKhoa.Click += new EventHandler(this.btnTimTheoTuKhoa_Click);

            gridKetQua = new DataGridView(); gridKetQua.SetBounds(15, 60, 740, 180);
            gridKetQua.ReadOnly = true; gridKetQua.AllowUserToAddRows = false;

            btnXemLichSu = new Button { Text = "Xem lich su dieu tri cua ho so da chon" };
            btnXemLichSu.SetBounds(15, 250, 260, 27);
            btnXemLichSu.Click += new EventHandler(this.btnXemLichSu_Click);

            gridLichSu = new DataGridView(); gridLichSu.SetBounds(15, 285, 740, 200);
            gridLichSu.ReadOnly = true; gridLichSu.AllowUserToAddRows = false;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(770, 510);
            this.Controls.Add(lblMaHoSo); this.Controls.Add(txtMaHoSo); this.Controls.Add(btnTimTheoMa);
            this.Controls.Add(lblTuKhoa); this.Controls.Add(txtTuKhoa); this.Controls.Add(btnTimTheoTuKhoa);
            this.Controls.Add(gridKetQua);
            this.Controls.Add(btnXemLichSu);
            this.Controls.Add(gridLichSu);
            this.Text = "Tra cuu ho so benh an";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
