using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Button btnDanhMuc, btnPhong, btnDatPhong, btnDichVu, btnTraPhong, btnThongKe, btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.btnDanhMuc = new Button();
            this.btnPhong = new Button();
            this.btnDatPhong = new Button();
            this.btnDichVu = new Button();
            this.btnTraPhong = new Button();
            this.btnThongKe = new Button();
            this.btnThoat = new Button();
            this.SuspendLayout();

            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.SetBounds(20, 20, 640, 50);

            this.btnDanhMuc.Text = "Danh mục";
            this.btnDanhMuc.SetBounds(30, 90, 190, 60);
            this.btnDanhMuc.Click += new System.EventHandler(this.btnDanhMuc_Click);

            this.btnPhong.Text = "Phòng - Tiện nghi";
            this.btnPhong.SetBounds(240, 90, 190, 60);
            this.btnPhong.Click += new System.EventHandler(this.btnPhong_Click);

            this.btnDatPhong.Text = "Đặt / Nhận phòng";
            this.btnDatPhong.SetBounds(450, 90, 190, 60);
            this.btnDatPhong.Click += new System.EventHandler(this.btnDatPhong_Click);

            this.btnDichVu.Text = "Sử dụng dịch vụ";
            this.btnDichVu.SetBounds(30, 165, 190, 60);
            this.btnDichVu.Click += new System.EventHandler(this.btnDichVu_Click);

            this.btnTraPhong.Text = "Trả phòng - Thanh toán";
            this.btnTraPhong.SetBounds(240, 165, 190, 60);
            this.btnTraPhong.Click += new System.EventHandler(this.btnTraPhong_Click);

            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.SetBounds(450, 165, 190, 60);
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);

            this.btnThoat.Text = "Thoát";
            this.btnThoat.SetBounds(240, 240, 190, 50);
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 330);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDanhMuc);
            this.Controls.Add(this.btnPhong);
            this.Controls.Add(this.btnDatPhong);
            this.Controls.Add(this.btnDichVu);
            this.Controls.Add(this.btnTraPhong);
            this.Controls.Add(this.btnThongKe);
            this.Controls.Add(this.btnThoat);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý khách sạn";
            this.ResumeLayout(false);
        }
    }
}