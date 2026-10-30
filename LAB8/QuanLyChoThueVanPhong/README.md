# Bài 4 – Quản lý cho thuê văn phòng ở cao ốc

C# WinForms (.NET 10) · SQL Server · ADO.NET – kiến trúc 3 lớp **Form → Service → Db**.
Mở bằng **Visual Studio 2026** (Community 18.x, workload *.NET desktop development*).

## Chạy project

1. Mở SQL Server Management Studio, chạy `Database/QuanLyChoThueVanPhong.sql` (tạo CSDL + dữ liệu mẫu).
2. Mở `QuanLyChoThueVanPhong.sln` bằng Visual Studio 2026.
3. Sửa chuỗi kết nối trong `App.config` cho đúng máy (mặc định `BEHIHI\MSSQLSERVER01`, user `sa`).
4. Nhấn **F5**.

## Chức năng

| Form | Nghiệp vụ |
|---|---|
| FrmDanhMuc | Tầng (hệ số giá theo độ cao), phòng (giá thuê tự tính), khách hàng, dịch vụ |
| FrmHopDong | Lập hợp đồng nhiều phòng (≥ 6 tháng, thu trước 6 tháng trong cùng transaction); thanh lý hợp đồng |
| FrmThanhToanThue | Thu tiền thuê hằng tháng từ kỳ 7, vào đầu mỗi tháng |
| FrmHoaDonDichVu | Lập hóa đơn dịch vụ cuối tháng (điện, nước, internet...), thanh toán hóa đơn |
| FrmGiaHan | Lập phiếu gia hạn cho từng phòng: ≤ 1/2 thời gian thuê, chỉ khi không nợ tiền thuê và đã thanh toán đủ dịch vụ |
| FrmNhanVienTheXe | Công ty báo cáo nhân viên mới (có hình ảnh), cấp / hủy thẻ giữ xe |
| FrmKiemTraPhat | Kiểm tra phòng mỗi năm một lần, phiếu phạt thay đổi kết cấu |

## Quy tắc chính

```
Giá thuê phòng / tháng = diện tích × 250.000 đ/m² × hệ số tầng
Kỳ k = tháng thứ k của hợp đồng; kỳ 1 - 6 trả trước khi ký; từ kỳ 7 trả đầu mỗi tháng
Tiền thuê kỳ k = tổng giá các phòng còn hiệu lực vào ngày đầu kỳ (mỗi phòng có thể gia hạn riêng)
Gia hạn: số tháng ≤ SoThang / 2; không nợ kỳ nào đến hạn; không còn hóa đơn dịch vụ chưa thanh toán
```

## Sơ đồ UML (draw.io)

Thư mục `../UML` chứa 8 file `.drawio` đã xếp bố cục sẵn. Mở draw.io → **File → Open from → Device**.

| File | Câu |
|---|---|
| 01_UseCase_TongQuat.drawio | Câu 3 |
| 02_UseCase_HopDong_ThanhToan.drawio | Câu 4 |
| 03_ClassDiagram.drawio | Câu 1, Câu 2 |
| 04_Activity_HopDong_ThanhToan.drawio | Câu 5 |
| 05_Sequence_LapPhieuGiaHan.drawio | Câu 6 |
| 06_Sequence_ThuTienThueThang.drawio | Bổ sung |
| 07_State_Phong_HopDong_HoaDon.drawio | Bổ sung |
| 08_ERD_CSDL.drawio | Lược đồ CSDL |
