using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public sealed class FrmPhanCongHDV : Form
    {
        readonly PhanCongService service=new PhanCongService(); readonly TextBox ma=Ui.Text(Ui.Id("PC"),20);
        readonly ComboBox hdv=Ui.Combo(),loai=Ui.Combo(),doiTuong=Ui.Combo(); readonly NumericUpDown thuLao=Ui.Money(); readonly DataGridView grid=Ui.Grid(); bool loading;
        public FrmPhanCongHDV()
        {
            Ui.Setup(this,"Phân công hướng dẫn viên"); var body=Ui.Body(this); var top=new Panel { Dock=DockStyle.Top,Height=118 };
            Ui.Field(top,"Mã PC",ma,0,0,230); Ui.Field(top,"HDV",hdv,365,0,610,70);
            Ui.Field(top,"Loại",loai,0,36,230); Ui.Field(top,"Chuyến/đoàn",doiTuong,365,36,610,90);
            Ui.Field(top,"Thù lao",thuLao,0,72,230);
            var note=Ui.Label("Ngày bắt đầu/kết thúc tự lấy theo chuyến hoặc đoàn."); note.ForeColor=System.Drawing.SystemColors.GrayText;
            Ui.Place(top,note,365,72,475); Ui.Place(top,Ui.Button("Phân công",Save),880,72,140,30);
            body.Controls.Add(grid); body.Controls.Add(top); loai.Items.AddRange(new object[]{"LE","DOAN"}); loai.SelectedIndex=0;
            loai.SelectedIndexChanged+=(s,e)=> { if(!loading)Ui.Safe(this,LoadTargets); };
            Shown+=(s,e)=>Ui.Safe(this,()=> { loading=true; try { Ui.Bind(hdv,service.LayHDV(),"MaHDV","Ten"); LoadTargets(); grid.DataSource=service.LayPhanCong(); } finally { loading=false; } });
        }
        void LoadTargets() { Ui.Bind(doiTuong,service.LayDoiTuong(loai.SelectedIndex==1),"Ma","Ten"); }
        void Save() { service.PhanCong(ma.Text,Ui.Key(hdv),loai.SelectedIndex==1,Ui.Key(doiTuong),thuLao.Value); grid.DataSource=service.LayPhanCong(); ma.Text=Ui.Id("PC"); Ui.Done(this,"Đã phân công hướng dẫn viên."); }
    }
}
