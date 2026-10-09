using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmTour : Form
    {
        readonly TourService service=new TourService(); readonly DanhMucService catalog=new DanhMucService();
        readonly ComboBox chosen=Ui.Combo(),vehicle=Ui.Combo(),attraction=Ui.Combo();
        readonly DataGridView tours=Ui.Grid(),stops=Ui.Grid(),legs=Ui.Grid(),visits=Ui.Grid();
        readonly TextBox ma=Ui.Text("",20),ten=Ui.Text(),moTa=Ui.Text("",4000),stop=Ui.Text(),ghi=Ui.Text("",300);
        readonly NumericUpDown days=Ui.Number(3,3650),nights=Ui.Number(2,3650),price=Ui.Money(1500000),stt=Ui.Number(1),stars=Ui.Number(3,5),legNo=Ui.Number(1),visitNo=Ui.Number(1);
        readonly CheckBox hotel=new CheckBox { Text="Có khách sạn",Checked=true },eat=new CheckBox { Text="Có nơi ăn",Checked=true },change=new CheckBox { Text="Đổi phương tiện" };
        bool loading;
        public FrmTour()
        {
            Ui.Setup(this,"Tour - hành trình"); var body=Ui.Body(this); var top=new Panel { Dock=DockStyle.Top,Height=44 }; Ui.Field(top,"Tour đang chọn",chosen,0,3,680,125);
            var tabs=new TabControl { Dock=DockStyle.Fill }; body.Controls.Add(tabs); body.Controls.Add(top);
            var a=new TabPage("Tour"); tabs.TabPages.Add(a); var edit=Ui.Editor(a,102);
            Ui.Field(edit,"Mã tour",ma,0,6,160,75); Ui.Field(edit,"Tên tour",ten,255,6,375,80); Ui.Field(edit,"Số ngày",days,735,6,105,75); Ui.Field(edit,"Số đêm",nights,930,6,100,65);
            Ui.Field(edit,"Đơn giá/khách",price,0,44,160,95); Ui.Field(edit,"Mô tả",moTa,285,44,510,60);
            Ui.Place(edit,Ui.Button("Thêm tour",()=> { service.ThemTour(ma.Text,ten.Text,(int)days.Value,(int)nights.Value,price.Value,moTa.Text); Reload(); Ui.Done(this); }),900,44,140,30); a.Controls.Add(tours);
            var b=new TabPage("Điểm dừng"); tabs.TabPages.Add(b); var be=Ui.Editor(b,106);
            Ui.Field(be,"Thứ tự",stt,0,6,75,75); Ui.Field(be,"Tên điểm dừng",stop,180,6,480,115);
            Ui.Place(be,change,10,45,160); Ui.Place(be,eat,185,45,130); Ui.Place(be,hotel,330,45,140); Ui.Field(be,"Hạng sao",stars,495,43,75,75);
            Ui.Place(be,Ui.Button("Thêm điểm dừng",()=> { service.ThemDiemDung(Ui.Key(chosen),(int)stt.Value,stop.Text,change.Checked,eat.Checked,hotel.Checked,(int)stars.Value); LoadDetails(); Ui.Done(this); },180),850,43,180,30); b.Controls.Add(stops);
            hotel.CheckedChanged+=(s,e)=>stars.Enabled=hotel.Checked;
            var c=new TabPage("Phương tiện theo chặng"); tabs.TabPages.Add(c); var ce=Ui.Editor(c,100);
            Ui.Field(ce,"Thứ tự chặng",legNo,0,6,75,115); Ui.Field(ce,"Phương tiện",vehicle,225,6,540,100); Ui.Field(ce,"Ghi chú",ghi,0,45,700,80);
            Ui.Place(ce,Ui.Button("Gắn phương tiện",()=> { service.GanPhuongTien(Ui.Key(chosen),(int)legNo.Value,Ui.Key(vehicle),ghi.Text); LoadDetails(); Ui.Done(this); },180),850,43,180,30); c.Controls.Add(legs);
            var d=new TabPage("Điểm tham quan"); tabs.TabPages.Add(d); var de=Ui.Editor(d,70);
            Ui.Field(de,"Thứ tự",visitNo,0,12,75,75); Ui.Field(de,"Điểm tham quan",attraction,180,12,525,125);
            Ui.Place(de,Ui.Button("Gắn điểm tham quan",()=> { service.GanDiemThamQuan(Ui.Key(chosen),Ui.Key(attraction),(int)visitNo.Value); LoadDetails(); Ui.Done(this); },180),850,10,180,30); d.Controls.Add(visits);
            chosen.SelectedIndexChanged+=(s,e)=> { if(!loading) Ui.Safe(this,LoadDetails); };
            Shown+=(s,e)=>Ui.Safe(this,()=> { Reload(); Ui.Bind(vehicle,catalog.Lay(LoaiDanhMuc.PhuongTien),"MaPT","TenPT"); Ui.Bind(attraction,catalog.Lay(LoaiDanhMuc.DiemThamQuan),"MaDiemTQ","TenDiemTQ"); });
        }
        void Reload()
        {
            string old=Ui.Key(chosen); loading=true;
            try { var data=service.LayTour(); tours.DataSource=data; Ui.Bind(chosen,data.Copy(),"MaTour","HienThi"); if(old!="")chosen.SelectedValue=old; }
            finally { loading=false; } LoadDetails();
        }
        void LoadDetails() { string key=Ui.Key(chosen); stops.DataSource=service.LayDiemDung(key); legs.DataSource=service.LayChang(key); visits.DataSource=service.LayThamQuan(key); }
    }
}
