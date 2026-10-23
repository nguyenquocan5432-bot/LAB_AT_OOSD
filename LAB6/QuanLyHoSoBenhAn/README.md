# Quan ly Ho so Benh an Benh nhan (Bai 5)

Ung dung WinForms (.NET, C#) quan ly ho so benh an: dang ky benh nhan, cap ho so,
lap phieu kham, ghi trieu chung, chi dinh/tra ket qua xet nghiem, lap toa thuoc,
dat lich kham dinh ky va thanh toan vien phi/xuat vien.

## Kien truc

3 lop: **Forms** (GUI) → **Services** (BUS - nghiep vu) → **Data/Db.cs** (DAL - ADO.NET) → **SQL Server**.
Cac nghiep vu ghi nhieu bang (dang ky benh nhan, lap phieu kham kem trieu chung, lap toa thuoc
kem chi tiet) deu duoc boc trong `SqlTransaction` o tang Services de dam bao atomic.

## Cau truc thu muc

```
QuanLyHoSoBenhAn/
├─ Data/Db.cs                       # Ket noi + Query/Execute/Scalar + P() xu ly null
├─ Services/
│   ├─ Models.cs                    # KetQuaXuLy, ChiTietToaItem, hang so QuyDinh
│   ├─ DanhMucService.cs            # Khoa, Bac si, Thuoc
│   ├─ BenhNhanService.cs           # Dang ky (transaction), dong le phi, tra cuu, lich su
│   ├─ PhieuKhamService.cs          # Lap phieu kham + trieu chung (transaction), xet nghiem
│   ├─ ToaThuocService.cs           # Lap toa thuoc + chi tiet (transaction)
│   ├─ LichHenService.cs            # Dat/huy/hoan tat lich kham dinh ky
│   └─ ThanhToanService.cs          # Thanh toan vien phi, xac nhan mien phi (dinh ky)
├─ Forms/                           # FrmMain + 7 form nghiep vu (.cs + .Designer.cs)
├─ Database/QuanLyHoSoBenhAn.sql
├─ Program.cs
├─ App.config
└─ QuanLyHoSoBenhAn.csproj / .sln
```

## Quy tac nghiep vu chinh

- Benh nhan lan dau duoc dang ky, dong le phi nam va cap ho so benh an trong **cung 1 giao dich**:
  neu loi o buoc nao, toan bo rollback (khong de ho so mo coi).
- So dien thoai la duy nhat; moi benh nhan chi dong le phi **1 lan / nam** (`UQ_LePhi_BN_Nam`).
- Moi phieu kham chi duoc lap **toi da 1 toa thuoc** (`UQ` tren `ToaThuoc.MaPhieuKham`).
- Kham dinh ky theo quy la **mien phi** — khong the lap phieu thanh toan cho loai kham nay.
- Chi phieu kham loai `DIEU_TRI` moi phat sinh `ThanhToanVienPhi` va chuyen trang thai "Da xuat vien".

## Cau hinh ket noi CSDL

Sua chuoi ket noi trong `App.config` (tag `connectionStrings`) cho dung voi SQL Server
tren may ban, vi du:

```
Data Source=<TEN_SERVER>;Initial Catalog=QuanLyHoSoBenhAn;User ID=sa;Password=<MAT_KHAU>;TrustServerCertificate=True;
```

## Cach chay

1. Tao CSDL: mo `Database/QuanLyHoSoBenhAn.sql` trong SSMS (hoac Azure Data Studio) va Execute.
2. Sua `App.config` cho dung thong tin SQL Server cua ban.
3. Mo `QuanLyHoSoBenhAn.sln` bang Visual Studio 2022 (hoac VS Code voi C# Dev Kit),
   restore NuGet packages, Build va Run (F5) — hoac tu dong lenh:
   ```
   dotnet restore
   dotnet build
   dotnet run
   ```
4. Man hinh chinh (`FrmMain`) co 7 nut mo lan luot cac chuc nang.

## Yeu cau

- .NET SDK 10.0+ (du an dung `net10.0-windows`)
- SQL Server (local hoac instance co san) cho phep SQL Authentication
