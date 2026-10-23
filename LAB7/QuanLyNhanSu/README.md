# Bài 7 – Quản lý nhân sự (Công ty sơn Đông Á)

C# WinForms (.NET 10) · SQL Server · ADO.NET – kiến trúc 3 lớp **Form → Service → Db**.
Mở bằng **Visual Studio 2026** (Community 18.x, workload *.NET desktop development*).

## Chạy project

1. Mở SQL Server Management Studio, chạy `Database/QuanLyNhanSu.sql` (tạo CSDL `QuanLyNhanSu` + dữ liệu mẫu).
2. Mở `QuanLyNhanSu.sln` bằng Visual Studio 2026.
3. Sửa chuỗi kết nối trong `App.config` cho đúng máy (mặc định `BEHIHI\MSSQLSERVER01`, user `sa`).
4. Nhấn **F5**.

## Cấu trúc

| Thư mục | Nội dung |
|---|---|
| `Data/Db.cs` | Truy cập dữ liệu: Query, Execute, Scalar, các hàm chạy trong SqlTransaction |
| `Services/` | Nghiệp vụ: TuyenDung, HopDong, QuyetDinh, ChamCong, KhenThuong, Luong, DanhMuc |
| `Forms/` | Giao diện: FrmMain + 8 form chức năng |
| `Database/` | Script tạo CSDL (19 bảng) và dữ liệu mẫu |
| `../UML/` | Mã PlantUML của 8 sơ đồ (dán vào draw.io) |

## Chức năng

| Form | Nghiệp vụ |
|---|---|
| FrmDanhMuc | Phòng ban (nhu cầu tuyển), chức vụ (phụ cấp), ca làm việc, danh sách nhân viên |
| FrmTuyenDung | Lập quyết định tuyển dụng (nhân viên + QĐ + giảm nhu cầu tuyển, 1 transaction); chấm dứt thử việc |
| FrmKyHopDong | Ký hợp đồng chính thức: HĐ + QĐ lương + QĐ chức vụ (nếu có) + hồ sơ BHYT/BHXH (nếu dài hạn), 1 transaction |
| FrmQuyetDinh | QĐ tăng lương, QĐ bổ nhiệm chức vụ, lịch sử quyết định của nhân viên |
| FrmPhanCong | Kế hoạch phân công ca theo tuần của trưởng phòng |
| FrmChamCong | Bảng chấm công tháng, phiếu ghi nhận phụ trội (tăng ca / ngày lễ / ngày nghỉ) |
| FrmKhenThuongKyLuat | QĐ khen thưởng (phiếu chi hoặc cộng lương), QĐ kỷ luật (phiếu thu hoặc trừ lương), cá nhân hoặc tập thể |
| FrmBangLuong | Tính bảng lương tháng |

## Công thức lương

```
Lương theo công = LCB / 26 × số ngày công
Tiền phụ trội   = số giờ phụ trội × (LCB / 26 / 8) × 1.5
Phụ cấp         = PC chức vụ + PC lương + PC độc hại (theo quyết định đang hiệu lực)
Thực lãnh       = lương theo công + phụ cấp + tiền phụ trội + thưởng − phạt − BHXH 8% − BHYT 1.5%
```

Nhân viên thử việc: LCB = lương thử việc, không phụ cấp, không bảo hiểm. Bảo hiểm chỉ trừ với nhân viên có hợp đồng dài hạn.

Kiểm tra với dữ liệu mẫu tháng 9/2026: NV001 = 12.925.538 đ, NV002 = 7.165.384 đ, NV003 (thử việc) = 5.769.231 đ.

## Vẽ sơ đồ UML trên draw.io

Mở https://app.diagrams.net → **Arrange (Sắp xếp) → Insert (Chèn) → Advanced (Nâng cao) → PlantUML…**,
dán toàn bộ nội dung một file `.puml` trong thư mục `LAB7/UML`, nhấn **Insert**.

| File | Câu |
|---|---|
| 01_UseCase_TongQuat.puml | Câu 3 |
| 02_UseCase_ThuViec_HopDong.puml | Câu 4 |
| 03_ClassDiagram.drawio | Câu 1, Câu 2 – mở trực tiếp bằng draw.io (File → Open), đã xếp bố cục sẵn |
| 03_ClassDiagram.puml | Câu 1, Câu 2 – bản PlantUML (tự xếp, đường nối rối hơn) |
| 04_Activity_ThuViec_HopDong.puml | Câu 5 |
| 05_Sequence_LapQuyetDinhKhenThuong.puml | Câu 6 |
| 06_Sequence_TinhBangLuong.puml | Bổ sung |
| 07_State_NhanVien_KhenThuong.puml | Bổ sung |
| 08_ERD_CSDL.drawio | Lược đồ CSDL – mở trực tiếp bằng draw.io, 19 bảng, 24 khóa ngoại, ký hiệu chân chim |
| 08_ERD_CSDL.puml | Lược đồ CSDL – bản PlantUML |
