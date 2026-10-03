using System;
using System.Collections.Generic;
using System.Globalization;

namespace EShopping
{
    // ===== Lớp thực thể / DTO (khớp với lớp phân tích và bảng CSDL) =====

    public enum ShippingType { Normal = 1, Express = 2, SameDay = 3 }

    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }

    public class Product            // dữ liệu lấy từ Hệ thống quản lý sản phẩm (bên ngoài)
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string Specs { get; set; }
        public string Images { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get { return Product.Price * Quantity; } }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdNumber { get; set; }       // CMND / Passport
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }          // có thể để trống
    }

    public class Receiver
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    public class CardType
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int NumberLength { get; set; }
        public int CsvLength { get; set; }
        public decimal Fee { get; set; }
    }

    public class CardInfo
    {
        public string CardTypeCode { get; set; }
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string HolderName { get; set; }
        public string Csv { get; set; }
    }

    public class Quote
    {
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal CardFee { get; set; }
        public decimal Total { get { return Subtotal + ShippingFee + CardFee; } }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderTime { get; set; }
        public ShippingType ShippingType { get; set; }
        public string Region { get; set; }
        public Receiver Receiver { get; set; }
        public string CardTypeCode { get; set; }
        public string CardMasked { get; set; }
        public string CardHolder { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal CardFee { get; set; }
        public decimal Total { get { return Subtotal + ShippingFee + CardFee; } }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public bool EmailSent { get; set; }       // không lưu CSDL
    }

    public class ShipOption
    {
        public ShippingType Type { get; set; }
        public string Name { get; set; }
    }

    public static class Fmt
    {
        static readonly CultureInfo Vi = new CultureInfo("vi-VN");

        public static string Money(decimal v) { return v.ToString("N0", Vi) + " đ"; }

        public static string ShipName(ShippingType t)
        {
            switch (t)
            {
                case ShippingType.Express: return "Chuyển phát nhanh";
                case ShippingType.SameDay: return "Chuyển phát nhanh trong ngày";
                default: return "Thường";
            }
        }
    }
}
