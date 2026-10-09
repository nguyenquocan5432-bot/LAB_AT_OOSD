using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    internal static class ServiceRules
    {
        public static string Text(string value, string label, int max)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > max)
                throw new ArgumentException("Vui lòng nhập " + label + " (tối đa " + max + " ký tự).");
            return value.Trim();
        }
        public static string Key(string value) { return Text(value, "mã/số phiếu", 20); }
        public static void Money(decimal value, bool positive = false)
        {
            if (value < 0 || (positive && value == 0) || value > 9999999999999999.99m || decimal.Round(value, 2) != value)
                throw new ArgumentException("Số tiền không hợp lệ (tối đa 2 chữ số thập phân).");
        }
        public static void Transaction(Action<SqlConnection, SqlTransaction> action)
        {
            try
            {
                using (var connection = Db.OpenConnection())
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try { action(connection, transaction); transaction.Commit(); }
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
                string message = ex.Number == 2601 || ex.Number == 2627 ? "Mã hoặc số phiếu đã tồn tại."
                    : ex.Number == 547 ? "Dữ liệu vi phạm ràng buộc hoặc chưa có trong danh mục."
                    : "Không xác nhận được kết quả. Kiểm tra kết nối và danh sách dữ liệu trước khi thử lại.";
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
