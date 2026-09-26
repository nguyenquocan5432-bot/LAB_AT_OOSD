# LAB 3 – Hệ thống Quản lý Khách sạn

## Thông tin sinh viên
- Họ tên: Nguyễn Quốc An
- MSSV: 1250080002
- Lớp: CNPM1
- Tên bài Lab: Lab 3 - Hệ thống quản lý khách sạn 

## Môi trường / Version
- Visual Studio 2022 / 2026
- .NET SDK (WinForms, net8.0-windows)
- SQL Server (instance cá nhân) + System.Data.SqlClient
- Hệ điều hành: Windows

## Nội dung đã thực hiện
- Khảo sát nghiệp vụ khách sạn: khu vực - phòng - tiện nghi, đặt/nhận phòng, sử dụng dịch vụ, trả phòng - đền bù - hóa đơn - thanh toán.
- Xây dựng biểu đồ Use Case tổng quát và phân rã, biểu đồ lớp phân tích, biểu đồ trạng thái (PhieuDatPhong, HoaDon), biểu đồ tuần tự cho 6 chức năng chính, biểu đồ lớp chi tiết, biểu đồ hoạt động cho chức năng Đặt phòng.
- Thiết kế CSDL SQL Server `QuanLyKhachSan` gồm 16 bảng, đầy đủ khóa chính/khóa ngoại và các ràng buộc: sức chứa phòng > 0, một thiết bị/1 phòng/1 ngày (UNIQUE), không trùng lịch đặt phòng, hóa đơn = tiền phòng + tiền dịch vụ, hình thức thanh toán giới hạn 4 loại.
- Cài đặt ứng dụng WinForms theo kiến trúc UI -> Service -> Data -> SQL Server với 7 Form: FrmMain, FrmDanhMuc, FrmPhongTienNghi, FrmDatPhong, FrmDichVu, FrmTraPhong, FrmThongKe.
- Cài đặt đầy đủ 6 Service nghiệp vụ dùng transaction cho các thao tác nhiều bảng (đặt phòng, ghi dịch vụ, đền bù, thanh toán).

## Cấu trúc thư mục bài Lab
```
LAB3/
├── README.md
├── QuanLyKhachSan.sln
├── QuanLyKhachSan.csproj
├── App.config
├── Program.cs
├── Models.cs
├── Data/                 # Db.cs - lớp truy cập dữ liệu dùng chung
├── Services/              # DanhMucService, PhongTienNghiService, DatPhongService,
│                           #   DichVuService, TraPhongService, ThongKeService
├── Forms/                 # FrmMain, FrmDanhMuc, FrmPhongTienNghi, FrmDatPhong,
│                           #   FrmDichVu, FrmTraPhong, FrmThongKe
└── Database/               # QuanLyKhachSan.sql - script tạo CSDL + dữ liệu mẫu
```

## Kết quả
- Ứng dụng chạy được, kết nối SQL Server, thực hiện đủ các luồng: đặt phòng, nhận phòng, ghi nhận dịch vụ (cộng dồn cùng ngày), lập phiếu đền bù, lập hóa đơn, thanh toán nhiều phương thức, trả phòng, thống kê theo khoảng thời gian.
- Các quy tắc đã kiểm thử: sức chứa phòng, trùng lịch đặt phòng, một thiết bị chỉ lắp 1 phòng/1 ngày, cộng dồn dịch vụ cùng ngày, chỉ trả phòng khi hóa đơn đã thanh toán đủ.

## Lỗi gặp phải và cách khắc phục
- (Điền theo thực tế khi chạy trên máy cá nhân, ví dụ: lỗi NETSDK1004 do chưa restore NuGet → chạy `dotnet restore` hoặc chuột phải solution → Restore NuGet Packages; lỗi kết nối SQL Server → kiểm tra lại chuỗi kết nối trong App.config và chế độ Mixed Mode Authentication.)

## Hướng dẫn chạy lại
1. Chạy script `Database/QuanLyKhachSan.sql` trên SQL Server để tạo cơ sở dữ liệu.
2. Mở file `QuanLyKhachSan.sln` bằng Visual Studio.
3. Trong `App.config`, chỉnh `connectionString` cho đúng SQL Server instance và tài khoản đăng nhập trên máy đang chạy.
4. Restore NuGet Packages (nếu build báo thiếu package) rồi nhấn **F5** để chạy.
