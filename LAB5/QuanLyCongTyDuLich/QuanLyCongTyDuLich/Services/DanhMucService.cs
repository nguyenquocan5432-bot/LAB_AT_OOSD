using System;
using System.Data;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public enum LoaiDanhMuc { DiemBanVe, PhuongTien, DiemThamQuan, HuongDanVien }
    public sealed class DanhMucInput
    {
        public string Ma,Ten,DiaChi,DienThoai,NoiDung,YNghia;
        public decimal Luong;
    }
    public sealed class DanhMucService
    {
        public DataTable Lay(LoaiDanhMuc loai)
        {
            switch(loai)
            {
                case LoaiDanhMuc.DiemBanVe: return Db.Query("SELECT MaDiemBan,TenDiemBan,DiaChi,DienThoai FROM dbo.DiemBanVe ORDER BY MaDiemBan;");
                case LoaiDanhMuc.PhuongTien: return Db.Query("SELECT MaPT,TenPT FROM dbo.PhuongTien ORDER BY MaPT;");
                case LoaiDanhMuc.DiemThamQuan: return Db.Query("SELECT MaDiemTQ,TenDiemTQ,DiaDiem,NoiDung,YNghia FROM dbo.DiemThamQuan ORDER BY MaDiemTQ;");
                case LoaiDanhMuc.HuongDanVien: return Db.Query("SELECT MaHDV,HoTen,DienThoai,LuongCoBan FROM dbo.HuongDanVien ORDER BY MaHDV;");
                default: throw new ArgumentException("Loại danh mục không hợp lệ.");
            }
        }
        public void Them(LoaiDanhMuc loai,DanhMucInput x)
        {
            if(x==null) throw new ArgumentNullException("x");
            x.Ma=ServiceRules.Key(x.Ma); x.Ten=ServiceRules.Text(x.Ten,"tên",loai==LoaiDanhMuc.HuongDanVien?120:loai==LoaiDanhMuc.PhuongTien?100:200);
            if((x.DienThoai??"").Length>20) throw new ArgumentException("Điện thoại tối đa 20 ký tự.");
            if(loai==LoaiDanhMuc.DiemBanVe || loai==LoaiDanhMuc.DiemThamQuan) x.DiaChi=ServiceRules.Text(x.DiaChi,"địa chỉ/địa điểm",250);
            if(loai==LoaiDanhMuc.HuongDanVien) ServiceRules.Money(x.Luong);
            string sql;
            switch(loai)
            {
                case LoaiDanhMuc.DiemBanVe: sql="INSERT dbo.DiemBanVe(MaDiemBan,TenDiemBan,DiaChi,DienThoai) VALUES(@Ma,@Ten,@Dia,@DT);"; break;
                case LoaiDanhMuc.PhuongTien: sql="INSERT dbo.PhuongTien(MaPT,TenPT) VALUES(@Ma,@Ten);"; break;
                case LoaiDanhMuc.DiemThamQuan: sql="INSERT dbo.DiemThamQuan(MaDiemTQ,TenDiemTQ,DiaDiem,NoiDung,YNghia) VALUES(@Ma,@Ten,@Dia,@NoiDung,@YNghia);"; break;
                case LoaiDanhMuc.HuongDanVien: sql="INSERT dbo.HuongDanVien(MaHDV,HoTen,DienThoai,LuongCoBan) VALUES(@Ma,@Ten,@DT,@Luong);"; break;
                default: throw new ArgumentException("Loại danh mục không hợp lệ.");
            }
            ServiceRules.Transaction((c,t)=>Db.Execute(c,t,sql,Db.P("@Ma",SqlDbType.VarChar,x.Ma,20),Db.P("@Ten",SqlDbType.NVarChar,x.Ten,200),Db.P("@Dia",SqlDbType.NVarChar,x.DiaChi,250),Db.P("@DT",SqlDbType.VarChar,x.DienThoai,20),Db.P("@NoiDung",SqlDbType.NVarChar,x.NoiDung),Db.P("@YNghia",SqlDbType.NVarChar,x.YNghia),Db.P("@Luong",SqlDbType.Decimal,x.Luong)));
        }
    }
}
