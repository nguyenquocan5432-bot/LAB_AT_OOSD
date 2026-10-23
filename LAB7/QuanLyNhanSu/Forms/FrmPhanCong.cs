using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmPhanCong : Form
    {
        private BindingList<ChiTietPhanCongItem> _dsTam;
        private DataTable _dsNhanVien;

        public FrmPhanCong()
        {
            InitializeComponent();
            _dsTam = new BindingList<ChiTietPhanCongItem>();
            gridTam.DataSource = _dsTam;
            _dsNhanVien = DanhMucService.DsNhanVienDangLam();
            FormHelper.Nap(cboCa, DanhMucService.DsCaLamViec(), "TenCa", "MaCa");

            // Mac dinh: tuan ke tiep, bat dau tu thu Hai.
            DateTime homNay = DateTime.Today;
            int lech = ((int)DayOfWeek.Monday - (int)homNay.DayOfWeek + 7) % 7;
            dtpTuNgay.Value = homNay.AddDays(lech == 0 ? 7 : lech);
            dtpNgayLam.Value = dtpTuNgay.Value;

            FormHelper.Nap(cboPhongBan, DanhMucService.DsPhongBan(), "TenPB", "MaPB");
        }

        private void cboPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maPB = FormHelper.Gia(cboPhongBan);
            if (maPB == null || _dsNhanVien == null) return;
            var view = new DataView(_dsNhanVien) { RowFilter = "MaPB = '" + maPB.Replace("'", "''") + "'" };
            FormHelper.Nap(cboNhanVien, view.ToTable(), "HienThi", "MaNV");
            _dsTam.Clear();
            gridKeHoach.DataSource = ChamCongService.DsPhanCong(maPB);
        }

        private void dtpTuNgay_ValueChanged(object sender, EventArgs e)
        {
            lblDenNgay.Text = "den " + dtpTuNgay.Value.AddDays(6).ToString("dd/MM/yyyy");
            dtpNgayLam.Value = dtpTuNgay.Value;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboNhanVien.SelectedValue == null) { MessageBox.Show("Phong ban chua co nhan vien"); return; }
            if (cboCa.SelectedValue == null) { MessageBox.Show("Chon ca lam viec"); return; }
            var row = (DataRowView)cboNhanVien.SelectedItem;
            _dsTam.Add(new ChiTietPhanCongItem
            {
                MaNV = FormHelper.Gia(cboNhanVien),
                HoTen = row["HoTen"].ToString(),
                NgayLam = dtpNgayLam.Value.Date,
                MaCa = FormHelper.Gia(cboCa),
                TenCa = cboCa.Text
            });
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridTam.CurrentRow?.DataBoundItem is ChiTietPhanCongItem item) _dsTam.Remove(item);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var kq = ChamCongService.LapKeHoachPhanCong(FormHelper.Gia(cboPhongBan), dtpTuNgay.Value.Date,
                txtNguoiLap.Text, _dsTam.ToList());
            if (FormHelper.Bao(kq))
            {
                _dsTam.Clear();
                gridKeHoach.DataSource = ChamCongService.DsPhanCong(FormHelper.Gia(cboPhongBan));
            }
        }
    }
}
