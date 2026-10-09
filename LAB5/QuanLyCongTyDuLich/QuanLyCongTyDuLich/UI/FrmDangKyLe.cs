using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmDangKyLe : Form
    {
        readonly DangKyLeService service=new DangKyLeService(); readonly ChuyenService trips=new ChuyenService();
        readonly TextBox ma=Ui.Text(Ui.Id("L"),20),ten=Ui.Text("",120),phone=Ui.Text("",20);
        readonly ComboBox chuyen=Ui.Combo(),diem=Ui.Combo(); readonly NumericUpDown nguoi=Ui.Number(1,1000);
        readonly Label tien=Ui.Label("",true); readonly DataGridView grid=Ui.Grid(); bool loading;
        public FrmDangKyLe()
        {
            Ui.Setup(this,"Đăng ký khách lẻ"); var body=Ui.Body(this); var top=new Panel { Dock=DockStyle.Top,Height=118 };
            Ui.Field(top,"Số đăng ký",ma,0,0,230); Ui.Field(top,"Chuyến",chuyen,365,0,610,70);
            Ui.Field(top,"Điểm bán vé",diem,0,36,230); Ui.Field(top,"Người ĐK",ten,365,36,245,70); Ui.Field(top,"Điện thoại",phone,700,36,280,85);
            Ui.Field(top,"Số người",nguoi,0,72,100); tien.ForeColor=Color.Maroon;
            Ui.Field(top,"Thành tiền",tien,260,72,250,90); Ui.Place(top,Ui.Button("Đăng ký và thanh toán vé",Save,240),690,72,240,30);
            body.Controls.Add(grid); body.Controls.Add(top);
            chuyen.SelectedIndexChanged+=(s,e)=>Preview(); nguoi.ValueChanged+=(s,e)=>Preview();
            Shown+=(s,e)=>Ui.Safe(this,()=> { loading=true; try { Ui.Bind(chuyen,trips.LayChuyen(true),"MaChuyen","HienThi"); Ui.Bind(diem,new DanhMucService().Lay(LoaiDanhMuc.DiemBanVe),"MaDiemBan","TenDiemBan"); grid.DataSource=service.LayDangKy(); } finally { loading=false; } Preview(); });
        }
        void Preview() { if(loading)return; var r=Ui.Row(chuyen); tien.Text=r==null?"":Ui.Amount(Convert.ToDecimal(r["DonGiaKhach"])*nguoi.Value); }
        void Save() { var total=service.DangKy(ma.Text,Ui.Key(chuyen),Ui.Key(diem),ten.Text,phone.Text,(int)nguoi.Value); grid.DataSource=service.LayDangKy(); ma.Text=Ui.Id("L"); Ui.Done(this,"Đã đăng ký và thanh toán "+Ui.Amount(total)+" đồng."); }
    }
}
