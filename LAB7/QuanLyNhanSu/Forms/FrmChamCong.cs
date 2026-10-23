using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmChamCong : Form
    {
        private BindingList<ChiTietPhuTroiItem> _dsPhuTroi;

        public FrmChamCong()
        {
            InitializeComponent();
            var thangTruoc = DateTime.Today.AddMonths(-1);
            numThang.Value = thangTruoc.Month;
            numNam.Value = thangTruoc.Year;

            FormHelper.Nap(cboNhanVienCC, DanhMucService.DsNhanVienDangLam(), "HienThi", "MaNV");
            FormHelper.Nap(cboNhanVienPT, DanhMucService.DsNhanVienDangLam(), "HienThi", "MaNV");
            FormHelper.Nap(cboPhongBan, DanhMucService.DsPhongBan(), "TenPB", "MaPB");
            cboHinhThuc.Items.AddRange(new object[] { QuyDinh.TangCa, QuyDinh.NgayLe, QuyDinh.NgayNghi });
            cboHinhThuc.SelectedIndex = 0;
            txtNguoiLap.Text = "Ban cham cong";

            _dsPhuTroi = new BindingList<ChiTietPhuTroiItem>();
            gridPhuTroiTam.DataSource = _dsPhuTroi;
            NapDuLieuThang();
        }

        private void NapDuLieuThang()
        {
            int t = (int)numThang.Value, n = (int)numNam.Value;
            gridChamCong.DataSource = ChamCongService.DsChamCong(t, n);
            gridPhuTroi.DataSource = ChamCongService.DsPhuTroi(t, n);
        }

        private void numThangNam_ValueChanged(object sender, EventArgs e) => NapDuLieuThang();

        private void btnLuuCC_Click(object sender, EventArgs e)
        {
            var kq = ChamCongService.LuuChamCong(FormHelper.Gia(cboNhanVienCC), (int)numThang.Value, (int)numNam.Value,
                numNgayCong.Value, numNghiPhep.Value, numNghiKP.Value);
            if (FormHelper.Bao(kq)) NapDuLieuThang();
        }

        private void gridChamCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string maNV = FormHelper.O(gridChamCong, "MaNV");
            if (maNV == null) return;
            cboNhanVienCC.SelectedValue = maNV;
            numNgayCong.Value = Convert.ToDecimal(FormHelper.O(gridChamCong, "SoNgayCong"));
            numNghiPhep.Value = Convert.ToDecimal(FormHelper.O(gridChamCong, "SoNgayNghiPhep"));
            numNghiKP.Value = Convert.ToDecimal(FormHelper.O(gridChamCong, "SoNgayNghiKP"));
        }

        private void btnThemPT_Click(object sender, EventArgs e)
        {
            if (cboNhanVienPT.SelectedValue == null) { MessageBox.Show("Chon nhan vien"); return; }
            string maNV = FormHelper.Gia(cboNhanVienPT);
            if (_dsPhuTroi.Any(x => x.MaNV == maNV)) { MessageBox.Show("Nhan vien da co tren phieu"); return; }
            _dsPhuTroi.Add(new ChiTietPhuTroiItem
            {
                MaNV = maNV,
                HoTen = ((DataRowView)cboNhanVienPT.SelectedItem)["HoTen"].ToString(),
                SoGio = numSoGio.Value
            });
        }

        private void btnXoaPT_Click(object sender, EventArgs e)
        {
            if (gridPhuTroiTam.CurrentRow?.DataBoundItem is ChiTietPhuTroiItem item) _dsPhuTroi.Remove(item);
        }

        private void btnLuuPT_Click(object sender, EventArgs e)
        {
            var kq = ChamCongService.LapPhieuPhuTroi(dtpNgayPT.Value.Date, cboHinhThuc.SelectedItem.ToString(),
                FormHelper.Gia(cboPhongBan), txtNguoiLap.Text, txtGhiChu.Text, _dsPhuTroi.ToList());
            if (FormHelper.Bao(kq)) { _dsPhuTroi.Clear(); txtGhiChu.Clear(); NapDuLieuThang(); }
        }
    }
}
