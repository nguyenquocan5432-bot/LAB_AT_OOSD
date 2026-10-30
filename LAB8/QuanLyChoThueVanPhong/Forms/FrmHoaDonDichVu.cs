using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;

namespace QuanLyChoThueVanPhong.Forms
{
    public partial class FrmHoaDonDichVu : Form
    {
        private BindingList<ChiTietDichVuItem> _dsCT;

        public FrmHoaDonDichVu()
        {
            InitializeComponent();
            _dsCT = new BindingList<ChiTietDichVuItem>();
            _dsCT.ListChanged += (s, e) => lblTong.Text = "Tong tien: " + _dsCT.Sum(x => x.ThanhTien).ToString("N0") + " d";
            gridChiTietTam.DataSource = _dsCT;
            numThang.Value = DateTime.Today.Month;
            numNam.Value = DateTime.Today.Year;
            FormHelper.Nap(cboDichVu, DanhMucService.DsDichVu(), "TenDV", "MaDV");
            FormHelper.Nap(cboHopDong, HopDongService.DsHopDong(true), "HienThi", "SoHD");
        }

        private void NapHoaDon()
        {
            string soHD = FormHelper.Gia(cboHopDong);
            gridHoaDon.DataSource = soHD == null ? null : DichVuService.DsHoaDon(soHD);
        }

        private void cboHopDong_SelectedIndexChanged(object sender, EventArgs e) => NapHoaDon();

        private void btnThemDV_Click(object sender, EventArgs e)
        {
            if (!(cboDichVu.SelectedItem is DataRowView r)) return;
            string ma = r["MaDV"].ToString();
            if (_dsCT.Any(x => x.MaDV == ma)) { MessageBox.Show("Dich vu da co tren hoa don"); return; }
            if (numSoLuong.Value <= 0) { MessageBox.Show("So luong phai lon hon 0"); return; }
            _dsCT.Add(new ChiTietDichVuItem { MaDV = ma, TenDV = r["TenDV"].ToString(), SoLuong = numSoLuong.Value, DonGia = (decimal)r["DonGia"] });
        }

        private void btnXoaDV_Click(object sender, EventArgs e)
        {
            if (gridChiTietTam.CurrentRow?.DataBoundItem is ChiTietDichVuItem item) _dsCT.Remove(item);
        }

        private void btnLapHoaDon_Click(object sender, EventArgs e)
        {
            var kq = DichVuService.LapHoaDon(FormHelper.Gia(cboHopDong), (int)numThang.Value, (int)numNam.Value, _dsCT.ToList());
            if (FormHelper.Bao(kq)) { _dsCT.Clear(); NapHoaDon(); }
        }

        private void gridHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            string ma = FormHelper.O(gridHoaDon, "MaHoaDon");
            gridChiTietHD.DataSource = ma == null ? null : DichVuService.ChiTietHoaDon(ma);
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string ma = FormHelper.O(gridHoaDon, "MaHoaDon");
            if (ma == null) { MessageBox.Show("Chon hoa don"); return; }
            if (FormHelper.Bao(DichVuService.ThanhToanHoaDon(ma))) NapHoaDon();
        }
    }
}
