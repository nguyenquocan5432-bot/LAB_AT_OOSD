using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnDanhMuc, btnDangKy, btnTraCuu, btnLapPhieuKham, btnLapToaThuoc, btnDatLich, btnThanhToan, btnThoat;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.btnDanhMuc = new Button();
            this.btnDangKy = new Button();
            this.btnTraCuu = new Button();
            this.btnLapPhieuKham = new Button();
            this.btnLapToaThuoc = new Button();
            this.btnDatLich = new Button();
            this.btnThanhToan = new Button();
            this.btnThoat = new Button();
            this.SuspendLayout();

            this.lblTitle.SetBounds(30, 20, 440, 30);
            this.lblTitle.Text = "QUAN LY HO SO BENH AN BENH NHAN";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            this.btnDanhMuc.SetBounds(30, 70, 220, 40);
            this.btnDanhMuc.Text = "1. Danh muc (Khoa/BS/Thuoc)";
            this.btnDanhMuc.Click += new System.EventHandler(this.btnDanhMuc_Click);

            this.btnDangKy.SetBounds(30, 120, 220, 40);
            this.btnDangKy.Text = "2. Dang ky benh nhan moi";
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);

            this.btnTraCuu.SetBounds(30, 170, 220, 40);
            this.btnTraCuu.Text = "3. Tra cuu ho so benh an";
            this.btnTraCuu.Click += new System.EventHandler(this.btnTraCuu_Click);

            this.btnLapPhieuKham.SetBounds(30, 220, 220, 40);
            this.btnLapPhieuKham.Text = "4. Lap phieu kham benh";
            this.btnLapPhieuKham.Click += new System.EventHandler(this.btnLapPhieuKham_Click);

            this.btnLapToaThuoc.SetBounds(270, 70, 220, 40);
            this.btnLapToaThuoc.Text = "5. Lap toa thuoc";
            this.btnLapToaThuoc.Click += new System.EventHandler(this.btnLapToaThuoc_Click);

            this.btnDatLich.SetBounds(270, 120, 220, 40);
            this.btnDatLich.Text = "6. Dat lich kham dinh ky";
            this.btnDatLich.Click += new System.EventHandler(this.btnDatLich_Click);

            this.btnThanhToan.SetBounds(270, 170, 220, 40);
            this.btnThanhToan.Text = "7. Thanh toan / Xuat vien";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);

            this.btnThoat.SetBounds(270, 220, 220, 40);
            this.btnThoat.Text = "Thoat";
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(540, 300);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDanhMuc);
            this.Controls.Add(this.btnDangKy);
            this.Controls.Add(this.btnTraCuu);
            this.Controls.Add(this.btnLapPhieuKham);
            this.Controls.Add(this.btnLapToaThuoc);
            this.Controls.Add(this.btnDatLich);
            this.Controls.Add(this.btnThanhToan);
            this.Controls.Add(this.btnThoat);
            this.Text = "Quan ly Ho so Benh an";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
