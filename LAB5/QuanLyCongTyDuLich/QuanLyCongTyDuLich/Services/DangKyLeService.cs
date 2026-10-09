using System;
using System.Data;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class DangKyLeService
    {
        public DataTable LayDangKy()
        {
            return Db.Query(@"SELECT d.SoPhieu AS SoDKLe,d.MaChuyen,t.TenTour,c.NgayDi,c.NgayVe,b.TenDiemBan,d.TenNguoiDangKy,d.DienThoai,d.SoNguoi,d.ThanhTien,d.TrangThai
FROM dbo.DangKyLe d JOIN dbo.ChuyenLe c ON c.MaChuyen=d.MaChuyen JOIN dbo.Tour t ON t.MaTour=c.MaTour JOIN dbo.DiemBanVe b ON b.MaDiemBan=d.MaDiemBan ORDER BY d.NgayDangKy DESC;");
        }
        public decimal DangKy(string phieu,string chuyen,string diem,string ten,string dt,int soNguoi)
        {
            phieu=ServiceRules.Key(phieu); chuyen=ServiceRules.Key(chuyen); diem=ServiceRules.Key(diem);
            ten=ServiceRules.Text(ten,"người đăng ký",120); dt=ServiceRules.Text(dt,"điện thoại",20);
            if(soNguoi<1 || soNguoi>=12) throw new ArgumentException("Khách lẻ phải từ 1 đến 11 người; đúng 12 người chưa được quy định.");
            decimal total=0;
            ServiceRules.Transaction((c,t)=>
            {
                object price=Db.Scalar(c,t,@"SELECT t.DonGiaKhach FROM dbo.ChuyenLe c JOIN dbo.Tour t ON t.MaTour=c.MaTour WHERE c.MaChuyen=@Ma AND c.TrangThai=N'Mở đăng ký' AND c.NgayDi>=CAST(GETDATE() AS DATE);",Db.P("@Ma",SqlDbType.VarChar,chuyen,20));
                if(price==null) throw new ArgumentException("Chuyến không còn mở đăng ký.");
                total=Convert.ToDecimal(price)*soNguoi; ServiceRules.Money(total);
                Db.Execute(c,t,@"INSERT dbo.DangKyLe(SoPhieu,MaChuyen,MaDiemBan,TenNguoiDangKy,DienThoai,SoNguoi,ThanhTien,DaThanhToan) VALUES(@Phieu,@Chuyen,@Diem,@Ten,@DT,@So,@Tien,1);",
                    Db.P("@Phieu",SqlDbType.VarChar,phieu,20),Db.P("@Chuyen",SqlDbType.VarChar,chuyen,20),Db.P("@Diem",SqlDbType.VarChar,diem,20),Db.P("@Ten",SqlDbType.NVarChar,ten,120),Db.P("@DT",SqlDbType.VarChar,dt,20),Db.P("@So",SqlDbType.Int,soNguoi),Db.P("@Tien",SqlDbType.Decimal,total));
            });
            return total;
        }
    }
}
