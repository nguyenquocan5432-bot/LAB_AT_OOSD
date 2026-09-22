# LAB 2 – Hệ thống Quản lý Thư viện

## Mô tả
Ứng dụng desktop quản lý thư viện, xây dựng bằng **C# WinForms (.NET)** kết nối **SQL Server**, thực hiện các nghiệp vụ:
- Quản lý danh mục: Nhân viên, Thể loại, Nhà xuất bản
- Quản lý đầu sách (thêm/sửa/xóa/tìm kiếm)
- Quản lý độc giả, cấp thẻ và gia hạn thẻ thư viện
- Mượn sách / Trả sách, tự động tính phí phạt trả trễ / mất / hư hỏng
- Thống kê lượt mượn, sách quá hạn, sách mất, sách hư hỏng, tổng phí phạt theo khoảng thời gian

## Công nghệ sử dụng
- Ngôn ngữ: C#
- Nền tảng: .NET (WinForms)
- Cơ sở dữ liệu: Microsoft SQL Server
- IDE: Visual Studio

## Cấu trúc thư mục bài Lab
```
LAB2/
├── README.md
├── QuanLyThuVien.sln
├── QuanLyThuVien.csproj
├── App.config
├── Program.cs
├── Models.cs
├── Data/              # Lớp truy cập cơ sở dữ liệu (Db.cs)
├── Services/           # Xử lý nghiệp vụ (DanhMucService, SachService,
│                        #   DocGiaService, MuonTraService, ThongKeService)
├── Forms/              # Giao diện WinForms (FrmMain, FrmDanhMuc, FrmSach,
│                        #   FrmDocGia, FrmMuonTra, FrmThongKe)
└── Database/            # Script tạo cơ sở dữ liệu (QuanLyThuVien.sql)
```

## Hướng dẫn chạy chương trình
1. Chạy script `Database/QuanLyThuVien.sql` trên SQL Server để tạo cơ sở dữ liệu `QuanLyThuVienDB`.
2. Mở file `QuanLyThuVien.sln` bằng Visual Studio.
3. Trong `App.config`, chỉnh `connectionString` cho đúng SQL Server instance và tài khoản đăng nhập trên máy đang chạy.
4. Nhấn **F5** để build và chạy chương trình.

## Sinh viên thực hiện
- Họ tên: Nguyễn Quốc An
- MSSV:1250080002
- Lớp: CNPM1
