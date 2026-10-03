# e-SHOPPING – Prototype C# WinForms (.NET Framework 4.7.2)

Kiến trúc: **UI (Forms.cs) → Service/Adapter (Services.cs, Adapters.cs) → Data (Data.cs) → SQL Server**.
Ba hệ thống ngoài (quản lý sản phẩm, thanh toán, email) chỉ được gọi qua Adapter và hiện là bản giả lập.

## 1. Cần cài (Windows)
- .NET SDK 6 hoặc mới hơn (để dùng `dotnet build`)
- VS Code + extension **C# Dev Kit** (hoặc C#) và extension **SQL Server (mssql)**
- SQL Server Express hoặc **LocalDB** (đi kèm Visual Studio). Kiểm tra LocalDB: `sqllocaldb info`
- .NET Framework 4.7.2 Developer Pack (thường đã có sẵn trên Windows 10/11; thiếu thì dự án sẽ tự kéo reference assemblies qua NuGet)

## 2. Tạo CSDL
Mở `eshopping.sql` trong VS Code, kết nối SQL Server rồi chạy toàn bộ file (hoặc dùng `sqlcmd -S "(localdb)\MSSQLLocalDB" -i eshopping.sql -f 65001`).

## 3. Chuỗi kết nối
Mặc định: `Server=(localdb)\MSSQLLocalDB;Database=EShoppingDB;Integrated Security=true;`
Nếu dùng SQL Express, đặt biến môi trường trước khi chạy (PowerShell):
```
$env:ESHOP_CONN = "Server=.\SQLEXPRESS;Database=EShoppingDB;Integrated Security=true;"
```

## 4. Chạy
```
cd EShopping
dotnet build
dotnet run
```
Email giả lập được ghi thành file `.txt` trong thư mục `bin\Debug\net472\Outbox`.

## 5. Dữ liệu thử nghiệm
- Sản phẩm: 12 sản phẩm / 4 nhóm (CAM003 đang hết hàng) – nằm trong `MockProductAdapter`.
- Thẻ thử: Visa/Master/Discover 16 số, CSV 3 số; Amex 15 số, CSV 4 số.
  - Số thẻ kết thúc `0000` → bị dịch vụ thanh toán từ chối.
  - Số thẻ kết thúc `9999` → không đủ khả năng thanh toán.
  - Ví dụ thẻ hợp lệ: `4111111111111111`, CSV `123`.

## 6. Truy vết Yêu cầu → UML → Form/Service → Bảng CSDL
| Use case | Form | Service / Adapter | Bảng CSDL |
|---|---|---|---|
| UC01 Đăng ký | RegisterForm | AccountService.Register | Customer |
| UC02 Đăng nhập | LoginForm | AccountService.Login | Customer |
| UC03 Xem SP theo nhóm | MainForm | IProductAdapter.GetProductsByGroup | (hệ thống ngoài) |
| UC04 Xem chi tiết | ProductDetailForm | IProductAdapter.GetProduct | (hệ thống ngoài) |
| UC05 Thêm vào giỏ | MainForm, ProductDetailForm | CartService.Add | (bộ nhớ) |
| UC06 Xem giỏ | MainForm | CartService.Items | (bộ nhớ) |
| UC07 Cập nhật giỏ | MainForm | CartService.UpdateQuantity / Remove | (bộ nhớ) |
| UC08.1–08.2 Loại phiếu, người nhận | CheckoutForm | OrderService.PlaceOrder | Orders |
| UC08.3 Phí giao hàng | CheckoutForm | ShippingCalculator | ShippingRate |
| UC08.4–08.5 Thẻ, kiểm tra thẻ | CheckoutForm | OrderService + IPaymentAdapter | CardType |
| UC08.6 Ghi nhận đơn | CheckoutForm | OrderRepository.Insert | Orders, OrderItem |
| UC08.7 Email xác nhận | CheckoutForm | IEmailAdapter (FileEmailAdapter) | — |

## 7. Kịch bản kiểm thử (test case)
| TC | Kịch bản | Kết quả mong đợi |
|---|---|---|
| TC01 | Đăng ký tên đăng nhập đã tồn tại | Báo "Tên đăng nhập đã tồn tại" |
| TC02 | Đăng nhập sai mật khẩu | Báo sai tên đăng nhập hoặc mật khẩu |
| TC03 | Thêm sản phẩm hết hàng (CAM003) vào giỏ | Báo "Sản phẩm đã hết hàng" |
| TC04 | Cập nhật số lượng, loại bỏ sản phẩm | Tạm tính cập nhật đúng |
| TC05 | Bấm Tính tiền khi chưa đăng nhập | Hiện form đăng nhập trước |
| TC06 | Tiền hàng 1.200.000 đ, chọn Chuyển phát nhanh | Phí giao = Miễn phí |
| TC07 | Tiền hàng 1.200.000 đ, chọn Nhanh trong ngày | Tính phí theo khu vực |
| TC08 | Tiền hàng ≥ 5.000.000 đ, chọn Nhanh trong ngày | Phí giao = Miễn phí |
| TC09 | Visa 15 số hoặc CSV 4 số | Báo sai định dạng thẻ |
| TC10 | Amex 15 số, CSV 4 số | Hợp lệ về định dạng |
| TC11 | Số thẻ kết thúc 0000 | Báo thẻ bị từ chối, không tạo đơn, nhập lại được |
| TC12 | Thẻ hợp lệ, khách có email | Tạo Orders + OrderItem, có file email trong Outbox, email không chứa số thẻ |
| TC13 | Thẻ hợp lệ, khách không có email | Tạo đơn, không có file email |
| TC14 | Người nhận khác người mua | Đơn lưu ReceiverName khác tên khách |

Kiểm tra nhanh CSDL: `SELECT * FROM Orders; SELECT * FROM OrderItem;`
