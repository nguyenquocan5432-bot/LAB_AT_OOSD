using System.Windows.Forms;

namespace QuanLyThuVien.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Button btnDanhMuc, btnSach, btnDocGia, btnMuonTra, btnThongKe, btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.btnDanhMuc = new Button();
            this.btnSach = new Button();
            this.btnDocGia = new Button();
            this.btnMuonTra = new Button();
            this.btnThongKe = new Button();
            this.btnThoat = new Button();
            this.SuspendLayout();

            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ THƯ VIỆN";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.SetBounds(20, 20, 740, 60);

            int y = 110, gap = 65;
            SetupButton(this.btnDanhMuc, "Danh mục / Nhân viên", y); this.btnDanhMuc.Click += new System.EventHandler(this.btnDanhMuc_Click);
            SetupButton(this.btnSach, "Quản lý đầu sách", y += gap); this.btnSach.Click += new System.EventHandler(this.btnSach_Click);
            SetupButton(this.btnDocGia, "Độc giả và thẻ", y += gap); this.btnDocGia.Click += new System.EventHandler(this.btnDocGia_Click);
            SetupButton(this.btnMuonTra, "Mượn - Trả sách", y += gap); this.btnMuonTra.Click += new System.EventHandler(this.btnMuonTra_Click);
            SetupButton(this.btnThongKe, "Thống kê", y += gap); this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            SetupButton(this.btnThoat, "Thoát", y += gap); this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 450);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDanhMuc);
            this.Controls.Add(this.btnSach);
            this.Controls.Add(this.btnDocGia);
            this.Controls.Add(this.btnMuonTra);
            this.Controls.Add(this.btnThongKe);
            this.Controls.Add(this.btnThoat);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý thư viện";
            this.ResumeLayout(false);
        }

        private void SetupButton(Button b, string text, int y)
        {
            b.Text = text;
            b.SetBounds(270, y, 240, 60);
        }
    }
}
