using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyCongTyDuLich.Data
{
    public static class Db
    {
        public static string ConnectionString
        {
            get
            {
                string value = Environment.GetEnvironmentVariable("VHV_CONN");
                if (!string.IsNullOrWhiteSpace(value)) return value;
                var setting = ConfigurationManager.ConnectionStrings["DuLichDb"];
                if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                    throw new InvalidOperationException("Chưa cấu hình kết nối DuLichDb trong App.config.");
                return setting.ConnectionString;
            }
        }

        public static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            try { connection.Open(); return connection; }
            catch { connection.Dispose(); throw; }
        }

        private static SqlCommand Command(SqlConnection connection,
            SqlTransaction transaction, string sql, SqlParameter[] parameters)
        {
            if (transaction != null && transaction.Connection != connection)
                throw new ArgumentException("Giao dịch không thuộc kết nối này.");

            var command = new SqlCommand(sql, connection, transaction);
            command.CommandTimeout = 30;
            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            return command;
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
                return Query(connection, null, sql, parameters);
        }

        public static DataTable Query(SqlConnection connection, SqlTransaction transaction,
            string sql, params SqlParameter[] parameters)
        {
            using (var command = Command(connection, transaction, sql, parameters))
            using (var adapter = new SqlDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
                return Execute(connection, null, sql, parameters);
        }

        public static int Execute(SqlConnection connection, SqlTransaction transaction,
            string sql, params SqlParameter[] parameters)
        {
            using (var command = Command(connection, transaction, sql, parameters))
                return command.ExecuteNonQuery();
        }

        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
                return Scalar(connection, null, sql, parameters);
        }

        public static object Scalar(SqlConnection connection, SqlTransaction transaction,
            string sql, params SqlParameter[] parameters)
        {
            using (var command = Command(connection, transaction, sql, parameters))
                return command.ExecuteScalar();
        }

        public static SqlParameter P(string name, SqlDbType type, object value, int size = 0)
        {
            var parameter = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
            if (size > 0) parameter.Size = size;
            if (type == SqlDbType.Decimal)
            {
                parameter.Precision = 18;
                parameter.Scale = 2;
            }
            return parameter;
        }
    }
}
