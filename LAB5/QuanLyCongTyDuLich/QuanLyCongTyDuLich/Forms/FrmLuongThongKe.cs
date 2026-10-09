using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmLuongThongKe : Form
    {
        readonly ThongKeService service=new ThongKeService(); readonly DataGridView wages=Ui.Grid(),stats=Ui.Grid();
        readonly NumericUpDown month=Ui.Number(DateTime.Today.Month,12),year=Ui.Number(DateTime.Today.Year,9998);
        readonly DateTimePicker from=Ui.Date(new DateTime(DateTime.Today.Year,DateTime.Today.Month,1)),to=Ui.Date();
        public FrmLuongThongKe()
        {
            Ui.Setup(this,"Lương - thống kê",1080,580); month.Minimum=1; year.Minimum=1900;
            var body=Ui.Body(this); var tabs=new TabControl { Dock=DockStyle.Fill }; body.Controls.Add(tabs);
            var a=new TabPage("Lương HDV"); tabs.TabPages.Add(a); var top=new Panel { Dock=DockStyle.Top,Height=60 };
            Ui.Field(top,"Tháng",month,8,12,85,55); Ui.Field(top,"Năm",year,180,12,100,50);
            Ui.Place(top,Ui.Button("Tính lương",()=>wages.DataSource=service.Luong((int)month.Value,(int)year.Value)),365,10,130,30);
            var note=Ui.Label("Lương = lương cơ bản + thù lao các tour kết thúc trong tháng."); note.ForeColor=SystemColors.GrayText; Ui.Place(top,note,525,12,510);
            a.Controls.Add(wages); a.Controls.Add(top);
            var b=new TabPage("Thống kê tổng hợp"); tabs.TabPages.Add(b); var filter=new Panel { Dock=DockStyle.Top,Height=60 };
            Ui.Field(filter,"Từ ngày",from,8,12,190,75); Ui.Field(filter,"Đến ngày",to,320,12,190,80);
            Ui.Place(filter,Ui.Button("Thống kê",()=>stats.DataSource=service.TongHop(from.Value,to.Value)),650,10,140,30); b.Controls.Add(stats); b.Controls.Add(filter);
            Shown+=(s,e)=>Ui.Safe(this,()=>wages.DataSource=service.Luong((int)month.Value,(int)year.Value));
        }
    }
}
