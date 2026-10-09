using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.UI
{
    public partial class FrmDangKyDoan : Form
    {
        private readonly DangKyDoanService service = new DangKyDoanService();

        public FrmDangKyDoan()
        {
            InitializeComponent();
            btnLapPhieu.Click -= btnLapPhieu_Click;
            btnLapPhieu.Click += btnLapPhieu_Click;
            numSoNguoi.Maximum = int.MaxValue;
            numSoNguoi.DecimalPlaces = 0;
            numTienCoc.Maximum = 9999999999999999.99m;
            numTienCoc.DecimalPlaces = 2;
        }

        private async void btnLapPhieu_Click(object sender, EventArgs e)
        {
            btnLapPhieu.Enabled = false;
            btnHuy.Enabled = false;
            try
            {
                if (!dgvThanhVien.EndEdit())
                    throw new ArgumentException("Vui lòng kiểm tra ô dữ liệu đang nhập.");

                var input = new DangKyDoanInput
                {
                    SoPhieu = txtSoPhieu.Text,
                    MaDoan = txtMaDoan.Text,
                    TenCoQuanDaiDien = txtTenDoan.Text,
                    DiaChi = txtDiaChi.Text,
                    DienThoai = txtDienThoai.Text,
                    NguoiDaiDien = txtNguoiDaiDien.Text,
                    MaTour = Ui.Key(cboTour),
                    SoNguoi = decimal.ToInt32(numSoNguoi.Value),
                    NgayDi = dtpNgayDi.Value.Date,
                    DiaDiemDon = txtDiaDiemDon.Text,
                    MuaBaoHiem = chkMuaBaoHiem.Checked,
                    TienCoc = numTienCoc.Value
                };

                if (input.MuaBaoHiem)
                {
                    foreach (DataGridViewRow row in dgvThanhVien.Rows)
                    {
                        if (row.IsNewRow) continue;
                        object value = row.Cells["NgaySinh"].Value;
                        string text = Convert.ToString(value).Trim();
                        DateTime? ngaySinh = null;
                        if (value is DateTime)
                            ngaySinh = ((DateTime)value).Date;
                        else if (text.Length > 0)
                        {
                            DateTime parsed;
                            if (!DateTime.TryParse(text, CultureInfo.GetCultureInfo("vi-VN"),
                                DateTimeStyles.None, out parsed))
                                throw new ArgumentException("Ngày sinh không hợp lệ tại dòng " + (row.Index + 1) + ".");
                            ngaySinh = parsed.Date;
                        }
                        input.ThanhVien.Add(new ThanhVienDoanInput
                        {
                            HoTen = Convert.ToString(row.Cells["HoTen"].Value),
                            NgaySinh = ngaySinh,
                            SoGiayTo = Convert.ToString(row.Cells["SoGiayTo"].Value)
                        });
                    }
                }

                var result = await Task.Run(() => service.LapPhieu(input));
                if (IsDisposed || Disposing) return;
                MessageBox.Show(this, string.Format(CultureInfo.GetCultureInfo("vi-VN"),
                    "Đã lập phiếu {0}.\nKết thúc dự kiến: {1:dd/MM/yyyy}\nTổng tiền: {2:N2}\nĐã đặt cọc: {3:N2}",
                    result.SoPhieu, result.NgayKetThuc, result.TongTien, result.TienCoc),
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoPhieu.Text = Ui.Id("DK");
                Ui.Safe(this, RefreshList);
            }
            catch (ArgumentException ex)
            {
                if (!IsDisposed && !Disposing)
                    MessageBox.Show(this, ex.Message, "Dữ liệu không hợp lệ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                if (!IsDisposed && !Disposing)
                    MessageBox.Show(this, ex.Message, "Không hoàn tất thao tác",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                if (!IsDisposed && !Disposing)
                    MessageBox.Show(this, "Có lỗi khi xử lý đăng ký. Vui lòng kiểm tra lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && !Disposing) { btnLapPhieu.Enabled = true; btnHuy.Enabled = true; }
            }
        }
    }
}
