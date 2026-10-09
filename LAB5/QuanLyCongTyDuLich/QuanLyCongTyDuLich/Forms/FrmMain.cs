using System;
using System.Drawing;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmMain : Form
    {
        public FrmMain()
        {
            Ui.Setup(this, "Công ty du lịch Văn Hóa Việt", 720, 600);
            StartPosition = FormStartPosition.CenterScreen;
            var title = Ui.Label("CÔNG TY DU LỊCH VĂN HÓA VIỆT", true);
            title.Font = new Font("Segoe UI", 18F, FontStyle.Bold); title.ForeColor = Color.Navy;
            Ui.Place(this, title, 32, 20, 650, 58);
            var menu = new TableLayoutPanel { Location = new Point(32, 100), Size = new Size(656, 342),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, ColumnCount = 2, RowCount = 4 };
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 4; i++) menu.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            string[] names = { "Danh mục", "Tour - hành trình", "Lịch chuyến khách lẻ", "Đăng ký khách lẻ", "Đăng ký theo đoàn", "Phân công hướng dẫn viên", "Kết thúc tour - khảo sát", "Lương - thống kê" };
            Func<Form>[] forms = { () => new FrmDanhMuc(), () => new FrmTour(), () => new FrmChuyenLe(), () => new FrmDangKyLe(), () => new FrmDangKyDoan(), () => new FrmPhanCongHDV(), () => new FrmKetThucKhaoSat(), () => new FrmLuongThongKe() };
            for (int i = 0; i < names.Length; i++) {
                int index = i; var b = Ui.Button(names[i], () => { using (var f = forms[index]()) f.ShowDialog(this); });
                b.Dock = DockStyle.Fill; b.Margin = new Padding(10, 10, 10, 10); menu.Controls.Add(b, i % 2, i / 2);
            }
            Controls.Add(menu); Ui.Place(this, Ui.Button("Thoát", Close, 110), 305, 470, 110, 38);
        }
    }
}
