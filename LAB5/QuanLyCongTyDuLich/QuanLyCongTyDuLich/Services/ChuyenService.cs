using System;
using System.Data;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class ChuyenService
    {
        public DataTable LayChuyen(bool chiMo=false)
        {
            return Db.Query(@"SELECT c.MaChuyen,c.MaTour,t.TenTour,c.NgayDi,c.NgayVe,c.DiaDiemDon,c.TrangThai,t.DonGiaKhach,
c.MaChuyen+N' - '+t.TenTour+N' ('+CONVERT(NVARCHAR(10),c.NgayDi,103)+N')' AS HienThi
FROM dbo.ChuyenLe c JOIN dbo.Tour t ON t.MaTour=c.MaTour "
                +(chiMo?"WHERE c.TrangThai=N'Mở đăng ký' AND c.NgayDi>=CAST(GETDATE() AS DATE) ":"")+"ORDER BY c.NgayDi DESC;");
        }
        public void TaoChuyen(string ma,string tour,DateTime ngay,string noi)
        {
            ma=ServiceRules.Key(ma); tour=ServiceRules.Key(tour); noi=ServiceRules.Text(noi,"địa điểm đón",250);
            if(ngay.Date<DateTime.Today) throw new ArgumentException("Ngày đi không được ở quá khứ.");
            ServiceRules.Transaction((c,t)=>
            {
                object days=Db.Scalar(c,t,"SELECT SoNgay FROM dbo.Tour WHERE MaTour=@Tour AND DangMoBan=1;",Db.P("@Tour",SqlDbType.VarChar,tour,20));
                if(days==null) throw new ArgumentException("Tour không tồn tại hoặc chưa mở bán.");
                DateTime ve=ngay.Date.AddDays(Convert.ToInt32(days)-1);
                Db.Execute(c,t,"INSERT dbo.ChuyenLe(MaChuyen,MaTour,NgayDi,NgayVe,DiaDiemDon) VALUES(@Ma,@Tour,@Di,@Ve,@Noi);",Db.P("@Ma",SqlDbType.VarChar,ma,20),Db.P("@Tour",SqlDbType.VarChar,tour,20),Db.P("@Di",SqlDbType.Date,ngay.Date),Db.P("@Ve",SqlDbType.Date,ve),Db.P("@Noi",SqlDbType.NVarChar,noi,250));
            });
        }
        public void DongDangKy(string ma)
        {
            ma=ServiceRules.Key(ma);
            ServiceRules.Transaction((c,t)=>
            {
                int rows=Db.Execute(c,t,"UPDATE dbo.ChuyenLe SET TrangThai=N'Đóng đăng ký' WHERE MaChuyen=@Ma AND TrangThai=N'Mở đăng ký';",Db.P("@Ma",SqlDbType.VarChar,ma,20));
                if(rows!=1) throw new ArgumentException("Chuyến không tồn tại hoặc đã đóng đăng ký.");
            });
        }
    }
}
