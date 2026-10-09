using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public sealed class PhanCongService
    {
        public DataTable LayHDV()
        {
            return Db.Query("SELECT MaHDV, MaHDV + N' - ' + HoTen AS Ten FROM dbo.HuongDanVien ORDER BY MaHDV;");
        }

        public DataTable LayDoiTuong(bool laDoan)
        {
            return Db.Query(laDoan ? @"
SELECT d.SoPhieu AS Ma, d.SoPhieu + N' - ' + k.TenCoQuanDaiDien AS Ten
FROM dbo.DangKyDoan d JOIN dbo.DoanKhach k ON k.MaDoan=d.MaDoan
WHERE d.TrangThai<>N'Hủy - mất cọc' AND d.NgayKetThucDuKien>=CAST(GETDATE() AS DATE)
ORDER BY d.NgayDi, d.SoPhieu;" : @"
SELECT c.MaChuyen AS Ma, c.MaChuyen + N' - ' + t.TenTour AS Ten
FROM dbo.ChuyenLe c JOIN dbo.Tour t ON t.MaTour=c.MaTour
WHERE c.TrangThai<>N'Hủy' AND c.NgayVe>=CAST(GETDATE() AS DATE)
ORDER BY c.NgayDi, c.MaChuyen;");
        }

        public DataTable LayPhanCong()
        {
            return Db.Query(@"
SELECT p.MaPC,p.MaHDV,h.HoTen,
       CASE WHEN p.MaChuyen IS NOT NULL THEN 'LE' ELSE 'DOAN' END AS LoaiDoiTuong,
       COALESCE(p.MaChuyen,p.SoPhieuDoan) AS DoiTuong,
       p.NgayBatDau,p.NgayKetThuc,p.ThuLaoTour
FROM dbo.PhanCongHDV p JOIN dbo.HuongDanVien h ON h.MaHDV=p.MaHDV
ORDER BY p.NgayBatDau, p.MaPC;");
        }

        public void PhanCong(string maPC, string maHDV, bool laDoan, string maDoiTuong, decimal thuLao)
        {
            maPC = MaHopLe(maPC, "mã phân công");
            maHDV = MaHopLe(maHDV, "hướng dẫn viên");
            maDoiTuong = MaHopLe(maDoiTuong, "đoàn/chuyến");
            if (thuLao < 0 || thuLao > 9999999999999999.99m || decimal.Round(thuLao, 2) != thuLao)
                throw new ArgumentException("Thù lao phải không âm, có tối đa 2 chữ số thập phân và nằm trong phạm vi cho phép.");

            try
            {
                using (var connection = Db.OpenConnection())
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var hdv = Db.Scalar(connection, transaction,
                            "SELECT MaHDV FROM dbo.HuongDanVien WITH (UPDLOCK,HOLDLOCK) WHERE MaHDV=@HDV;",
                            Db.P("@HDV", SqlDbType.VarChar, maHDV, 20));
                        if (hdv == null) throw new ArgumentException("Hướng dẫn viên không tồn tại.");

                        var target = Db.Query(connection, transaction, laDoan ? @"
SELECT NgayDi AS BatDau, NgayKetThucDuKien AS KetThuc FROM dbo.DangKyDoan
WHERE SoPhieu=@Ma AND TrangThai<>N'Hủy - mất cọc';" : @"
SELECT NgayDi AS BatDau, NgayVe AS KetThuc FROM dbo.ChuyenLe
WHERE MaChuyen=@Ma AND TrangThai<>N'Hủy';",
                            Db.P("@Ma", SqlDbType.VarChar, maDoiTuong, 20));
                        if (target.Rows.Count == 0) throw new ArgumentException("Đoàn/chuyến không tồn tại hoặc đã hủy.");
                        var batDau = Convert.ToDateTime(target.Rows[0]["BatDau"]);
                        var ketThuc = Convert.ToDateTime(target.Rows[0]["KetThuc"]);
                        if (ketThuc.Date < DateTime.Today) throw new ArgumentException("Tour đã kết thúc.");

                        if (!laDoan && Convert.ToInt32(Db.Scalar(connection, transaction,
                            "SELECT COUNT(*) FROM dbo.PhanCongHDV WHERE MaChuyen=@Ma;",
                            Db.P("@Ma", SqlDbType.VarChar, maDoiTuong, 20))) > 0)
                            throw new ArgumentException("Chuyến khách lẻ đã có hướng dẫn viên.");

                        int trung = Convert.ToInt32(Db.Scalar(connection, transaction, @"
SELECT COUNT(*) FROM dbo.PhanCongHDV WITH (UPDLOCK,HOLDLOCK)
WHERE MaHDV=@HDV AND NgayBatDau<=@KetThuc AND NgayKetThuc>=@BatDau;",
                            Db.P("@HDV", SqlDbType.VarChar, maHDV, 20),
                            Db.P("@BatDau", SqlDbType.Date, batDau),
                            Db.P("@KetThuc", SqlDbType.Date, ketThuc)));
                        if (trung > 0) throw new ArgumentException("HDV đã có lịch phân công trùng thời gian này.");

                        Db.Execute(connection, transaction, @"
INSERT INTO dbo.PhanCongHDV(MaPC,MaHDV,MaChuyen,SoPhieuDoan,NgayBatDau,NgayKetThuc,ThuLaoTour)
VALUES(@PC,@HDV,@Chuyen,@Doan,@BatDau,@KetThuc,@ThuLao);",
                            Db.P("@PC", SqlDbType.VarChar, maPC, 20),
                            Db.P("@HDV", SqlDbType.VarChar, maHDV, 20),
                            Db.P("@Chuyen", SqlDbType.VarChar, laDoan ? null : maDoiTuong, 20),
                            Db.P("@Doan", SqlDbType.VarChar, laDoan ? maDoiTuong : null, 20),
                            Db.P("@BatDau", SqlDbType.Date, batDau),
                            Db.P("@KetThuc", SqlDbType.Date, ketThuc),
                            Db.P("@ThuLao", SqlDbType.Decimal, thuLao));
                        transaction.Commit();
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
                string message = ex.Number == 51001 ? "HDV bị trùng lịch."
                    : ex.Number == 2601 || ex.Number == 2627 ? "Mã phân công hoặc chuyến đã được phân công."
                    : "Không xác nhận được kết quả phân công. Kiểm tra kết nối và danh sách trước khi thử lại.";
                throw new InvalidOperationException(message, ex);
            }
        }

        private static string MaHopLe(string value, string label)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 20)
                throw new ArgumentException("Vui lòng chọn/nhập " + label + " hợp lệ (tối đa 20 ký tự).");
            return value.Trim();
        }
    }
}
