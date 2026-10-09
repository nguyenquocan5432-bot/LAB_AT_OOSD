using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmDanhMuc : Form
    {
        readonly DanhMucService service=new DanhMucService(); readonly DataGridView[] grids=new DataGridView[4];
        public FrmDanhMuc()
        {
            Ui.Setup(this,"Danh mục"); var body=Ui.Body(this); var tabs=new TabControl { Dock=DockStyle.Fill }; body.Controls.Add(tabs);
            string[] titles={"Điểm bán vé","Phương tiện","Điểm tham quan","Hướng dẫn viên"};
            for(int i=0;i<titles.Length;i++) Build(tabs,titles[i],(LoaiDanhMuc)i);
            Shown+=(s,e)=>Ui.Safe(this,()=> { for(int i=0;i<grids.Length;i++) grids[i].DataSource=service.Lay((LoaiDanhMuc)i); });
        }
        void Build(TabControl tabs,string title,LoaiDanhMuc type)
        {
            var tab=new TabPage(title); tabs.TabPages.Add(tab); var edit=Ui.Editor(tab,145); var grid=Ui.Grid(); grids[(int)type]=grid; tab.Controls.Add(grid);
            var ma=Ui.Text("",20); var ten=Ui.Text("",type==LoaiDanhMuc.HuongDanVien?120:type==LoaiDanhMuc.PhuongTien?100:200);
            var dia=Ui.Text("",250); var phone=Ui.Text("",20); var content=Ui.Text("",4000); var meaning=Ui.Text("",4000); var salary=Ui.Money();
            Ui.Field(edit,"Mã",ma,0,8,170,70); Ui.Field(edit,"Tên",ten,280,8,620,70);
            if(type==LoaiDanhMuc.DiemBanVe || type==LoaiDanhMuc.DiemThamQuan) Ui.Field(edit,type==LoaiDanhMuc.DiemBanVe?"Địa chỉ":"Địa điểm",dia,0,48,520,90);
            if(type==LoaiDanhMuc.HuongDanVien || type==LoaiDanhMuc.DiemBanVe) Ui.Field(edit,"Điện thoại",phone,650,48,280,90);
            if(type==LoaiDanhMuc.HuongDanVien) Ui.Field(edit,"Lương cơ bản",salary,0,48,260,110);
            if(type==LoaiDanhMuc.DiemThamQuan) { Ui.Field(edit,"Nội dung",content,0,88,380,90); Ui.Field(edit,"Ý nghĩa",meaning,490,88,350,80); }
            Ui.Place(edit,Ui.Button("Thêm",()=> { service.Them(type,new DanhMucInput { Ma=ma.Text,Ten=ten.Text,DiaChi=dia.Text,DienThoai=phone.Text,NoiDung=content.Text,YNghia=meaning.Text,Luong=salary.Value }); grid.DataSource=service.Lay(type); Ui.Done(this); }),965,88,120,30);
        }
    }
}
