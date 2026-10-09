using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmKetThucKhaoSat : Form
    {
        readonly KetThucService service=new KetThucService(); readonly DataGridView debts=Ui.Grid(),surveys=Ui.Grid();
        readonly TextBox soTT=Ui.Text(Ui.Id("TT"),20),phieu=Ui.Text("",20),note=Ui.Text("",300),ks=Ui.Text(Ui.Id("KS"),20),selected=Ui.Text("",20),feedback=Ui.Text("",1500);
        readonly NumericUpDown money=Ui.Money(),rating=Ui.Number(5,5);
        readonly DateTimePicker paidDate=Ui.Date(),sentDate=Ui.Date(),replyDate=Ui.Date();
        readonly ComboBox type=Ui.Combo(),registration=Ui.Combo(); bool loading;
        public FrmKetThucKhaoSat()
        {
            Ui.Setup(this,"Kết thúc tour - khảo sát"); var body=Ui.Body(this); var tabs=new TabControl { Dock=DockStyle.Fill }; body.Controls.Add(tabs);
            var a=new TabPage("Thanh toán sau tour (đoàn)"); tabs.TabPages.Add(a); var edit=Ui.Editor(a,130); phieu.ReadOnly=true;
            Ui.Field(edit,"Số TT",soTT,0,8,220,75); Ui.Field(edit,"Phiếu đoàn",phieu,340,8,230,85); Ui.Field(edit,"Ngày TT",paidDate,725,8,210,75);
            Ui.Field(edit,"Số tiền",money,0,48,220,75); Ui.Field(edit,"Ghi chú",note,340,48,535,85);
            Ui.Place(edit,Ui.Button("Ghi nhận thanh toán",Pay,210),425,88,210,30); a.Controls.Add(debts);
            debts.SelectionChanged+=(s,e)=> { if(debts.CurrentRow==null)return; phieu.Text=Ui.Cell(debts,"SoDKDoan"); money.Value=Math.Max(0,Convert.ToDecimal(debts.CurrentRow.Cells["ConLai"].Value)); };
            var b=new TabPage("Khảo sát khách hàng"); tabs.TabPages.Add(b); var se=Ui.Editor(b,170); b.Controls.Add(surveys);
            type.Items.AddRange(new object[]{"LE","DOAN"}); type.SelectedIndex=0; selected.ReadOnly=true; rating.Minimum=1;
            Ui.Field(se,"Mã khảo sát",ks,0,8,170,100); Ui.Field(se,"Loại khách",type,300,8,120,85); Ui.Field(se,"Ngày gửi",sentDate,555,8,200,85);
            Ui.Field(se,"Phiếu đăng ký",registration,0,48,730,110); Ui.Place(se,Ui.Button("Gửi phiếu khảo sát",Send,190),880,46,190,30);
            Ui.Field(se,"Phiếu phản hồi",selected,0,88,170,110); Ui.Field(se,"Ngày phản hồi",replyDate,320,88,200,115); Ui.Field(se,"Điểm (1–5)",rating,680,88,80,90);
            Ui.Field(se,"Góp ý",feedback,0,128,730,110); Ui.Place(se,Ui.Button("Ghi nhận góp ý",Reply,190),880,126,190,30);
            type.SelectedIndexChanged+=(s,e)=> { if(!loading)Ui.Safe(this,LoadEligible); };
            surveys.SelectionChanged+=(s,e)=>selected.Text=Ui.Cell(surveys,"MaKhaoSat");
            Shown+=(s,e)=>Ui.Safe(this,()=> { loading=true; try { Reload(); } finally { loading=false; } });
        }
        void LoadEligible() { Ui.Bind(registration,service.LayChoKhaoSat(type.SelectedIndex==1),"Ma","Ten"); }
        void Reload() { debts.DataSource=service.LayCongNo(); surveys.DataSource=service.LayKhaoSat(); LoadEligible(); }
        void Pay() { service.ThanhToan(soTT.Text,phieu.Text,paidDate.Value,money.Value,note.Text); soTT.Text=Ui.Id("TT"); Reload(); Ui.Done(this,"Đã ghi nhận thanh toán sau tour."); }
        void Send() { service.GuiKhaoSat(ks.Text,type.SelectedIndex==1,Ui.Key(registration),sentDate.Value); ks.Text=Ui.Id("KS"); Reload(); Ui.Done(this,"Đã lưu phiếu khảo sát gửi khách hàng."); }
        void Reply() { service.GhiPhanHoi(selected.Text,replyDate.Value,(int)rating.Value,feedback.Text); surveys.DataSource=service.LayKhaoSat(); Ui.Done(this,"Đã ghi nhận phản hồi của khách hàng."); }
    }
}
