# LAB5 - Quản lý công ty du lịch Văn Hóa Việt

Project C# WinForms, .NET Framework 4.7.2, kiến trúc Form → Service → Data.

- [Báo cáo Word](BaoCao_QuanLyCongTyDuLich.docx): nội dung PDF, bốn sơ đồ UML bổ sung và 14 khung chèn ảnh Form.
- [Gói source ZIP](QuanLyCongTyDuLich_GiongHinh.zip).
- [Solution](QuanLyCongTyDuLich/QuanLyCongTyDuLich.sln).
- [Một file SQL tạo CSDL và dữ liệu mẫu](QuanLyCongTyDuLich/Database/QuanLyCongTyDuLich.sql).
- [Hướng dẫn chạy chi tiết](QuanLyCongTyDuLich/README.md).
- [Sơ đồ UML PNG và mã PlantUML](UML/).

## Chạy bằng Visual Studio 2026

1. Clone repository hoặc chọn **Code → Download ZIP** trên GitHub rồi giải nén.
2. Cài workload **.NET desktop development**, .NET Framework 4.7.2 Developer Pack và SQL Server.
3. Chạy toàn bộ `QuanLyCongTyDuLich/Database/QuanLyCongTyDuLich.sql` trong SSMS.
4. Mở `QuanLyCongTyDuLich/QuanLyCongTyDuLich.sln`, chỉnh connection string trong `App.config` theo SQL Server của bạn.
5. NuGet restore, chọn Startup Project rồi nhấn **F5**.

Trên trang file Word hoặc ZIP của GitHub, chọn **Download raw file** để tải riêng file đó.
Bản nguồn đã biên dịch cho net472; chưa kiểm thử GUI Windows hoặc giao dịch SQL Server trong môi trường Linux.
