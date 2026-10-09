using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;
namespace QuanLyCongTyDuLich.UI
{
    public partial class FrmDangKyDoan
    {
        private TextBox txtSoPhieu, txtMaDoan, txtTenDoan, txtDiaChi, txtDienThoai, txtNguoiDaiDien, txtDiaDiemDon;
        private ComboBox cboTour;
        private NumericUpDown numSoNguoi, numTienCoc;
        private DateTimePicker dtpNgayDi;
        private CheckBox chkMuaBaoHiem;
        private DataGridView dgvThanhVien, dgvPhieu;
        private Button btnLapPhieu, btnHuy;
        private Label lblKetThuc, lblTong;
        private bool loading;
        private void InitializeComponent()
        {
            Ui.Setup(this,"Đăng ký theo đoàn",1220,710);
            txtSoPhieu=Ui.Text(Ui.Id("DK"),20); txtMaDoan=Ui.Text("D001",20);
            txtTenDoan=Ui.Text("Công ty An Phú",200); txtDiaChi=Ui.Text("10 Nguyễn Huệ, TP.HCM",250);
            txtDienThoai=Ui.Text("0900000000",20); txtNguoiDaiDien=Ui.Text("Nguyễn Văn An",120); txtDiaDiemDon=Ui.Text("TP.HCM",250);
            cboTour=Ui.Combo(); numSoNguoi=Ui.Number(15,int.MaxValue); numTienCoc=Ui.Money(10000000);
            dtpNgayDi=Ui.Date(DateTime.Today.AddDays(10)); chkMuaBaoHiem=new CheckBox { Text="Mua bảo hiểm" };
            lblKetThuc=Ui.Label("",true); lblTong=Ui.Label("",true); lblTong.ForeColor=Color.Maroon;
            btnLapPhieu=Ui.Button("Lập phiếu đăng ký",null,170); btnHuy=Ui.Button("Hủy phiếu (mất cọc)",Cancel,170);
            var body=Ui.Body(this); var root=new TableLayoutPanel { Dock=DockStyle.Fill,ColumnCount=1,RowCount=3 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute,210)); root.RowStyles.Add(new RowStyle(SizeType.Absolute,208)); root.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            var memberPanel=new Panel { Dock=DockStyle.Fill }; var actions=new Panel { Dock=DockStyle.Right,Width=185 };
            Ui.Place(actions,btnLapPhieu,10,40,170,34); Ui.Place(actions,btnHuy,10,82,170,34);
            dgvThanhVien=Ui.Grid(true); dgvThanhVien.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            dgvThanhVien.Columns.Add("HoTen","HoTen"); dgvThanhVien.Columns.Add("NgaySinh","NgaySinh"); dgvThanhVien.Columns.Add("SoGiayTo","SoGiayTo");
            dgvThanhVien.Enabled=false;
            var hint=Ui.Label("Danh sách người cùng đi (bắt buộc nếu mua bảo hiểm; ngày sinh: dd/MM/yyyy)"); hint.Dock=DockStyle.Bottom; hint.Height=30;
            memberPanel.Controls.Add(dgvThanhVien); memberPanel.Controls.Add(hint); memberPanel.Controls.Add(actions);
            root.Controls.Add(memberPanel,0,0);
            var groups=new TableLayoutPanel { Dock=DockStyle.Fill,ColumnCount=2,RowCount=1 };
            groups.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50)); groups.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            var info=new GroupBox { Text="Thông tin đoàn khách",Dock=DockStyle.Fill }; var registration=new GroupBox { Text="Đăng ký tour",Dock=DockStyle.Fill };
            var a=Fields(2,5); AddField(a,0,0,"Mã đoàn",txtMaDoan); AddField(a,1,0,"Cơ quan/gia đình",txtTenDoan);
            AddField(a,2,0,"Địa chỉ",txtDiaChi); AddField(a,3,0,"Điện thoại",txtDienThoai); AddField(a,4,0,"Người đại diện",txtNguoiDaiDien); info.Controls.Add(a);
            var b=Fields(4,5); AddField(b,0,0,"Số phiếu",txtSoPhieu); AddField(b,0,2,"Tour",cboTour);
            AddField(b,1,0,"Ngày đi",dtpNgayDi); AddField(b,1,2,"Số người",numSoNguoi);
            AddField(b,2,0,"Địa điểm đón",txtDiaDiemDon); b.SetColumnSpan(txtDiaDiemDon,3);
            AddField(b,3,0,"Tiền cọc",numTienCoc); chkMuaBaoHiem.Dock=DockStyle.Fill; b.Controls.Add(chkMuaBaoHiem,3,3);
            AddField(b,4,0,"Kết thúc DK",lblKetThuc); AddField(b,4,2,"Tổng dự kiến",lblTong); registration.Controls.Add(b);
            groups.Controls.Add(info,0,0); groups.Controls.Add(registration,1,0); root.Controls.Add(groups,0,1);
            var list=new Panel { Dock=DockStyle.Fill }; var caption=Ui.Label("Các phiếu đăng ký đoàn:"); caption.Dock=DockStyle.Top; caption.Height=28;
            dgvPhieu=Ui.Grid(); list.Controls.Add(dgvPhieu); list.Controls.Add(caption); root.Controls.Add(list,0,2); body.Controls.Add(root);
            chkMuaBaoHiem.CheckedChanged+=(s,e)=>dgvThanhVien.Enabled=chkMuaBaoHiem.Checked;
            cboTour.SelectedIndexChanged+=(s,e)=>Preview(); numSoNguoi.ValueChanged+=(s,e)=>Preview(); dtpNgayDi.ValueChanged+=(s,e)=>Preview();
            Shown+=(s,e)=>Ui.Safe(this,()=> { loading=true; try { Ui.Bind(cboTour,new TourService().LayTour(true),"MaTour","HienThi"); RefreshList(); } finally { loading=false; } Preview(); });
        }
        static TableLayoutPanel Fields(int columns,int rows)
        {
            var t=new TableLayoutPanel { Dock=DockStyle.Fill,ColumnCount=columns,RowCount=rows,Padding=new Padding(8,8,8,4) };
            for(int i=0;i<columns;i++) t.ColumnStyles.Add(i%2==0?new ColumnStyle(SizeType.Absolute,108):new ColumnStyle(SizeType.Percent,100F/(columns/2)));
            for(int i=0;i<rows;i++) t.RowStyles.Add(new RowStyle(SizeType.Percent,100F/rows)); return t;
        }
        static void AddField(TableLayoutPanel p,int row,int col,string title,Control input)
        {
            var label=Ui.Label(title); label.Dock=DockStyle.Fill; p.Controls.Add(label,col,row);
            input.Dock=DockStyle.Fill; input.Margin=new Padding(3,5,8,3); p.Controls.Add(input,col+1,row);
        }
        void RefreshList() { dgvPhieu.DataSource=service.LayPhieu(); dgvPhieu.Columns["MaDoan"].Visible=false; }
        void Preview()
        {
            if(loading)return; var row=Ui.Row(cboTour);
            if(row==null) { lblKetThuc.Text=""; lblTong.Text=""; return; }
            try { var p=TourService.DuKien(Convert.ToInt32(row["SoNgay"]),Convert.ToDecimal(row["DonGiaKhach"]),(int)numSoNguoi.Value,dtpNgayDi.Value); lblKetThuc.Text=p.NgayKetThuc.ToString("dd/MM/yyyy"); lblTong.Text=Ui.Amount(p.TongTien); }
            catch(ArgumentException) { lblTong.Text="Vượt giới hạn"; }
        }
        void Cancel()
        {
            string id=Ui.Cell(dgvPhieu,"SoDKDoan"); if(string.IsNullOrEmpty(id))throw new ArgumentException("Chọn phiếu cần hủy trong bảng.");
            if(MessageBox.Show(this,"Hủy phiếu "+id+"? Đoàn mất tiền cọc.","Hủy đăng ký",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            service.HuyPhieu(id); RefreshList(); Ui.Done(this,"Đã hủy phiếu và giữ tiền cọc.");
        }
    }
}
