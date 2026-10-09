using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmChuyenLe : Form
    {
        readonly ChuyenService service = new ChuyenService(); readonly TourService tours = new TourService();
        readonly TextBox ma = Ui.Text(Ui.Id("C"), 20), noi = Ui.Text("TP.HCM", 250);
        readonly ComboBox tour = Ui.Combo(); readonly DateTimePicker ngay = Ui.Date(DateTime.Today.AddDays(10));
        readonly Label ve = Ui.Label("", true); readonly DataGridView grid = Ui.Grid(); bool loading;
        public FrmChuyenLe()
        {
            Ui.Setup(this, "Lịch chuyến khách lẻ"); var body = Ui.Body(this);
            var top = new Panel { Dock = DockStyle.Top, Height = 116 };
            Ui.Field(top, "Mã chuyến", ma, 0, 0); Ui.Field(top, "Tour", tour, 370, 0, 540, 75);
            Ui.Field(top, "Ngày đi", ngay, 0, 36); Ui.Field(top, "Ngày về", ve, 370, 36, 300, 75);
            Ui.Field(top, "Địa điểm đón", noi, 0, 72, 450);
            Ui.Place(top, Ui.Button("Tạo chuyến", Save), 580, 72, 130, 30);
            Ui.Place(top, Ui.Button("Đóng đăng ký", () => { service.DongDangKy(Ui.Cell(grid, "MaChuyen")); RefreshGrid(); }), 730, 72, 150, 30);
            body.Controls.Add(grid); body.Controls.Add(top);
            tour.SelectedIndexChanged += (s,e) => Preview(); ngay.ValueChanged += (s,e) => Preview();
            Shown += (s,e) => Ui.Safe(this, () => { loading = true; try { Ui.Bind(tour, tours.LayTour(true), "MaTour", "HienThi"); RefreshGrid(); } finally { loading = false; } Preview(); });
        }
        void Preview() { if (loading) return; var r=Ui.Row(tour); ve.Text=r==null?"":TourService.DuKien(Convert.ToInt32(r["SoNgay"]),0,0,ngay.Value).NgayKetThuc.ToString("dd/MM/yyyy"); }
        void RefreshGrid() { grid.DataSource = service.LayChuyen(); grid.Columns["DonGiaKhach"].Visible=false; }
        void Save() { service.TaoChuyen(ma.Text, Ui.Key(tour), ngay.Value, noi.Text); ma.Text=Ui.Id("C"); RefreshGrid(); Ui.Done(this,"Đã tạo chuyến khách lẻ."); }
    }
}
