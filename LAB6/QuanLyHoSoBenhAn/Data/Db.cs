using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyHoSoBenhAn.Data
{
    public static class Db
    {
        private static readonly string ConnStr =
            ConfigurationManager.ConnectionStrings["QuanLyHoSoBenhAnDB"].ConnectionString;

        public static SqlConnection GetConnection() => new SqlConnection(ConnStr);

        public static SqlParameter P(string name, object value)
        {
            if (value == null) return new SqlParameter(name, DBNull.Value);
            if (value is string s && string.IsNullOrWhiteSpace(s))
                return new SqlParameter(name, DBNull.Value);
            return new SqlParameter(name, value);
        }

        public static DataTable Query(string sql, params SqlParameter[] ps)
        {
            var dt = new DataTable();
            using (var cn = GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
            }
            return dt;
        }

        public static int Execute(string sql, params SqlParameter[] ps)
        {
            using (var cn = GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params SqlParameter[] ps)
        {
            using (var cn = GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                cn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static int ExecuteInTransaction(SqlConnection cn, SqlTransaction tran, string sql, params SqlParameter[] ps)
        {
            using (var cmd = new SqlCommand(sql, cn, tran))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ScalarInTransaction(SqlConnection cn, SqlTransaction tran, string sql, params SqlParameter[] ps)
        {
            using (var cmd = new SqlCommand(sql, cn, tran))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteScalar();
            }
        }
    }
}
