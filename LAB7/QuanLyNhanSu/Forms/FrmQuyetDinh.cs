using System;
using System.Data;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmQuyetDinh : Form
    {
        public FrmQuyetDinh()
        {
            InitializeComponent();
            txtNguoiKy.Text = "Giam doc Vo Thanh Son";
            FormHelper.Nap(cboChucVu, DanhMucService.DsChucVu(), "TenCV", "MaCV");

            var dt = DanhMucService.DsNhanVien(QuyDinh.ChinhThuc);
            dt.Columns.Add("HienThi", typeof(string), "MaNV + ' - ' + HoTen");
            FormHelper.Nap(cboNhanVien, dt, "HienThi", "MaNV");
            NapLichSu();
        }

        private void NapLichSu()
        {
            string maNV = FormHelper.Gia(cboNhanVien);
            gridLichSu.DataSource = maNV == null ? null : QuyetDinhService.LichSuQuyetDinh(maNV);
        }

        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e) => NapLichSu();

        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedItem is DataRowView r)
                txtPhuCapCV.Text = ((decimal)r["PhuCapMacDinh"]).ToString("0");
        }

        private void btnLapQDLuong_Click(object sender, EventArgs e)
        {
            decimal lcb, pcl, pcdh;
            if (!FormHelper.Tien(txtLuongCoBan, "Luong co ban", out lcb)) return;
            if (!FormHelper.Tien(txtPhuCapLuong, "Phu cap luong", out pcl)) return;
            if (!FormHelper.Tien(txtPhuCapDocHai, "Phu cap doc hai", out pcdh)) return;
            var kq = QuyetDinhService.LapQuyetDinhLuong(FormHelper.Gia(cboNhanVien), dtpHieuLucLuong.Value.Date,
                lcb, pcl, pcdh, txtNguoiKy.Text, txtGhiChu.Text);
            if (FormHelper.Bao(kq)) { txtLuongCoBan.Clear(); txtGhiChu.Clear(); NapLichSu(); }
        }

        private void btnLapQDChucVu_Click(object sender, EventArgs e)
        {
            decimal pccv;
            if (!FormHelper.Tien(txtPhuCapCV, "Phu cap chuc vu", out pccv)) return;
            var kq = QuyetDinhService.LapQuyetDinhChucVu(FormHelper.Gia(cboNhanVien), FormHelper.Gia(cboChucVu),
                dtpHieuLucCV.Value.Date, pccv, txtNguoiKy.Text);
            if (FormHelper.Bao(kq)) NapLichSu();
        }
    }
}
