using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EShopping
{
    // ===== Tầng Service: nghiệp vụ. UI chỉ gọi vào đây, không gọi trực tiếp Data/Adapter =====

    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(Derive(password, salt));
        }

        public static bool Verify(string password, string stored)
        {
            var parts = stored.Split(':');
            if (parts.Length != 2) return false;
            byte[] salt = Convert.FromBase64String(parts[0]);
            return Convert.ToBase64String(Derive(password, salt)) == parts[1];
        }

        static byte[] Derive(string password, byte[] salt)
        {
            using (var d = new Rfc2898DeriveBytes(password, salt, 10000)) return d.GetBytes(32);
        }
    }

    // UC01, UC02
    public class AccountService
    {
        readonly CustomerRepository _repo;
        public AccountService(CustomerRepository repo) { _repo = repo; }

        public int Register(Customer c, string password)
        {
            if (string.IsNullOrWhiteSpace(c.FullName)) throw new BusinessException("Vui lòng nhập họ tên.");
            if (c.BirthDate.Date >= DateTime.Today) throw new BusinessException("Ngày sinh không hợp lệ.");
            if (string.IsNullOrWhiteSpace(c.IdNumber)) throw new BusinessException("Vui lòng nhập số CMND/Passport.");
            if (string.IsNullOrWhiteSpace(c.Address)) throw new BusinessException("Vui lòng nhập địa chỉ.");
            if (string.IsNullOrWhiteSpace(c.Phone)) throw new BusinessException("Vui lòng nhập số điện thoại.");
            if (string.IsNullOrWhiteSpace(c.Username) || c.Username.Trim().Length < 4)
                throw new BusinessException("Tên đăng nhập tối thiểu 4 ký tự.");
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new BusinessException("Mật khẩu tối thiểu 6 ký tự.");
            if (!string.IsNullOrWhiteSpace(c.Email) && !c.Email.Contains("@"))
                throw new BusinessException("Địa chỉ email không hợp lệ.");

            c.Username = c.Username.Trim();
            if (_repo.UsernameExists(c.Username)) throw new BusinessException("Tên đăng nhập đã tồn tại.");

            c.PasswordHash = PasswordHasher.Hash(password);
            return _repo.Insert(c);
        }

        public Customer Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                throw new BusinessException("Vui lòng nhập tên đăng nhập và mật khẩu.");
            var c = _repo.GetByUsername(username.Trim());
            if (c == null || !PasswordHasher.Verify(password, c.PasswordHash))
                throw new BusinessException("Sai tên đăng nhập hoặc mật khẩu.");
            return c;
        }
    }

    // UC05, UC06, UC07 (giỏ hàng lưu trong bộ nhớ)
    public class CartService
    {
        readonly List<CartItem> _items = new List<CartItem>();

        public IReadOnlyList<CartItem> Items { get { return _items; } }
        public bool IsEmpty { get { return _items.Count == 0; } }
        public decimal Subtotal { get { return _items.Sum(i => i.LineTotal); } }

        public void Add(Product p, int qty)
        {
            if (!p.InStock) throw new BusinessException("Sản phẩm đã hết hàng.");
            if (qty < 1) throw new BusinessException("Số lượng phải lớn hơn 0.");
            var ex = _items.FirstOrDefault(i => i.Product.Id == p.Id);
            if (ex != null) ex.Quantity += qty;
            else _items.Add(new CartItem { Product = p, Quantity = qty });
        }

        public void UpdateQuantity(string productId, int qty)
        {
            if (qty < 1) throw new BusinessException("Số lượng phải lớn hơn 0.");
            var it = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (it != null) it.Quantity = qty;
        }

        public void Remove(string productId) { _items.RemoveAll(i => i.Product.Id == productId); }
        public void Clear() { _items.Clear(); }
    }

    // UC08.3: tính phí giao hàng
    public class ShippingCalculator
    {
        public const decimal FreeExpressFrom = 1000000m;
        public const decimal FreeSameDayFrom = 5000000m;

        readonly LookupRepository _lookup;
        public ShippingCalculator(LookupRepository lookup) { _lookup = lookup; }

        public decimal Calculate(decimal subtotal, ShippingType type, string region)
        {
            if (type == ShippingType.Express && subtotal >= FreeExpressFrom) return 0m;
            if (type == ShippingType.SameDay && subtotal >= FreeSameDayFrom) return 0m;
            return _lookup.GetShippingFee(region, type);
        }
    }

    // UC08: đặt hàng và tính tiền
    public class OrderService
    {
        readonly IProductAdapter _products;
        readonly IPaymentAdapter _payment;
        readonly IEmailAdapter _email;
        readonly OrderRepository _orders;
        readonly LookupRepository _lookup;
        readonly ShippingCalculator _shipping;

        public OrderService(IProductAdapter products, IPaymentAdapter payment, IEmailAdapter email,
                            OrderRepository orders, LookupRepository lookup, ShippingCalculator shipping)
        {
            _products = products; _payment = payment; _email = email;
            _orders = orders; _lookup = lookup; _shipping = shipping;
        }

        public Quote GetQuote(decimal subtotal, ShippingType type, string region, string cardTypeCode)
        {
            var ct = _lookup.GetCardTypes().FirstOrDefault(x => x.Code == cardTypeCode);
            return new Quote
            {
                Subtotal = subtotal,
                ShippingFee = _shipping.Calculate(subtotal, type, region),
                CardFee = ct == null ? 0m : ct.Fee
            };
        }

        public Order PlaceOrder(Customer buyer, Receiver receiver, ShippingType type, string region,
                                CardInfo card, CartService cart)
        {
            if (cart.IsEmpty) throw new BusinessException("Giỏ hàng trống.");
            if (receiver == null || string.IsNullOrWhiteSpace(receiver.FullName) ||
                string.IsNullOrWhiteSpace(receiver.Address) || string.IsNullOrWhiteSpace(receiver.Phone))
                throw new BusinessException("Vui lòng nhập đầy đủ họ tên, địa chỉ, điện thoại người nhận.");
            if (string.IsNullOrEmpty(region)) throw new BusinessException("Vui lòng chọn khu vực giao hàng.");

            CardType ct = ValidateCardFormat(card);

            // Lấy lại giá hiện hành từ hệ thống quản lý sản phẩm (giá có thể đã thay đổi)
            foreach (var item in cart.Items)
            {
                var fresh = _products.GetProduct(item.Product.Id);
                if (fresh == null || !fresh.InStock)
                    throw new BusinessException("Sản phẩm '" + item.Product.Name + "' hiện đã hết hàng. Vui lòng cập nhật giỏ hàng.");
                item.Product = fresh;
            }

            Quote q = GetQuote(cart.Subtotal, type, region, card.CardTypeCode);

            PaymentResult pr = _payment.Verify(card, q.Total);
            if (!pr.Success) throw new BusinessException(pr.Message);

            var order = new Order
            {
                CustomerId = buyer.CustomerId,
                OrderTime = DateTime.Now,
                ShippingType = type,
                Region = region,
                Receiver = receiver,
                CardTypeCode = ct.Code,
                CardMasked = new string('*', card.Number.Length - 4) + card.Number.Substring(card.Number.Length - 4),
                CardHolder = card.HolderName.Trim(),
                Subtotal = q.Subtotal,
                ShippingFee = q.ShippingFee,
                CardFee = q.CardFee
            };
            foreach (var i in cart.Items)
                order.Items.Add(new OrderItem
                {
                    ProductId = i.Product.Id, ProductName = i.Product.Name,
                    UnitPrice = i.Product.Price, Quantity = i.Quantity
                });

            order.OrderId = _orders.Insert(order);

            // Gửi email xác nhận nếu khách có email. Lỗi email không làm mất đơn hàng.
            if (!string.IsNullOrWhiteSpace(buyer.Email))
            {
                try
                {
                    _email.Send(buyer.Email, "Xác nhận đơn hàng #" + order.OrderId, BuildEmailBody(buyer, order));
                    order.EmailSent = true;
                }
                catch { order.EmailSent = false; }
            }

            cart.Clear();
            return order;
        }

        CardType ValidateCardFormat(CardInfo card)
        {
            var ct = _lookup.GetCardTypes().FirstOrDefault(x => x.Code == card.CardTypeCode);
            if (ct == null) throw new BusinessException("Vui lòng chọn loại thẻ.");
            if (string.IsNullOrWhiteSpace(card.Number) || !card.Number.All(char.IsDigit) || card.Number.Length != ct.NumberLength)
                throw new BusinessException("Số thẻ " + ct.Name + " phải gồm đúng " + ct.NumberLength + " chữ số.");
            if (string.IsNullOrWhiteSpace(card.Csv) || !card.Csv.All(char.IsDigit) || card.Csv.Length != ct.CsvLength)
                throw new BusinessException("Mã CSV của thẻ " + ct.Name + " phải gồm đúng " + ct.CsvLength + " chữ số.");
            if (string.IsNullOrWhiteSpace(card.HolderName))
                throw new BusinessException("Vui lòng nhập họ tên chủ thẻ.");
            var now = DateTime.Today;
            if (card.ExpiryYear < now.Year || (card.ExpiryYear == now.Year && card.ExpiryMonth < now.Month))
                throw new BusinessException("Thẻ đã hết hạn.");
            return ct;
        }

        // Email KHÔNG chứa thông tin thẻ tín dụng (yêu cầu an ninh)
        static string BuildEmailBody(Customer buyer, Order o)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Cảm ơn " + buyer.FullName + " đã đặt hàng tại e-SHOPPING (cửa hàng ABC).");
            sb.AppendLine();
            sb.AppendLine("Mã đơn hàng : " + o.OrderId);
            sb.AppendLine("Thời điểm đặt: " + o.OrderTime.ToString("dd/MM/yyyy HH:mm"));
            sb.AppendLine("Người mua    : " + buyer.FullName + " - " + buyer.Phone);
            sb.AppendLine("Người nhận   : " + o.Receiver.FullName + " - " + o.Receiver.Phone);
            sb.AppendLine("Địa chỉ giao : " + o.Receiver.Address + " (" + o.Region + ")");
            sb.AppendLine("Hình thức giao: " + Fmt.ShipName(o.ShippingType));
            sb.AppendLine();
            sb.AppendLine("Sản phẩm:");
            foreach (var i in o.Items)
                sb.AppendLine(" - " + i.ProductName + " x" + i.Quantity + " @ " + Fmt.Money(i.UnitPrice));
            sb.AppendLine();
            sb.AppendLine("Tiền hàng     : " + Fmt.Money(o.Subtotal));
            sb.AppendLine("Phí giao hàng : " + Fmt.Money(o.ShippingFee));
            sb.AppendLine("Lệ phí thẻ    : " + Fmt.Money(o.CardFee));
            sb.AppendLine("TỔNG CỘNG     : " + Fmt.Money(o.Total));
            return sb.ToString();
        }
    }

    // Nơi khởi tạo và giữ các đối tượng dùng chung (composition root đơn giản)
    public static class AppServices
    {
        public static readonly IProductAdapter Products = new MockProductAdapter();
        public static readonly IPaymentAdapter Payment = new MockPaymentAdapter();
        public static readonly IEmailAdapter Email = new FileEmailAdapter(
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox"));

        public static readonly CustomerRepository CustomerRepo = new CustomerRepository();
        public static readonly OrderRepository OrderRepo = new OrderRepository();
        public static readonly LookupRepository Lookup = new LookupRepository();

        public static readonly AccountService Accounts = new AccountService(CustomerRepo);
        public static readonly CartService Cart = new CartService();
        public static readonly ShippingCalculator Shipping = new ShippingCalculator(Lookup);
        public static readonly OrderService Orders = new OrderService(Products, Payment, Email, OrderRepo, Lookup, Shipping);

        public static Customer CurrentCustomer { get; set; }
    }
}
