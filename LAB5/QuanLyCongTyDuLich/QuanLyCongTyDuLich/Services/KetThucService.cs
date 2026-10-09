using System;
using System.Data;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class KetThucService
    {
        public DataTable LayCongNo()
        {
            return Db.Query(@"SELECT d.SoPhieu AS SoDKDoan,k.TenCoQuanDaiDien,t.TenTour,d.NgayKetThucDuKien,d.TongTienDuKien,d.TienCoc,
ISNULL(tt.DaTra,0) AS DaTraSauTour,d.TongTienDuKien-d.TienCoc-ISNULL(tt.DaTra,0) AS ConLai,d.TrangThai
FROM dbo.DangKyDoan d JOIN dbo.DoanKhach k ON k.MaDoan=d.MaDoan JOIN dbo.Tour t ON t.MaTour=d.MaTour
LEFT JOIN(SELECT SoPhieuDoan,SUM(SoTien) DaTra FROM dbo.ThanhToanDoan GROUP BY SoPhieuDoan) tt ON tt.SoPhieuDoan=d.SoPhieu
WHERE d.TrangThai<>N'Hủy - mất cọc' ORDER BY d.NgayKetThucDuKien DESC;");
        }
        public void ThanhToan(string soTT,string phieu,DateTime ngay,decimal soTien,string ghiChu)
        {
            soTT=ServiceRules.Key(soTT); phieu=ServiceRules.Key(phieu); ServiceRules.Money(soTien,true);
            if(ngay.Date>DateTime.Today) throw new ArgumentException("Ngày thanh toán không được ở tương lai.");
            if((ghiChu??"").Length>300) throw new ArgumentException("Ghi chú tối đa 300 ký tự.");
            ServiceRules.Transaction((c,t)=>
            {
                var d=Db.Query(c,t,"SELECT NgayKetThucDuKien,TongTienDuKien,TienCoc,TrangThai FROM dbo.DangKyDoan WITH(UPDLOCK,HOLDLOCK) WHERE SoPhieu=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20));
                if(d.Rows.Count==0 || Convert.ToString(d.Rows[0]["TrangThai"])=="Hủy - mất cọc") throw new ArgumentException("Phiếu đoàn không hợp lệ.");
                if(ngay.Date<=Convert.ToDateTime(d.Rows[0]["NgayKetThucDuKien"]).Date) throw new ArgumentException("Chỉ thanh toán kinh phí sau ngày kết thúc tour.");
                decimal da=Convert.ToDecimal(Db.Scalar(c,t,"SELECT ISNULL(SUM(SoTien),0) FROM dbo.ThanhToanDoan WHERE SoPhieuDoan=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20)));
                decimal con=Convert.ToDecimal(d.Rows[0]["TongTienDuKien"])-Convert.ToDecimal(d.Rows[0]["TienCoc"])-da;
                if(soTien>con) throw new ArgumentException("Số tiền vượt phần còn phải trả.");
                Db.Execute(c,t,"INSERT dbo.ThanhToanDoan(SoTT,SoPhieuDoan,NgayThanhToan,SoTien,GhiChu) VALUES(@So,@Ma,@Ngay,@Tien,@Ghi);",Db.P("@So",SqlDbType.VarChar,soTT,20),Db.P("@Ma",SqlDbType.VarChar,phieu,20),Db.P("@Ngay",SqlDbType.DateTime2,ngay.Date),Db.P("@Tien",SqlDbType.Decimal,soTien),Db.P("@Ghi",SqlDbType.NVarChar,ghiChu,300));
                if(soTien==con) Db.Execute(c,t,"UPDATE dbo.DangKyDoan SET TrangThai=N'Đã hoàn tất thanh toán' WHERE SoPhieu=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20));
            });
        }
        public DataTable LayKhaoSat()
        {
            return Db.Query("SELECT MaKhaoSat,CASE WHEN SoPhieuLe IS NULL THEN 'DOAN' ELSE 'LE' END AS LoaiKhach,COALESCE(SoPhieuLe,SoPhieuDoan) AS SoDangKy,NgayGui,NgayPhanHoi,DiemDanhGia,GopY FROM dbo.KhaoSat ORDER BY NgayGui DESC;");
        }
        public DataTable LayChoKhaoSat(bool doan)
        {
            return Db.Query(doan?@"SELECT d.SoPhieu AS Ma,d.SoPhieu+N' - '+k.TenCoQuanDaiDien AS Ten FROM dbo.DangKyDoan d JOIN dbo.DoanKhach k ON k.MaDoan=d.MaDoan WHERE d.TrangThai<>N'Hủy - mất cọc' AND d.NgayKetThucDuKien<CAST(GETDATE() AS DATE) AND NOT EXISTS(SELECT 1 FROM dbo.KhaoSat s WHERE s.SoPhieuDoan=d.SoPhieu);"
                :@"SELECT d.SoPhieu AS Ma,d.SoPhieu+N' - '+d.TenNguoiDangKy AS Ten FROM dbo.DangKyLe d JOIN dbo.ChuyenLe c ON c.MaChuyen=d.MaChuyen WHERE c.NgayVe<CAST(GETDATE() AS DATE) AND NOT EXISTS(SELECT 1 FROM dbo.KhaoSat s WHERE s.SoPhieuLe=d.SoPhieu);");
        }
        public void GuiKhaoSat(string ma,bool doan,string phieu,DateTime ngay)
        {
            ma=ServiceRules.Key(ma); phieu=ServiceRules.Key(phieu);
            if(ngay.Date>DateTime.Today) throw new ArgumentException("Ngày gửi không được ở tương lai.");
            ServiceRules.Transaction((c,t)=>
            {
                object end=Db.Scalar(c,t,doan?"SELECT NgayKetThucDuKien FROM dbo.DangKyDoan WHERE SoPhieu=@Ma AND TrangThai<>N'Hủy - mất cọc';":"SELECT c.NgayVe FROM dbo.DangKyLe d JOIN dbo.ChuyenLe c ON c.MaChuyen=d.MaChuyen WHERE d.SoPhieu=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20));
                if(end==null || ngay.Date<=Convert.ToDateTime(end).Date) throw new ArgumentException("Chỉ gửi khảo sát sau ngày kết thúc tour.");
                Db.Execute(c,t,"INSERT dbo.KhaoSat(MaKhaoSat,SoPhieuLe,SoPhieuDoan,NgayGui) VALUES(@Ma,@Le,@Doan,@Ngay);",Db.P("@Ma",SqlDbType.VarChar,ma,20),Db.P("@Le",SqlDbType.VarChar,doan?null:phieu,20),Db.P("@Doan",SqlDbType.VarChar,doan?phieu:null,20),Db.P("@Ngay",SqlDbType.Date,ngay.Date));
            });
        }
        public void GhiPhanHoi(string ma,DateTime ngay,int diem,string gopY)
        {
            ma=ServiceRules.Key(ma);
            if(diem<1 || diem>5 || ngay.Date>DateTime.Today || (gopY??"").Length>1500) throw new ArgumentException("Điểm từ 1 đến 5; ngày phản hồi không ở tương lai; góp ý tối đa 1500 ký tự.");
            ServiceRules.Transaction((c,t)=>
            {
                object sent=Db.Scalar(c,t,"SELECT NgayGui FROM dbo.KhaoSat WHERE MaKhaoSat=@Ma;",Db.P("@Ma",SqlDbType.VarChar,ma,20));
                if(sent==null || ngay.Date<Convert.ToDateTime(sent).Date) throw new ArgumentException("Phiếu khảo sát/ngày phản hồi không hợp lệ.");
                Db.Execute(c,t,"UPDATE dbo.KhaoSat SET NgayPhanHoi=@Ngay,DiemDanhGia=@Diem,GopY=@Y WHERE MaKhaoSat=@Ma;",Db.P("@Ngay",SqlDbType.Date,ngay.Date),Db.P("@Diem",SqlDbType.TinyInt,diem),Db.P("@Y",SqlDbType.NVarChar,gopY,1500),Db.P("@Ma",SqlDbType.VarChar,ma,20));
            });
        }
    }
}
