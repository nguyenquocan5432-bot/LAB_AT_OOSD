using System;
using System.Data;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class ThongKeService
    {
        public DataTable Luong(int thang,int nam)
        {
            if(thang<1 || thang>12 || nam<1900 || nam>9998) throw new ArgumentException("Tháng/năm không hợp lệ.");
            DateTime tu=new DateTime(nam,thang,1), den=tu.AddMonths(1);
            return Db.Query(@"SELECT h.MaHDV,h.HoTen,h.LuongCoBan,COUNT(p.MaPC) AS SoTour,ISNULL(SUM(p.ThuLaoTour),0) AS LuongTheoTour,
h.LuongCoBan+ISNULL(SUM(p.ThuLaoTour),0) AS TongLuong FROM dbo.HuongDanVien h
LEFT JOIN dbo.PhanCongHDV p ON p.MaHDV=h.MaHDV AND p.NgayKetThuc>=@Tu AND p.NgayKetThuc<@Den
GROUP BY h.MaHDV,h.HoTen,h.LuongCoBan ORDER BY h.MaHDV;",Db.P("@Tu",SqlDbType.Date,tu),Db.P("@Den",SqlDbType.Date,den));
        }
        public DataTable TongHop(DateTime tu,DateTime den)
        {
            if(den.Date<tu.Date) throw new ArgumentException("Ngày đến phải >= ngày từ.");
            return Db.Query(@"SELECT N'Đăng ký khách lẻ' AS ChiSo,COUNT(*) AS SoLuong,ISNULL(SUM(ThanhTien),0) AS GiaTri FROM dbo.DangKyLe WHERE CAST(NgayDangKy AS DATE) BETWEEN @Tu AND @Den
UNION ALL SELECT N'Đăng ký đoàn',COUNT(*),ISNULL(SUM(TongTienDuKien),0) FROM dbo.DangKyDoan WHERE CAST(NgayDangKy AS DATE) BETWEEN @Tu AND @Den AND TrangThai<>N'Hủy - mất cọc'
UNION ALL SELECT N'Thanh toán sau tour',COUNT(*),ISNULL(SUM(SoTien),0) FROM dbo.ThanhToanDoan WHERE CAST(NgayThanhToan AS DATE) BETWEEN @Tu AND @Den
UNION ALL SELECT N'Phản hồi khảo sát',COUNT(*),ISNULL(AVG(CAST(DiemDanhGia AS DECIMAL(18,2))),0) FROM dbo.KhaoSat WHERE NgayPhanHoi BETWEEN @Tu AND @Den;",Db.P("@Tu",SqlDbType.Date,tu.Date),Db.P("@Den",SqlDbType.Date,den.Date));
        }
    }
}
