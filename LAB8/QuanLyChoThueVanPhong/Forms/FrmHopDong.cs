using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmHopDong : Form
    {
        private BindingList<PhongChonItem> _dsPhong;

        public FrmHopDong()
        {
            InitializeComponent();
            _dsPhong = new BindingList<PhongChonItem>();
            _dsPhong.ListChanged += (s, e) => CapNhatTongTien();
            gridPhongChon.DataSource = _dsPhong;
            FormHelper.Nap(cboKhachHang, DanhMucService.DsKhachHang(), "HienThi", "MaKH");
            dtpBatDau.MinDate = DateTime.Today;
            dtpBatDau.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
            numSoThang.Value = QuyDinh.SoThangToiThieu;
            txtNguoiThu.Text = "Ke toan Mai";
            txtNoiDung.Text = "Tra truoc tien thue 6 thang dau; tu thang thu 7 tra vao dau moi thang. Gia thue chua gom dien va dich vu; "
                            + "chi phi dich vu (dien, nuoc, internet...) thanh toan cuoi thang.";
            NapPhongTrong();
            NapDsHopDong();
            CapNhatTongTien();
        }

        private void NapPhongTrong() => FormHelper.Nap(cboPhong, DanhMucService.DsPhongTrong(), "HienThi", "MaPhong");
        private void NapDsHopDong() => gridHopDong.DataSource = HopDongService.DsHopDong();

        private void CapNhatTongTien()
        {
            decimal tong = _dsPhong.Sum(p => p.GiaThue);
            lblTongTien.Text = "Tong gia thue/thang: " + tong.ToString("N0") + " d    Tra truoc 6 thang: "
                             + (tong * QuyDinh.SoThangTraTruoc).ToString("N0") + " d";
        }

        private void btnThemPhong_Click(object sender, EventArgs e)
        {
            if (!(cboPhong.SelectedItem is DataRowView r)) { MessageBox.Show("Khong con phong trong"); return; }
            string ma = r["MaPhong"].ToString();
            if (_dsPhong.Any(p => p.MaPhong == ma)) { MessageBox.Show("Phong da co trong danh sach"); return; }
            _dsPhong.Add(new PhongChonItem
            {
                MaPhong = ma, Tang = r["TenTang"].ToString(), DienTich = (decimal)r["DienTich"],
                Huong = r["Huong"].ToString(), GiaThue = (decimal)r["GiaThue"]
            });
        }

        private void btnXoaPhong_Click(object sender, EventArgs e)
        {
            if (gridPhongChon.CurrentRow?.DataBoundItem is PhongChonItem item) _dsPhong.Remove(item);
        }

        private void btnLapHopDong_Click(object sender, EventArgs e)
        {
            var kq = HopDongService.LapHopDong(FormHelper.Gia(cboKhachHang), dtpBatDau.Value.Date, (int)numSoThang.Value,
                _dsPhong.Select(p => p.MaPhong).ToList(), txtNoiDung.Text, txtNguoiThu.Text);
            if (FormHelper.Bao(kq)) { _dsPhong.Clear(); NapPhongTrong(); NapDsHopDong(); }
        }

        private void gridHopDong_SelectionChanged(object sender, EventArgs e)
        {
            string soHD = FormHelper.O(gridHopDong, "SoHD");
            gridChiTiet.DataSource = soHD == null ? null : HopDongService.ChiTietHopDong(soHD);
        }

        private void btnThanhLy_Click(object sender, EventArgs e)
        {
            string soHD = FormHelper.O(gridHopDong, "SoHD");
            if (soHD == null) { MessageBox.Show("Chon hop dong"); return; }
            if (MessageBox.Show("Thanh ly hop dong " + soHD + "?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (FormHelper.Bao(HopDongService.ThanhLyHopDong(soHD))) { NapPhongTrong(); NapDsHopDong(); }
        }
    }
}
