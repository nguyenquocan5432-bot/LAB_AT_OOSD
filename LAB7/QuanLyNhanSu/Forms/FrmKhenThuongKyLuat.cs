using System;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmKhenThuongKyLuat : Form
    {
        public FrmKhenThuongKyLuat()
        {
            InitializeComponent();
            FormHelper.Nap(cboNhanVienKT, DanhMucService.DsNhanVienDangLam(), "HienThi", "MaNV");
            FormHelper.Nap(cboPhongBanKT, DanhMucService.DsPhongBan(), "TenPB", "MaPB");
            FormHelper.Nap(cboNhanVienKL, DanhMucService.DsNhanVienDangLam(), "HienThi", "MaNV");
            FormHelper.Nap(cboPhongBanKL, DanhMucService.DsPhongBan(), "TenPB", "MaPB");

            cboLoaiKT.Items.AddRange(QuyDinh.LoaiKhenThuong);
            cboLoaiKT.SelectedIndex = 0;
            cboHinhThucChi.Items.AddRange(new object[] { QuyDinh.PhieuChi, QuyDinh.BangLuong });
            cboHinhThucChi.SelectedIndex = 0;
            cboHinhThucKL.Items.AddRange(new object[] { "Khien trach", "Canh cao", "Phat tien", "Ha bac luong" });
            cboHinhThucKL.SelectedIndex = 0;
            cboHinhThucThu.Items.AddRange(new object[] { QuyDinh.TruLuong, QuyDinh.PhieuThu });
            cboHinhThucThu.SelectedIndex = 0;
            txtNguoiKyKT.Text = txtNguoiKyKL.Text = "Giam doc Vo Thanh Son";

            radCaNhanKT.Checked = true;
            radCaNhanKL.Checked = true;
            NapDs();
        }

        private void NapDs()
        {
            gridKhenThuong.DataSource = KhenThuongService.DsKhenThuong();
            gridKyLuat.DataSource = KhenThuongService.DsKyLuat();
        }

        private void radDoiTuongKT_CheckedChanged(object sender, EventArgs e)
        {
            cboNhanVienKT.Enabled = radCaNhanKT.Checked;
            cboPhongBanKT.Enabled = !radCaNhanKT.Checked;
        }

        private void radDoiTuongKL_CheckedChanged(object sender, EventArgs e)
        {
            cboNhanVienKL.Enabled = radCaNhanKL.Checked;
            cboPhongBanKL.Enabled = !radCaNhanKL.Checked;
        }

        private void btnLapKT_Click(object sender, EventArgs e)
        {
            decimal soTien;
            if (!FormHelper.Tien(txtSoTienKT, "So tien khen thuong", out soTien)) return;
            var kt = new ThongTinKhenThuong
            {
                LoaiKT = cboLoaiKT.SelectedItem.ToString(),
                DoiTuong = radCaNhanKT.Checked ? QuyDinh.CaNhan : QuyDinh.TapThe,
                MaNV = FormHelper.Gia(cboNhanVienKT),
                MaPB = FormHelper.Gia(cboPhongBanKT),
                LyDo = txtLyDoKT.Text,
                SoTien = soTien,
                HinhThucChi = cboHinhThucChi.SelectedItem.ToString(),
                NguoiKy = txtNguoiKyKT.Text,
                NguoiNhan = txtNguoiNhan.Text
            };
            if (FormHelper.Bao(KhenThuongService.LapQuyetDinhKhenThuong(kt)))
            {
                txtLyDoKT.Clear(); txtSoTienKT.Clear(); txtNguoiNhan.Clear();
                NapDs();
            }
        }

        private void btnLapKL_Click(object sender, EventArgs e)
        {
            decimal soTien;
            if (!FormHelper.Tien(txtSoTienKL, "So tien phat", out soTien)) return;
            var kl = new ThongTinKyLuat
            {
                DoiTuong = radCaNhanKL.Checked ? QuyDinh.CaNhan : QuyDinh.TapThe,
                MaNV = FormHelper.Gia(cboNhanVienKL),
                MaPB = FormHelper.Gia(cboPhongBanKL),
                LyDo = txtLyDoKL.Text,
                HinhThucKL = cboHinhThucKL.Text,
                SoTienPhat = soTien,
                HinhThucThu = cboHinhThucThu.SelectedItem.ToString(),
                NguoiKy = txtNguoiKyKL.Text,
                NguoiNop = txtNguoiNop.Text
            };
            if (FormHelper.Bao(KhenThuongService.LapQuyetDinhKyLuat(kl)))
            {
                txtLyDoKL.Clear(); txtSoTienKL.Clear(); txtNguoiNop.Clear();
                NapDs();
            }
        }
    }
}
