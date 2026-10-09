# Quản lý công ty du lịch Văn Hóa Việt

WinForms C#, .NET Framework 4.7.2, mở bằng Visual Studio 2026.
Giao diện được dựng theo 8 ảnh trong `Form_image.zip`: nền xám, bảng dữ liệu,
menu hai cột, thanh nhập liệu và nút Đóng; các màn hình tương ứng dùng cùng font Segoe UI.
Bố cục có thể thay đổi kích thước theo thiết lập DPI của Windows.

## Chạy trên Windows

1. Giải nén project vào thư mục có quyền ghi.
2. Trong Visual Studio Installer, cài workload **.NET desktop development**,
   .NET Framework 4.7.2 Developer Pack/targeting pack. Gói reference assemblies
   trong project hỗ trợ NuGet restore reference khi thiếu targeting pack.
3. Cài SQL Server Express/Developer hoặc LocalDB (SQL Server 2016 SP1 trở lên).
4. Dùng SSMS kết nối instance rồi chạy toàn bộ file
   `Database/QuanLyCongTyDuLich.sql`: tạo CSDL, bảng, khóa, CHECK, chỉ mục,
   trigger; bổ sung cấu trúc cho CSDL cũ và thêm dữ liệu mẫu.
   Script có thể chạy lại, không xóa dữ liệu; seed không ghi đè bản ghi đã có.
5. Mở `QuanLyCongTyDuLich.sln`, sửa connection string **DuLichDb** trong
   `QuanLyCongTyDuLich/App.config` theo instance đã chạy SQL.
6. Đợi NuGet restore, chọn project làm Startup Project, nhấn **F5**.

Ví dụ kết nối dùng Windows Authentication:

```text
SQL Express: Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCongTyDuLich;Integrated Security=True;Connect Timeout=10;
LocalDB: Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyCongTyDuLich;Integrated Security=True;Connect Timeout=10;
Instance mặc định: Data Source=.;Initial Catalog=QuanLyCongTyDuLich;Integrated Security=True;Connect Timeout=10;
```

Biến môi trường `VHV_CONN` nếu được đặt cho tiến trình sẽ ưu tiên hơn App.config.

## Các màn hình

| Nút trên menu | Chức năng |
| --- | --- |
| Danh mục | Thêm điểm bán vé, phương tiện, điểm tham quan, HDV; xem danh sách theo tab |
| Tour - hành trình | Thêm tour, điểm dừng, gắn phương tiện theo chặng và điểm tham quan |
| Lịch chuyến khách lẻ | Tạo chuyến, tự tính ngày về, đóng đăng ký chuyến được chọn |
| Đăng ký khách lẻ | Chọn chuyến đang mở, điểm bán, nhập người đăng ký, ghi nhận đã thanh toán vé |
| Đăng ký theo đoàn | Lập phiếu, đặt cọc, lưu danh sách mua bảo hiểm, tính tổng tiền/ngày kết thúc; hủy phiếu mất cọc |
| Phân công hướng dẫn viên | Chọn LE/DOAN, HDV, đối tượng, thù lao; tự lấy lịch và kiểm tra trùng |
| Kết thúc tour - khảo sát | Thanh toán kinh phí đoàn sau tour, lập phiếu khảo sát và ghi nhận góp ý |
| Lương - thống kê | Lương HDV theo tháng kết thúc tour; số lượng/giá trị đăng ký, thanh toán, phản hồi theo khoảng ngày |

Form chỉ gọi Service. SQL và giao dịch nằm trong Service, thực thi qua helper Data/Db.cs.
Bảng hiển thị chỉ đọc, trừ bảng nhập người cùng đi. Mở lại màn hình để tải dữ liệu mới.
Phiếu khảo sát được ghi nhận trong CSDL; ứng dụng không có tích hợp gửi email/SMS.

## Thử nghiệp vụ

- **Đoàn:** thông tin mẫu D001, T001, 15 người, cọc 10.000.000 đồng, ngày đi
  sau ngày hiện tại 10 ngày. Seed T001 có 5 ngày, đơn giá 2.000.000 đồng/người:
  tổng 30.000.000 đồng. Ngày kết thúc = ngày đi + số ngày - 1.
- Nếu mua bảo hiểm, nhập đủ 15 họ tên ở bảng phía trên; ngày sinh dd/MM/yyyy,
  số giấy tờ có thể trống. Bản ghi đoàn, phiếu, thành viên dùng cùng giao dịch,
  có lỗi sẽ rollback. Khách đoàn phải > 12; khách lẻ 1–11. Đúng 12 bị từ chối
  theo hai điều kiện trong đề, không tự chuyển nhóm.
- **HDV:** DK_DEMO đã có H001. Chọn H001 phân công cho cùng đoàn sẽ bị từ chối;
  H002/H003 có thể phân công thành công. Đoàn có thể có nhiều HDV, chuyến lẻ
  tối đa một HDV. Hai lịch chạm ngày biên vẫn tính trùng.
- **Hủy đoàn:** chọn dòng trong bảng dưới rồi bấm Hủy phiếu (mất cọc).
  Phiếu giữ lịch sử/cọc; các phân công của phiếu bị gỡ. Phiếu đã có thanh toán
  sau tour không được hủy bằng chức năng này.
- **Thanh toán:** chọn phiếu ở tab Thanh toán sau tour. Ngày thanh toán phải
  sau ngày kết thúc tour, không ở tương lai; số tiền > 0 và không vượt dư nợ.
- **Khảo sát:** chỉ phiếu của tour đã kết thúc xuất hiện ở danh sách gửi;
  mỗi đăng ký có một phiếu khảo sát. Chọn khảo sát trong bảng để nhập phản hồi,
  điểm 1–5, ngày phản hồi >= ngày gửi và không ở tương lai.
- Seed chỉ có chuyến/đoàn tương lai để thử đăng ký và phân công. Muốn thử
  thanh toán/khảo sát cần đăng ký đã kết thúc. Chạy seed lại không tự dời lịch
  các bản ghi mẫu cũ.

## Cấu trúc

```text
QuanLyCongTyDuLich.sln
Database/
  QuanLyCongTyDuLich.sql
QuanLyCongTyDuLich/
  QuanLyCongTyDuLich.csproj
  App.config
  Program.cs
  Data/Db.cs
  Services/
    TourService.cs
    DanhMucService.cs
    ChuyenService.cs
    DangKyLeService.cs
    DangKyDoanService.cs
    PhanCongService.cs
    KetThucService.cs
    ThongKeService.cs
    ServiceRules.cs
    HeThongService.cs
  UI/
    Ui.cs
    FrmMain.cs
    FrmDanhMuc.cs
    FrmTour.cs
    FrmChuyenLe.cs
    FrmDangKyLe.cs
    FrmDangKyDoan.cs
    FrmDangKyDoan.Layout.cs
    FrmPhanCongHDV.cs
    FrmKetThucKhaoSat.cs
    FrmLuongThongKe.cs
```

Các form được dựng bằng C#; không cần file Designer hay .resx riêng để chạy.
Chỉnh bố cục đoàn trong `FrmDangKyDoan.Layout.cs`, bố cục chung trong `Ui.cs`.

## Kiểm tra đã thực hiện

- Biên dịch solution thật với đầy đủ WinForms, target net472: không lỗi/cảnh báo.
- 36 ca dữ liệu không hợp lệ được Service từ chối trước khi kết nối DB.
- 2 ca kiểm tra tổng tiền/ngày kết thúc: đoàn 15 người, tour 5 ngày; tour 1 ngày.
- File SQL gộp qua parser cú pháp T-SQL 2016.
- Chưa chạy GUI trên Windows hoặc giao dịch trên SQL Server trong môi trường Linux.
  Kết quả kiểm tra trên chưa thay thế kiểm thử trực quan và tích hợp trên máy Windows.

Nếu báo lỗi kết nối: kiểm tra dịch vụ SQL Server, instance trong App.config,
quyền truy cập CSDL và file `Database/QuanLyCongTyDuLich.sql` đã chạy toàn bộ
trên đúng instance.
