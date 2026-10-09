using System;
using System.Data;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class TourService
    {
        public DataTable LayTour(bool chiMoBan = false)
        {
            return Db.Query("SELECT MaTour,TenTour,SoNgay,SoDem,DonGiaKhach,MoTa,DangMoBan,MaTour+N' - '+TenTour AS HienThi FROM dbo.Tour "
                + (chiMoBan ? "WHERE DangMoBan=1 " : "") + "ORDER BY MaTour;");
        }
        public DataTable LayDiemDung(string ma)
        { return Db.Query("SELECT ThuTu,TenDiemDung,DoiPhuongTien,CoNoiAn,CoKhachSan,HangSaoKhachSan FROM dbo.DiemDung WHERE MaTour=@Ma ORDER BY ThuTu;", Db.P("@Ma",SqlDbType.VarChar,ma,20)); }
        public DataTable LayChang(string ma)
        { return Db.Query("SELECT c.ThuTuChang,c.MaPT,p.TenPT,c.GhiChu FROM dbo.ChangPhuongTien c JOIN dbo.PhuongTien p ON p.MaPT=c.MaPT WHERE c.MaTour=@Ma ORDER BY c.ThuTuChang;", Db.P("@Ma",SqlDbType.VarChar,ma,20)); }
        public DataTable LayThamQuan(string ma)
        { return Db.Query("SELECT t.ThuTu,d.MaDiemTQ,d.TenDiemTQ,d.DiaDiem,d.NoiDung,d.YNghia FROM dbo.TourDiemThamQuan t JOIN dbo.DiemThamQuan d ON d.MaDiemTQ=t.MaDiemTQ WHERE t.MaTour=@Ma ORDER BY t.ThuTu;", Db.P("@Ma",SqlDbType.VarChar,ma,20)); }

        public static KetQuaDangKy DuKien(int soNgay, decimal donGia, int soNguoi, DateTime ngayDi)
        {
            if (soNgay <= 0 || soNguoi < 0) throw new ArgumentException("Số ngày hoặc số người không hợp lệ.");
            ServiceRules.Money(donGia);
            decimal total = donGia * soNguoi;
            ServiceRules.Money(total);
            return new KetQuaDangKy { NgayKetThuc=ngayDi.Date.AddDays(soNgay-1), TongTien=total };
        }
        public void ThemTour(string ma, string ten, int ngay, int dem, decimal gia, string moTa)
        {
            ma=ServiceRules.Key(ma); ten=ServiceRules.Text(ten,"tên tour",200);
            if (ngay<=0 || dem<0) throw new ArgumentException("Số ngày phải > 0, số đêm phải >= 0.");
            ServiceRules.Money(gia);
            ServiceRules.Transaction((c,t)=>Db.Execute(c,t,@"INSERT dbo.Tour(MaTour,TenTour,SoNgay,SoDem,DonGiaKhach,MoTa,DangMoBan) VALUES(@Ma,@Ten,@Ngay,@Dem,@Gia,@MoTa,1);",
                Db.P("@Ma",SqlDbType.VarChar,ma,20),Db.P("@Ten",SqlDbType.NVarChar,ten,200),Db.P("@Ngay",SqlDbType.Int,ngay),Db.P("@Dem",SqlDbType.Int,dem),Db.P("@Gia",SqlDbType.Decimal,gia),Db.P("@MoTa",SqlDbType.NVarChar,moTa)));
        }
        public void ThemDiemDung(string tour,int stt,string ten,bool doi,bool an,bool hotel,int sao)
        {
            tour=ServiceRules.Key(tour); ten=ServiceRules.Text(ten,"tên điểm dừng",200);
            if(stt<=0 || (hotel && (sao<2 || sao>5))) throw new ArgumentException("Thứ tự phải > 0; khách sạn từ 2 đến 5 sao.");
            ServiceRules.Transaction((c,t)=>Db.Execute(c,t,@"INSERT dbo.DiemDung(MaTour,ThuTu,TenDiemDung,DoiPhuongTien,CoNoiAn,CoKhachSan,HangSaoKhachSan) VALUES(@Tour,@STT,@Ten,@Doi,@An,@Hotel,@Sao);",
                Db.P("@Tour",SqlDbType.VarChar,tour,20),Db.P("@STT",SqlDbType.Int,stt),Db.P("@Ten",SqlDbType.NVarChar,ten,200),Db.P("@Doi",SqlDbType.Bit,doi),Db.P("@An",SqlDbType.Bit,an),Db.P("@Hotel",SqlDbType.Bit,hotel),Db.P("@Sao",SqlDbType.TinyInt,hotel?(object)sao:null)));
        }
        public void GanPhuongTien(string tour,int stt,string pt,string ghiChu)
        {
            tour=ServiceRules.Key(tour); pt=ServiceRules.Key(pt);
            if(stt<=0) throw new ArgumentException("Thứ tự chặng phải > 0.");
            if((ghiChu??"").Length>300) throw new ArgumentException("Ghi chú tối đa 300 ký tự.");
            ServiceRules.Transaction((c,t)=>Db.Execute(c,t,"INSERT dbo.ChangPhuongTien(MaTour,ThuTuChang,MaPT,GhiChu) VALUES(@Tour,@STT,@PT,@Ghi);",
                Db.P("@Tour",SqlDbType.VarChar,tour,20),Db.P("@STT",SqlDbType.Int,stt),Db.P("@PT",SqlDbType.VarChar,pt,20),Db.P("@Ghi",SqlDbType.NVarChar,ghiChu,300)));
        }
        public void GanDiemThamQuan(string tour,string diem,int stt)
        {
            tour=ServiceRules.Key(tour); diem=ServiceRules.Key(diem);
            if(stt<=0) throw new ArgumentException("Thứ tự tham quan phải > 0.");
            ServiceRules.Transaction((c,t)=>Db.Execute(c,t,"INSERT dbo.TourDiemThamQuan(MaTour,MaDiemTQ,ThuTu) VALUES(@Tour,@Diem,@STT);",
                Db.P("@Tour",SqlDbType.VarChar,tour,20),Db.P("@Diem",SqlDbType.VarChar,diem,20),Db.P("@STT",SqlDbType.Int,stt)));
        }
    }
}
