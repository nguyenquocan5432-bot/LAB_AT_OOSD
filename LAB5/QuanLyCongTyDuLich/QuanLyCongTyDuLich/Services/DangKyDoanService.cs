using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class ThanhVienDoanInput
    {
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string SoGiayTo { get; set; }
    }

    public sealed class DangKyDoanInput
    {
        public string SoPhieu { get; set; }
        public string MaDoan { get; set; }
        public string TenCoQuanDaiDien { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string NguoiDaiDien { get; set; }
        public string MaTour { get; set; }
        public int SoNguoi { get; set; }
        public DateTime NgayDi { get; set; }
        public string DiaDiemDon { get; set; }
        public bool MuaBaoHiem { get; set; }
        public decimal TienCoc { get; set; }
        public List<ThanhVienDoanInput> ThanhVien { get; set; }
            = new List<ThanhVienDoanInput>();
    }

    public sealed class KetQuaDangKy
    {
        public string SoPhieu { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienCoc { get; set; }
    }

    public sealed class DangKyDoanService
    {
        public DataTable LayPhieu()
        {
            return Db.Query(@"SELECT d.SoPhieu AS SoDKDoan,d.MaDoan,k.TenCoQuanDaiDien,t.TenTour,d.NgayDi,d.NgayKetThucDuKien,d.SoNguoi,d.MuaBaoHiem,d.TienCoc,d.TongTienDuKien,d.TrangThai
FROM dbo.DangKyDoan d JOIN dbo.DoanKhach k ON k.MaDoan=d.MaDoan JOIN dbo.Tour t ON t.MaTour=d.MaTour ORDER BY d.NgayDangKy DESC;");
        }
        public void HuyPhieu(string phieu)
        {
            phieu=ServiceRules.Key(phieu);
            ServiceRules.Transaction((c,t)=>
            {
                object status=Db.Scalar(c,t,"SELECT TrangThai FROM dbo.DangKyDoan WITH(UPDLOCK,HOLDLOCK) WHERE SoPhieu=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20));
                if(status==null || Convert.ToString(status)!= "Đã đăng ký") throw new ArgumentException("Phiếu không tồn tại hoặc không còn được hủy.");
                if(Convert.ToInt32(Db.Scalar(c,t,"SELECT COUNT(*) FROM dbo.ThanhToanDoan WHERE SoPhieuDoan=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20)))>0)
                    throw new ArgumentException("Phiếu đã có thanh toán sau tour, không thể hủy.");
                Db.Execute(c,t,"DELETE FROM dbo.PhanCongHDV WHERE SoPhieuDoan=@Ma; UPDATE dbo.DangKyDoan SET TrangThai=N'Hủy - mất cọc' WHERE SoPhieu=@Ma;",Db.P("@Ma",SqlDbType.VarChar,phieu,20));
            });
        }

        private static string Required(string value, string label, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Vui lòng nhập " + label + ".");
            value = value.Trim();
            if (value.Length > maxLength)
                throw new ArgumentException(label + " vượt quá " + maxLength + " ký tự.");
            return value;
        }

        private static void Validate(DangKyDoanInput input, DateTime ngayDangKy)
        {
            if (input == null) throw new ArgumentNullException("input");
            input.SoPhieu = Required(input.SoPhieu, "số phiếu", 20);
            input.MaDoan = Required(input.MaDoan, "mã đoàn", 20);
            input.TenCoQuanDaiDien = Required(input.TenCoQuanDaiDien, "tên cơ quan/đoàn", 200);
            input.DiaChi = Required(input.DiaChi, "địa chỉ", 250);
            input.DienThoai = Required(input.DienThoai, "điện thoại", 20);
            input.NguoiDaiDien = Required(input.NguoiDaiDien, "người đại diện", 120);
            input.MaTour = Required(input.MaTour, "mã tour", 20);
            input.DiaDiemDon = Required(input.DiaDiemDon, "địa điểm đón", 250);

            if (input.SoNguoi <= 12)
                throw new ArgumentException("Khách đoàn phải trên 12 người; đúng 12 người chưa được quy định.");
            if (input.NgayDi.Date <= ngayDangKy.Date)
                throw new ArgumentException("Ngày đi phải sau ngày đăng ký.");
            if (input.TienCoc <= 0 || decimal.Round(input.TienCoc, 2) != input.TienCoc)
                throw new ArgumentException("Tiền cọc phải lớn hơn 0 và có tối đa 2 chữ số thập phân.");

            if (!input.MuaBaoHiem) return;
            if (input.ThanhVien == null || input.ThanhVien.Count != input.SoNguoi)
                throw new ArgumentException("Mua bảo hiểm phải nhập danh sách đủ số người cùng đi.");
            foreach (var member in input.ThanhVien)
            {
                if (member == null) throw new ArgumentException("Danh sách có thành viên không hợp lệ.");
                member.HoTen = Required(member.HoTen, "họ tên thành viên", 120);
                member.SoGiayTo = (member.SoGiayTo ?? "").Trim();
                if (member.SoGiayTo.Length > 40)
                    throw new ArgumentException("Số giấy tờ vượt quá 40 ký tự.");
            }
        }

        public KetQuaDangKy LapPhieu(DangKyDoanInput input)
        {
            DateTime ngayDangKy = DateTime.Now;
            Validate(input, ngayDangKy);

            try
            {
                using (var connection = Db.OpenConnection())
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var tour = Db.Query(connection, transaction,
                            "SELECT SoNgay, DonGiaKhach FROM dbo.Tour WHERE MaTour=@MaTour AND DangMoBan=1;",
                            Db.P("@MaTour", SqlDbType.VarChar, input.MaTour, 20));
                        if (tour.Rows.Count == 0)
                            throw new ArgumentException("Tour không tồn tại hoặc chưa mở bán.");

                        int soNgay = Convert.ToInt32(tour.Rows[0]["SoNgay"]);
                        decimal tongTien = Convert.ToDecimal(tour.Rows[0]["DonGiaKhach"]) * input.SoNguoi;
                        if (tongTien > 9999999999999999.99m)
                            throw new ArgumentException("Tổng tiền vượt phạm vi DECIMAL(18,2).");
                        if (input.TienCoc > tongTien)
                            throw new ArgumentException("Tiền cọc không được vượt tổng tiền dự kiến.");
                        DateTime ngayKetThuc = input.NgayDi.Date.AddDays(soNgay - 1);

                        Db.Execute(connection, transaction, @"
IF EXISTS (SELECT 1 FROM dbo.DoanKhach WITH (UPDLOCK, HOLDLOCK) WHERE MaDoan=@MaDoan)
    UPDATE dbo.DoanKhach
    SET TenCoQuanDaiDien=@TenDoan, DiaChi=@DiaChi,
        DienThoai=@DienThoai, NguoiDaiDien=@DaiDien
    WHERE MaDoan=@MaDoan;
ELSE
    INSERT INTO dbo.DoanKhach(MaDoan, TenCoQuanDaiDien, DiaChi, DienThoai, NguoiDaiDien)
    VALUES(@MaDoan, @TenDoan, @DiaChi, @DienThoai, @DaiDien);",
                            Db.P("@MaDoan", SqlDbType.VarChar, input.MaDoan, 20),
                            Db.P("@TenDoan", SqlDbType.NVarChar, input.TenCoQuanDaiDien, 200),
                            Db.P("@DiaChi", SqlDbType.NVarChar, input.DiaChi, 250),
                            Db.P("@DienThoai", SqlDbType.VarChar, input.DienThoai, 20),
                            Db.P("@DaiDien", SqlDbType.NVarChar, input.NguoiDaiDien, 120));

                        Db.Execute(connection, transaction, @"
INSERT INTO dbo.DangKyDoan
    (SoPhieu, MaDoan, MaTour, NgayDangKy, SoNguoi, NgayDi, NgayKetThucDuKien,
     DiaDiemDon, MuaBaoHiem, TienCoc, TongTienDuKien, TrangThai)
VALUES
    (@SoPhieu, @MaDoan, @MaTour, @NgayDK, @SoNguoi, @NgayDi, @NgayKT,
     @NoiDon, @BaoHiem, @TienCoc, @TongTien, N'Đã đăng ký');",
                            Db.P("@SoPhieu", SqlDbType.VarChar, input.SoPhieu, 20),
                            Db.P("@MaDoan", SqlDbType.VarChar, input.MaDoan, 20),
                            Db.P("@MaTour", SqlDbType.VarChar, input.MaTour, 20),
                            Db.P("@NgayDK", SqlDbType.DateTime2, ngayDangKy),
                            Db.P("@SoNguoi", SqlDbType.Int, input.SoNguoi),
                            Db.P("@NgayDi", SqlDbType.Date, input.NgayDi.Date),
                            Db.P("@NgayKT", SqlDbType.Date, ngayKetThuc),
                            Db.P("@NoiDon", SqlDbType.NVarChar, input.DiaDiemDon, 250),
                            Db.P("@BaoHiem", SqlDbType.Bit, input.MuaBaoHiem),
                            Db.P("@TienCoc", SqlDbType.Decimal, input.TienCoc),
                            Db.P("@TongTien", SqlDbType.Decimal, tongTien));

                        if (input.MuaBaoHiem)
                        {
                            for (int i = 0; i < input.ThanhVien.Count; i++)
                            {
                                var member = input.ThanhVien[i];
                                Db.Execute(connection, transaction, @"
INSERT INTO dbo.ThanhVienDoan(SoPhieu, STT, HoTen, NgaySinh, SoGiayTo)
VALUES(@SoPhieu, @STT, @HoTen, @NgaySinh, @SoGiayTo);",
                                    Db.P("@SoPhieu", SqlDbType.VarChar, input.SoPhieu, 20),
                                    Db.P("@STT", SqlDbType.Int, i + 1),
                                    Db.P("@HoTen", SqlDbType.NVarChar, member.HoTen, 120),
                                    Db.P("@NgaySinh", SqlDbType.Date, member.NgaySinh),
                                    Db.P("@SoGiayTo", SqlDbType.NVarChar,
                                        string.IsNullOrEmpty(member.SoGiayTo) ? null : member.SoGiayTo, 40));
                            }
                        }

                        var result = new KetQuaDangKy
                        {
                            SoPhieu = input.SoPhieu, NgayKetThuc = ngayKetThuc,
                            TongTien = tongTien, TienCoc = input.TienCoc
                        };
                        transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        try { transaction.Rollback(); }
                        catch (SqlException) { }
                        catch (InvalidOperationException) { }
                        throw;
                    }
                }
            }
            catch (SqlException ex)
            {
                string message = (ex.Number == 2601 || ex.Number == 2627)
                    ? "Số phiếu hoặc khóa dữ liệu đã tồn tại. Vui lòng kiểm tra lại."
                    : "Không xác nhận được kết quả lập phiếu. Kiểm tra kết nối và dữ liệu trước khi thử lại.";
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
