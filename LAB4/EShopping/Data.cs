using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EShopping
{
    // ===== Tầng Data: ADO.NET thuần, truy cập SQL Server =====

    public static class Db
    {
        // Đổi chuỗi kết nối bằng biến môi trường ESHOP_CONN nếu cần.
        public static string ConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("ESHOP_CONN")
                    ?? @"Server=BEHIHI\MSSQLSERVER01;Database=EShoppingDB;User Id=sa;Password=123;";
            }
        }

        public static SqlConnection Open()
        {
            var c = new SqlConnection(ConnectionString);
            c.Open();
            return c;
        }

        public static object P(object v) { return v ?? DBNull.Value; }
    }

    public class CustomerRepository
    {
        public bool UsernameExists(string username)
        {
            using (var c = Db.Open())
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Customer WHERE Username=@u", c))
            {
                cmd.Parameters.AddWithValue("@u", username);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public int Insert(Customer x)
        {
            const string sql = @"INSERT INTO Customer(FullName,BirthDate,IdNumber,Address,Phone,Username,PasswordHash,Email)
                                 VALUES(@n,@b,@i,@a,@p,@u,@h,@e);
                                 SELECT CAST(SCOPE_IDENTITY() AS int);";
            using (var c = Db.Open())
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@n", x.FullName);
                cmd.Parameters.AddWithValue("@b", x.BirthDate.Date);
                cmd.Parameters.AddWithValue("@i", x.IdNumber);
                cmd.Parameters.AddWithValue("@a", x.Address);
                cmd.Parameters.AddWithValue("@p", x.Phone);
                cmd.Parameters.AddWithValue("@u", x.Username);
                cmd.Parameters.AddWithValue("@h", x.PasswordHash);
                cmd.Parameters.AddWithValue("@e", Db.P(string.IsNullOrWhiteSpace(x.Email) ? null : x.Email.Trim()));
                return (int)cmd.ExecuteScalar();
            }
        }

        public Customer GetByUsername(string username)
        {
            const string sql = @"SELECT CustomerId,FullName,BirthDate,IdNumber,Address,Phone,Username,PasswordHash,Email
                                 FROM Customer WHERE Username=@u";
            using (var c = Db.Open())
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@u", username);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new Customer
                    {
                        CustomerId = r.GetInt32(0),
                        FullName = r.GetString(1),
                        BirthDate = r.GetDateTime(2),
                        IdNumber = r.GetString(3),
                        Address = r.GetString(4),
                        Phone = r.GetString(5),
                        Username = r.GetString(6),
                        PasswordHash = r.GetString(7),
                        Email = r.IsDBNull(8) ? null : r.GetString(8)
                    };
                }
            }
        }
    }

    public class LookupRepository
    {
        public IList<string> GetRegions()
        {
            var list = new List<string>();
            using (var c = Db.Open())
            using (var cmd = new SqlCommand("SELECT DISTINCT Region FROM ShippingRate ORDER BY Region", c))
            using (var r = cmd.ExecuteReader())
                while (r.Read()) list.Add(r.GetString(0));
            return list;
        }

        public decimal GetShippingFee(string region, ShippingType type)
        {
            using (var c = Db.Open())
            using (var cmd = new SqlCommand("SELECT Fee FROM ShippingRate WHERE Region=@r AND ShippingType=@t", c))
            {
                cmd.Parameters.AddWithValue("@r", region);
                cmd.Parameters.AddWithValue("@t", (int)type);
                object o = cmd.ExecuteScalar();
                if (o == null) throw new BusinessException("Chưa có bảng phí giao hàng cho khu vực này.");
                return (decimal)o;
            }
        }

        public IList<CardType> GetCardTypes()
        {
            var list = new List<CardType>();
            using (var c = Db.Open())
            using (var cmd = new SqlCommand("SELECT Code,Name,NumberLength,CsvLength,Fee FROM CardType ORDER BY Name", c))
            using (var r = cmd.ExecuteReader())
                while (r.Read())
                    list.Add(new CardType
                    {
                        Code = r.GetString(0), Name = r.GetString(1),
                        NumberLength = r.GetInt32(2), CsvLength = r.GetInt32(3), Fee = r.GetDecimal(4)
                    });
            return list;
        }
    }

    public class OrderRepository
    {
        public int Insert(Order o)
        {
            using (var c = Db.Open())
            using (var tx = c.BeginTransaction())
            {
                try
                {
                    const string sqlOrder = @"INSERT INTO Orders(CustomerId,OrderTime,ShippingType,Region,ReceiverName,ReceiverAddress,
                                                ReceiverPhone,CardTypeCode,CardMasked,CardHolder,Subtotal,ShippingFee,CardFee,Total,Status)
                                              VALUES(@cid,@t,@st,@rg,@rn,@ra,@rp,@ct,@cm,@ch,@sub,@sf,@cf,@tot,N'Đã đặt');
                                              SELECT CAST(SCOPE_IDENTITY() AS int);";
                    int id;
                    using (var cmd = new SqlCommand(sqlOrder, c, tx))
                    {
                        cmd.Parameters.AddWithValue("@cid", o.CustomerId);
                        cmd.Parameters.AddWithValue("@t", o.OrderTime);
                        cmd.Parameters.AddWithValue("@st", (int)o.ShippingType);
                        cmd.Parameters.AddWithValue("@rg", o.Region);
                        cmd.Parameters.AddWithValue("@rn", o.Receiver.FullName);
                        cmd.Parameters.AddWithValue("@ra", o.Receiver.Address);
                        cmd.Parameters.AddWithValue("@rp", o.Receiver.Phone);
                        cmd.Parameters.AddWithValue("@ct", o.CardTypeCode);
                        cmd.Parameters.AddWithValue("@cm", o.CardMasked);
                        cmd.Parameters.AddWithValue("@ch", o.CardHolder);
                        cmd.Parameters.AddWithValue("@sub", o.Subtotal);
                        cmd.Parameters.AddWithValue("@sf", o.ShippingFee);
                        cmd.Parameters.AddWithValue("@cf", o.CardFee);
                        cmd.Parameters.AddWithValue("@tot", o.Total);
                        id = (int)cmd.ExecuteScalar();
                    }

                    foreach (var it in o.Items)
                    {
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO OrderItem(OrderId,ProductId,ProductName,UnitPrice,Quantity)
                              VALUES(@o,@p,@n,@u,@q)", c, tx))
                        {
                            cmd.Parameters.AddWithValue("@o", id);
                            cmd.Parameters.AddWithValue("@p", it.ProductId);
                            cmd.Parameters.AddWithValue("@n", it.ProductName);
                            cmd.Parameters.AddWithValue("@u", it.UnitPrice);
                            cmd.Parameters.AddWithValue("@q", it.Quantity);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return id;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
