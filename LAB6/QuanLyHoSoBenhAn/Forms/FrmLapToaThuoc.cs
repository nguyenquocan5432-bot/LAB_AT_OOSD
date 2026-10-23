using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public partial class FrmLapToaThuoc : Form
    {
        private BindingList<ChiTietToaItem> _dsChiTiet;

        public FrmLapToaThuoc()
        {
            InitializeComponent();
            FormHelper.Nap(cboBacSi, DanhMucService.DsBacSi(), "HoTen", "MaBacSi");
            FormHelper.Nap(cboThuoc, DanhMucService.DsThuoc(), "TenThuoc", "MaThuoc");

            _dsChiTiet = new BindingList<ChiTietToaItem>();
            gridChiTiet.DataSource = _dsChiTiet;
        }

        private void btnThemThuoc_Click(object sender, EventArgs e)
        {
            if (cboThuoc.SelectedValue == null) { MessageBox.Show("Chon thuoc"); return; }
            int soLuong;
            if (!int.TryParse(txtSoLuong.Text, out soLuong) || soLuong <= 0)
            { MessageBox.Show("So luong khong hop le"); return; }

            _dsChiTiet.Add(new ChiTietToaItem
            {
                MaThuoc = FormHelper.Gia(cboThuoc),
                TenThuoc = cboThuoc.Text,
                LieuDung = txtLieuDung.Text,
                SoLuong = soLuong
            });
            txtLieuDung.Clear(); txtSoLuong.Clear();
        }

        private void btnXoaThuoc_Click(object sender, EventArgs e)
        {
            if (gridChiTiet.CurrentRow?.DataBoundItem is ChiTietToaItem item)
                _dsChiTiet.Remove(item);
        }

        private void btnLuuToa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhieuKham.Text)) { MessageBox.Show("Nhap ma phieu kham"); return; }

            var kq = ToaThuocService.LapToa(txtMaPhieuKham.Text.Trim(), FormHelper.Gia(cboBacSi), txtGhiChu.Text,
                _dsChiTiet.ToList());

            if (FormHelper.Bao(kq))
            {
                _dsChiTiet.Clear();
                txtGhiChu.Clear();
            }
        }
    }
}
