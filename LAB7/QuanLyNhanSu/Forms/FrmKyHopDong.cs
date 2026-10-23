using System;
using System.Data;
using System.Windows.Forms;
using QuanLyNhanSu.Services;

namespace QuanLyNhanSu.Forms
{
    public partial class FrmKyHopDong : Form
    {
        public FrmKyHopDong()
        {
            InitializeComponent();
            cboLoaiHD.Items.AddRange(new object[] { QuyDinh.DaiHan, QuyDinh.NganHan });
            cboLoaiHD.SelectedIndex = 0;
            txtNguoiKy.Text = "Giam doc Vo Thanh Son";

            var cv = DanhMucService.DsChucVu();
            var dong = cv.NewRow();
            dong["MaCV"] = ""; dong["TenCV"] = "(Khong co chuc vu)"; dong["PhuCapMacDinh"] = 0m;
            cv.Rows.InsertAt(dong, 0);
            FormHelper.Nap(cboChucVu, cv, "TenCV", "MaCV");

            NapNhanVienThuViec();
            NapDsHopDong();
        }

        private void NapNhanVienThuViec()
        {
            var dt = TuyenDungService.DsThuViec();
            dt.Columns.Add("HienThi", typeof(string), "MaNV + ' - ' + HoTen");
            FormHelper.Nap(cboNhanVien, dt, "HienThi", "MaNV");
            cboNhanVien_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void NapDsHopDong() => gridHopDong.DataSource = HopDongService.DsHopDong();

        // Hien thoi han thu viec, goi y ngay bat dau hop dong = ngay ke tiep sau khi het thu viec.
        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNhanVien.SelectedItem is DataRowView r)
            {
                DateTime ketThuc = (DateTime)r["NgayKetThucTV"];
                lblThuViec.Text = "Thu viec: " + ((DateTime)r["NgayBatDauTV"]).ToString("dd/MM/yyyy") + " - " + ketThuc.ToString("dd/MM/yyyy")
                                + "  (" + r["TenPB"] + ")";
                dtpBatDau.Value = ketThuc.AddDays(1);
                dtpKetThuc.Value = ketThuc.AddDays(1).AddYears(1).AddDays(-1);
            }
            else lblThuViec.Text = "Khong co nhan vien thu viec";
        }

        // Hop dong dai han: lap ho so BHYT/BHXH; ngan han: bat buoc ngay ket thuc.
        private void cboLoaiHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool daiHan = cboLoaiHD.SelectedItem?.ToString() == QuyDinh.DaiHan;
            grpBaoHiem.Enabled = daiHan;
            dtpKetThuc.Enabled = !daiHan;
        }

        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedItem is DataRowView r)
                txtPhuCapCV.Text = ((decimal)r["PhuCapMacDinh"]).ToString("0");
        }

        private void btnKyHopDong_Click(object sender, EventArgs e)
        {
            decimal lcb, pcl, pcdh, pccv;
            if (!FormHelper.Tien(txtLuongCoBan, "Luong co ban", out lcb)) return;
            if (!FormHelper.Tien(txtPhuCapLuong, "Phu cap luong", out pcl)) return;
            if (!FormHelper.Tien(txtPhuCapDocHai, "Phu cap doc hai", out pcdh)) return;
            if (!FormHelper.Tien(txtPhuCapCV, "Phu cap chuc vu", out pccv)) return;

            string loai = cboLoaiHD.SelectedItem.ToString();
            string maCV = FormHelper.Gia(cboChucVu);
            var hd = new ThongTinHopDong
            {
                MaNV = FormHelper.Gia(cboNhanVien),
                LoaiHD = loai,
                NgayBatDau = dtpBatDau.Value.Date,
                NgayKetThuc = loai == QuyDinh.NganHan ? dtpKetThuc.Value.Date : (DateTime?)null,
                NoiDung = txtNoiDung.Text,
                NguoiKy = txtNguoiKy.Text,
                LuongCoBan = lcb,
                PhuCapLuong = pcl,
                PhuCapDocHai = pcdh,
                MaCV = string.IsNullOrEmpty(maCV) ? null : maCV,
                PhuCapChucVu = string.IsNullOrEmpty(maCV) ? 0 : pccv,
                SoBHXH = txtSoBHXH.Text,
                SoBHYT = txtSoBHYT.Text,
                NoiKCB = txtNoiKCB.Text
            };

            if (FormHelper.Bao(HopDongService.KyHopDong(hd)))
            {
                txtLuongCoBan.Clear(); txtPhuCapLuong.Clear(); txtPhuCapDocHai.Clear();
                txtSoBHXH.Clear(); txtSoBHYT.Clear(); txtNoiKCB.Clear(); txtNoiDung.Clear();
                NapNhanVienThuViec();
                NapDsHopDong();
            }
        }
    }
}
