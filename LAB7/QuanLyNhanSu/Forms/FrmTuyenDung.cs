using System;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmTuyenDung : Form
    {
        public FrmTuyenDung()
        {
            InitializeComponent();
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nu" });
            cboGioiTinh.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-22);
            dtpBatDauTV.Value = DateTime.Today.AddDays(7);
            dtpKetThucTV.Value = DateTime.Today.AddDays(7).AddMonths(2).AddDays(-1);
            txtNguoiKy.Text = "Giam doc Vo Thanh Son";
            NapPhongBan();
            NapDsThuViec();
        }

        // Chi hien thi phong ban dang co nhu cau tuyen dung.
        private void NapPhongBan()
        {
            var dt = DanhMucService.DsPhongBan();
            dt.DefaultView.RowFilter = "NhuCauTuyen > 0";
            FormHelper.Nap(cboPhongBan, dt.DefaultView.ToTable(), "TenPB", "MaPB");
        }

        private void NapDsThuViec() => gridThuViec.DataSource = TuyenDungService.DsThuViec();

        private void btnLapQD_Click(object sender, EventArgs e)
        {
            decimal luongTV;
            if (!FormHelper.Tien(txtLuongTV, "Luong thu viec", out luongTV)) return;

            var nv = new NhanVienMoi
            {
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value.Date,
                GioiTinh = cboGioiTinh.SelectedItem.ToString(),
                CCCD = txtCCCD.Text,
                DiaChi = txtDiaChi.Text,
                DienThoai = txtDienThoai.Text
            };
            var qd = new ThongTinTuyenDung
            {
                MaPB = FormHelper.Gia(cboPhongBan),
                NgayBatDauTV = dtpBatDauTV.Value.Date,
                NgayKetThucTV = dtpKetThucTV.Value.Date,
                LuongThuViec = luongTV,
                DieuKhoanCongTy = txtDieuKhoanCT.Text,
                DieuKhoanNhanVien = txtDieuKhoanNV.Text,
                NguoiKy = txtNguoiKy.Text
            };

            var kq = TuyenDungService.LapQuyetDinhTuyenDung(nv, qd);
            if (FormHelper.Bao(kq))
            {
                txtHoTen.Clear(); txtCCCD.Clear(); txtDiaChi.Clear(); txtDienThoai.Clear(); txtLuongTV.Clear();
                NapPhongBan();
                NapDsThuViec();
            }
        }

        private void btnChamDut_Click(object sender, EventArgs e)
        {
            string maNV = FormHelper.O(gridThuViec, "MaNV");
            if (maNV == null) { MessageBox.Show("Chon nhan vien thu viec trong danh sach"); return; }
            if (MessageBox.Show("Danh gia thu viec khong dat, cham dut thu viec cua " + maNV + "?", "Xac nhan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (FormHelper.Bao(TuyenDungService.ChamDutThuViec(maNV))) NapDsThuViec();
        }
    }
}
