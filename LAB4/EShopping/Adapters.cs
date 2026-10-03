using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EShopping
{
    // ===== Adapter: lớp duy nhất được phép gọi 3 hệ thống bên ngoài =====
    // Đây là bản GIẢ LẬP (mock). Khi có hệ thống thật chỉ cần viết lớp mới cài đặt interface.

    // ---------- 1. Hệ thống quản lý sản phẩm ----------
    public interface IProductAdapter
    {
        IList<string> GetGroups();
        IList<Product> GetProductsByGroup(string group);
        Product GetProduct(string id);
    }

    public class MockProductAdapter : IProductAdapter
    {
        const string G1 = "Máy chụp hình kỹ thuật số";
        const string G2 = "Đồ chơi";
        const string G3 = "Thiết bị điện gia dụng";
        const string G4 = "Thiết bị máy tính";

        readonly List<Product> _all = new List<Product>
        {
            P("CAM001","Canon IXUS 185","Canon",G1,"Máy ảnh compact 20MP, zoom quang 8x.","20MP; zoom 8x; quay HD 720p",2800000m,true),
            P("CAM002","Sony Cyber-shot W830","Sony",G1,"Máy ảnh nhỏ gọn, chống rung.","20.1MP; zoom 8x; chống rung",3200000m,true),
            P("CAM003","Nikon COOLPIX A300","Nikon",G1,"Máy ảnh du lịch, dễ dùng.","20.1MP; zoom 8x; Full HD",3500000m,false),
            P("TOY001","Bộ xếp hình 500 mảnh","Lego",G2,"Bộ xếp hình sáng tạo cho trẻ trên 6 tuổi.","500 mảnh; nhựa ABS",450000m,true),
            P("TOY002","Xe điều khiển từ xa","Hot Wheels",G2,"Xe địa hình điều khiển từ xa 2.4GHz.","Pin sạc; tầm xa 30m",680000m,true),
            P("TOY003","Gấu bông 80cm","Teddy",G2,"Gấu bông mềm mại, quà tặng Giáng Sinh.","80cm; bông PP",250000m,true),
            P("HOM001","Nồi cơm điện 1.8L","Panasonic",G3,"Nồi cơm điện nắp gài.","1.8L; 700W",1200000m,true),
            P("HOM002","Máy xay sinh tố","Philips",G3,"Máy xay đa năng cối thủy tinh.","600W; cối 1.5L",950000m,true),
            P("HOM003","Lò vi sóng 25L","Sharp",G3,"Lò vi sóng có nướng.","25L; 900W",2900000m,true),
            P("PC001","Chuột không dây","Logitech",G4,"Chuột không dây 2.4GHz.","1000 DPI; pin AA",350000m,true),
            P("PC002","Bàn phím cơ","Keychron",G4,"Bàn phím cơ không dây.","Bluetooth; switch đỏ",1800000m,true),
            P("PC003","Laptop 15.6 inch","Dell",G4,"Laptop văn phòng.","i5; 8GB RAM; SSD 512GB",14500000m,true),
        };

        static Product P(string id, string name, string maker, string group, string desc, string specs, decimal price, bool inStock)
        {
            return new Product
            {
                Id = id, Name = name, Manufacturer = maker, Group = group, Description = desc,
                Specs = specs, Images = id.ToLower() + "_1.jpg; " + id.ToLower() + "_2.jpg",
                Price = price, InStock = inStock
            };
        }

        public IList<string> GetGroups() { return _all.Select(p => p.Group).Distinct().ToList(); }

        public IList<Product> GetProductsByGroup(string group)
        {
            return _all.Where(p => p.Group == group).ToList();
        }

        public Product GetProduct(string id) { return _all.FirstOrDefault(p => p.Id == id); }
    }

    // ---------- 2. Dịch vụ thanh toán trực tuyến ----------
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public interface IPaymentAdapter
    {
        PaymentResult Verify(CardInfo card, decimal amount);
    }

    public class MockPaymentAdapter : IPaymentAdapter
    {
        // Quy ước thử nghiệm: số thẻ kết thúc 0000 => thẻ bị từ chối;
        //                     số thẻ kết thúc 9999 => không đủ khả năng thanh toán;
        //                     còn lại => hợp lệ.
        public PaymentResult Verify(CardInfo card, decimal amount)
        {
            if (card.Number.EndsWith("0000"))
                return new PaymentResult { Success = false, Message = "Thông tin thẻ không hợp lệ (bị dịch vụ thanh toán từ chối)." };
            if (card.Number.EndsWith("9999"))
                return new PaymentResult { Success = false, Message = "Thẻ không đủ khả năng thanh toán." };
            return new PaymentResult { Success = true, Message = "OK" };
        }
    }

    // ---------- 3. Dịch vụ email ----------
    public interface IEmailAdapter
    {
        void Send(string to, string subject, string body);
    }

    public class FileEmailAdapter : IEmailAdapter
    {
        readonly string _folder;

        public FileEmailAdapter(string folder) { _folder = folder; }

        // Giả lập gửi email: ghi nội dung ra file .txt trong thư mục Outbox
        public void Send(string to, string subject, string body)
        {
            Directory.CreateDirectory(_folder);
            string file = Path.Combine(_folder, DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".txt");
            File.WriteAllText(file, "To: " + to + Environment.NewLine + "Subject: " + subject +
                                    Environment.NewLine + Environment.NewLine + body);
        }
    }
}
