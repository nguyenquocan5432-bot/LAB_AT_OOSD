# LAB 3: HỆ THỐNG QUẢN LÝ KHÁCH SẠN

## Thông tin sinh viên
- **Họ và tên:** [Điền họ tên của bạn]
- **MSSV:** [Điền MSSV]
- **Học phần:** Thiết kế và Phân tích hệ thống Hướng đối tượng (OOSD)

## Môi trường phát triển & Công cụ
- **IDE:** Visual Studio 2022
- **Ngôn ngữ:** C# (.NET Framework 4.7.2)
- **Cơ sở dữ liệu:** SQL Server (LocalDB / Express)
- **Công cụ thiết kế UML:** [Draw.io / StarUML / ...]

## Nội dung đã thực hiện
- [x] Khảo sát hiện trạng, phân loại nghiệp vụ hệ thống khách sạn.
- [x] Vẽ các biểu đồ UML: Use Case, Class Diagram, State Diagram, Sequence Diagram, Activity Diagram.
- [x] Thiết kế và cài đặt CSDL SQL Server với đầy đủ khóa chính, khóa ngoại, ràng buộc (ràng buộc giá trị > 0, thời gian trả phòng >= ngày nhận...).
- [x] Thiết kế UI WinForms theo 3 lớp (Form - Service - Data).
- [x] Lập trình các Form chức năng: FrmMain, FrmDanhMuc, FrmPhongTienNghi, FrmDatPhong, FrmDichVu, FrmTraPhong, FrmThongKe.
- [x] Kiểm thử toàn bộ quy tắc nghiệp vụ (Sức chứa phòng, check trùng lịch đặt, luân chuyển thiết bị, cộng dồn dịch vụ, thanh toán đa phương thức).

## Kết quả đạt được
- Hệ thống chạy ổn định, kết nối CSDL thành công qua phương thức `Db.cs`.
- Mọi thao tác nghiệp vụ đều được áp dụng `SqlTransaction` để bảo toàn dữ liệu (ví dụ: đặt phòng, trả phòng).
- Kết xuất báo cáo thống kê chính xác theo khoảng thời gian.
- Chi tiết hình ảnh giao diện và CSDL được đính kèm trong file báo cáo Word.

## Lỗi gặp phải & Cách khắc phục
*Ghi lại các vấn đề thực tế bạn gặp trong quá trình làm:*
- **Lỗi 1:** [Ví dụ: Xung đột khóa ngoại khi cố xóa một phòng đã có người đặt.]
- **Cách khắc phục 1:** [Ví dụ: Không cho phép xóa cứng, chỉ cho phép cập nhật trạng thái phòng thành 'Bảo trì', hoặc dùng try/catch bắt exception và thông báo cho người dùng bằng tiếng Việt.]
- **Lỗi 2:** ...
- **Cách khắc phục 2:** ...

## Hướng dẫn cài đặt & Chạy ứng dụng (Dành cho Giảng viên)
1. Clone repository này về máy cục bộ.
2. Mở SQL Server Management Studio (SSMS), chạy script `Database/QuanLyKhachSan.sql` để khởi tạo Database và dữ liệu mẫu.
3. Mở file `QuanLyKhachSan.sln` bằng Visual Studio 2022.
4. Kiểm tra và cập nhật lại chuỗi kết nối (`ConnectionString`) trong file `App.config` nếu cấu hình SQL Server trên máy giảng viên khác với LocalDB.
5. Nhấn `F5` hoặc nút **Start** để chạy chương trình.