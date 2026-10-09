-- Chạy toàn bộ file bằng SSMS hoặc sqlcmd.
-- Tạo CSDL, nâng cấp cấu trúc và thêm dữ liệu mẫu; không xóa dữ liệu đã có.

-- 1. KHỞI TẠO CƠ SỞ DỮ LIỆU
-- SQL Server 2016 SP1 trở lên. Chạy toàn bộ file bằng SSMS hoặc sqlcmd.
-- Có thể chạy lại; không xóa dữ liệu đã có.
USE master;
GO
IF DB_ID(N'QuanLyCongTyDuLich') IS NULL
    EXEC(N'CREATE DATABASE [QuanLyCongTyDuLich]');
GO
USE QuanLyCongTyDuLich;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
GO

IF OBJECT_ID(N'dbo.Tour',N'U') IS NULL
CREATE TABLE dbo.Tour (
    MaTour VARCHAR(20) NOT NULL CONSTRAINT PK_Tour PRIMARY KEY,
    TenTour NVARCHAR(200) NOT NULL,
    SoNgay INT NOT NULL CONSTRAINT CK_Tour_SoNgay CHECK(SoNgay>0),
    SoDem INT NOT NULL CONSTRAINT CK_Tour_SoDem CHECK(SoDem>=0),
    DonGiaKhach DECIMAL(18,2) NOT NULL CONSTRAINT CK_Tour_DonGia CHECK(DonGiaKhach>=0),
    MoTa NVARCHAR(MAX) NULL,
    DangMoBan BIT NOT NULL CONSTRAINT DF_Tour_DangMoBan DEFAULT(1)
);

IF OBJECT_ID(N'dbo.PhuongTien',N'U') IS NULL
CREATE TABLE dbo.PhuongTien (
    MaPT VARCHAR(20) NOT NULL CONSTRAINT PK_PhuongTien PRIMARY KEY,
    TenPT NVARCHAR(100) NOT NULL
);

IF OBJECT_ID(N'dbo.DiemThamQuan',N'U') IS NULL
CREATE TABLE dbo.DiemThamQuan (
    MaDiemTQ VARCHAR(20) NOT NULL CONSTRAINT PK_DiemThamQuan PRIMARY KEY,
    TenDiemTQ NVARCHAR(200) NOT NULL,
    DiaDiem NVARCHAR(250) NOT NULL,
    NoiDung NVARCHAR(MAX) NULL,
    YNghia NVARCHAR(MAX) NULL
);

IF OBJECT_ID(N'dbo.DiemDung',N'U') IS NULL
CREATE TABLE dbo.DiemDung (
    MaTour VARCHAR(20) NOT NULL,
    ThuTu INT NOT NULL CONSTRAINT CK_DiemDung_ThuTu CHECK(ThuTu>0),
    TenDiemDung NVARCHAR(200) NOT NULL,
    DoiPhuongTien BIT NOT NULL CONSTRAINT DF_DiemDung_DoiPhuongTien DEFAULT(0),
    CoNoiAn BIT NOT NULL CONSTRAINT DF_DiemDung_CoNoiAn DEFAULT(0),
    CoKhachSan BIT NOT NULL CONSTRAINT DF_DiemDung_CoKhachSan DEFAULT(0),
    HangSaoKhachSan TINYINT NULL,
    CONSTRAINT PK_DiemDung PRIMARY KEY(MaTour,ThuTu),
    CONSTRAINT FK_DiemDung_Tour FOREIGN KEY(MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT CK_DiemDung_HangSao CHECK(
        (CoKhachSan=1 AND HangSaoKhachSan IS NOT NULL AND HangSaoKhachSan BETWEEN 2 AND 5)
        OR (CoKhachSan=0 AND HangSaoKhachSan IS NULL))
);

IF OBJECT_ID(N'dbo.ChangPhuongTien',N'U') IS NULL
CREATE TABLE dbo.ChangPhuongTien (
    MaTour VARCHAR(20) NOT NULL,
    ThuTuChang INT NOT NULL CONSTRAINT CK_ChangPhuongTien_ThuTu CHECK(ThuTuChang>0),
    MaPT VARCHAR(20) NOT NULL,
    GhiChu NVARCHAR(300) NULL,
    CONSTRAINT PK_ChangPhuongTien PRIMARY KEY(MaTour,ThuTuChang,MaPT),
    CONSTRAINT FK_ChangPhuongTien_Tour FOREIGN KEY(MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT FK_ChangPhuongTien_PhuongTien FOREIGN KEY(MaPT) REFERENCES dbo.PhuongTien(MaPT)
);

IF OBJECT_ID(N'dbo.TourDiemThamQuan',N'U') IS NULL
CREATE TABLE dbo.TourDiemThamQuan (
    MaTour VARCHAR(20) NOT NULL,
    MaDiemTQ VARCHAR(20) NOT NULL,
    ThuTu INT NOT NULL CONSTRAINT CK_TourDiemThamQuan_ThuTu CHECK(ThuTu>0),
    CONSTRAINT PK_TourDiemThamQuan PRIMARY KEY(MaTour,MaDiemTQ),
    CONSTRAINT UQ_TourDiemThamQuan_ThuTu UNIQUE(MaTour,ThuTu),
    CONSTRAINT FK_TourDiemThamQuan_Tour FOREIGN KEY(MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT FK_TourDiemThamQuan_DiemThamQuan FOREIGN KEY(MaDiemTQ) REFERENCES dbo.DiemThamQuan(MaDiemTQ)
);

IF OBJECT_ID(N'dbo.DoanKhach',N'U') IS NULL
CREATE TABLE dbo.DoanKhach (
    MaDoan VARCHAR(20) NOT NULL CONSTRAINT PK_DoanKhach PRIMARY KEY,
    TenCoQuanDaiDien NVARCHAR(200) NOT NULL,
    DiaChi NVARCHAR(250) NOT NULL,
    DienThoai VARCHAR(20) NOT NULL,
    NguoiDaiDien NVARCHAR(120) NOT NULL
);

IF OBJECT_ID(N'dbo.HuongDanVien',N'U') IS NULL
CREATE TABLE dbo.HuongDanVien (
    MaHDV VARCHAR(20) NOT NULL CONSTRAINT PK_HuongDanVien PRIMARY KEY,
    HoTen NVARCHAR(120) NOT NULL,
    DienThoai VARCHAR(20) NULL,
    LuongCoBan DECIMAL(18,2) NOT NULL CONSTRAINT CK_HuongDanVien_Luong CHECK(LuongCoBan>=0)
);

IF OBJECT_ID(N'dbo.DiemBanVe',N'U') IS NULL
CREATE TABLE dbo.DiemBanVe (
    MaDiemBan VARCHAR(20) NOT NULL CONSTRAINT PK_DiemBanVe PRIMARY KEY,
    TenDiemBan NVARCHAR(200) NOT NULL,
    DiaChi NVARCHAR(250) NOT NULL,
    DienThoai VARCHAR(20) NULL
);

IF OBJECT_ID(N'dbo.ChuyenLe',N'U') IS NULL
CREATE TABLE dbo.ChuyenLe (
    MaChuyen VARCHAR(20) NOT NULL CONSTRAINT PK_ChuyenLe PRIMARY KEY,
    MaTour VARCHAR(20) NOT NULL,
    NgayDi DATE NOT NULL,
    NgayVe DATE NOT NULL,
    DiaDiemDon NVARCHAR(250) NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL CONSTRAINT DF_ChuyenLe_TrangThai DEFAULT(N'Mở đăng ký'),
    CONSTRAINT FK_ChuyenLe_Tour FOREIGN KEY(MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT CK_ChuyenLe_Ngay CHECK(NgayVe>=NgayDi)
);

IF OBJECT_ID(N'dbo.DangKyLe',N'U') IS NULL
CREATE TABLE dbo.DangKyLe (
    SoPhieu VARCHAR(20) NOT NULL CONSTRAINT PK_DangKyLe PRIMARY KEY,
    MaChuyen VARCHAR(20) NOT NULL,
    MaDiemBan VARCHAR(20) NOT NULL,
    NgayDangKy DATETIME2(0) NOT NULL CONSTRAINT DF_DangKyLe_Ngay DEFAULT(SYSDATETIME()),
    TenNguoiDangKy NVARCHAR(120) NOT NULL,
    DienThoai VARCHAR(20) NOT NULL,
    SoNguoi INT NOT NULL CONSTRAINT CK_DangKyLe_SoNguoi CHECK(SoNguoi BETWEEN 1 AND 11),
    ThanhTien DECIMAL(18,2) NOT NULL CONSTRAINT CK_DangKyLe_ThanhTien CHECK(ThanhTien>=0),
    DaThanhToan BIT NOT NULL CONSTRAINT DF_DangKyLe_DaThanhToan DEFAULT(1),
    TrangThai NVARCHAR(50) NOT NULL CONSTRAINT DF_DangKyLe_TrangThai DEFAULT(N'Đã đăng ký'),
    CONSTRAINT FK_DangKyLe_ChuyenLe FOREIGN KEY(MaChuyen) REFERENCES dbo.ChuyenLe(MaChuyen),
    CONSTRAINT FK_DangKyLe_DiemBanVe FOREIGN KEY(MaDiemBan) REFERENCES dbo.DiemBanVe(MaDiemBan),
    CONSTRAINT CK_DangKyLe_DaThanhToan CHECK(DaThanhToan=1)
);

IF OBJECT_ID(N'dbo.DangKyDoan',N'U') IS NULL
CREATE TABLE dbo.DangKyDoan (
    SoPhieu VARCHAR(20) NOT NULL CONSTRAINT PK_DangKyDoan PRIMARY KEY,
    MaDoan VARCHAR(20) NOT NULL,
    MaTour VARCHAR(20) NOT NULL,
    NgayDangKy DATETIME2(0) NOT NULL CONSTRAINT DF_DangKyDoan_Ngay DEFAULT(SYSDATETIME()),
    SoNguoi INT NOT NULL CONSTRAINT CK_DangKyDoan_SoNguoi CHECK(SoNguoi>12),
    NgayDi DATE NOT NULL,
    NgayKetThucDuKien DATE NOT NULL,
    DiaDiemDon NVARCHAR(250) NOT NULL,
    MuaBaoHiem BIT NOT NULL CONSTRAINT DF_DangKyDoan_BaoHiem DEFAULT(0),
    TienCoc DECIMAL(18,2) NOT NULL,
    TongTienDuKien DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL CONSTRAINT DF_DangKyDoan_TrangThai DEFAULT(N'Đã đăng ký'),
    CONSTRAINT FK_DangKyDoan_DoanKhach FOREIGN KEY(MaDoan) REFERENCES dbo.DoanKhach(MaDoan),
    CONSTRAINT FK_DangKyDoan_Tour FOREIGN KEY(MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT CK_DangKyDoan_TienCoc CHECK(TienCoc>0 AND TienCoc<=TongTienDuKien),
    CONSTRAINT CK_DangKyDoan_Ngay CHECK(NgayKetThucDuKien>=NgayDi)
);

IF OBJECT_ID(N'dbo.ThanhVienDoan',N'U') IS NULL
CREATE TABLE dbo.ThanhVienDoan (
    SoPhieu VARCHAR(20) NOT NULL,
    STT INT NOT NULL CONSTRAINT CK_ThanhVienDoan_STT CHECK(STT>0),
    HoTen NVARCHAR(120) NOT NULL,
    NgaySinh DATE NULL,
    SoGiayTo NVARCHAR(40) NULL,
    CONSTRAINT PK_ThanhVienDoan PRIMARY KEY(SoPhieu,STT),
    CONSTRAINT FK_ThanhVienDoan_DangKyDoan FOREIGN KEY(SoPhieu) REFERENCES dbo.DangKyDoan(SoPhieu)
);

IF OBJECT_ID(N'dbo.PhanCongHDV',N'U') IS NULL
CREATE TABLE dbo.PhanCongHDV (
    MaPC VARCHAR(20) NOT NULL CONSTRAINT PK_PhanCongHDV PRIMARY KEY,
    MaHDV VARCHAR(20) NOT NULL,
    MaChuyen VARCHAR(20) NULL,
    SoPhieuDoan VARCHAR(20) NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    ThuLaoTour DECIMAL(18,2) NOT NULL CONSTRAINT CK_PhanCongHDV_ThuLao CHECK(ThuLaoTour>=0),
    CONSTRAINT FK_PhanCongHDV_HuongDanVien FOREIGN KEY(MaHDV) REFERENCES dbo.HuongDanVien(MaHDV),
    CONSTRAINT FK_PhanCongHDV_ChuyenLe FOREIGN KEY(MaChuyen) REFERENCES dbo.ChuyenLe(MaChuyen),
    CONSTRAINT FK_PhanCongHDV_DangKyDoan FOREIGN KEY(SoPhieuDoan) REFERENCES dbo.DangKyDoan(SoPhieu),
    CONSTRAINT CK_PhanCongHDV_DoiTuong CHECK(
        (MaChuyen IS NOT NULL AND SoPhieuDoan IS NULL)
        OR (MaChuyen IS NULL AND SoPhieuDoan IS NOT NULL)),
    CONSTRAINT CK_PhanCongHDV_Ngay CHECK(NgayKetThuc>=NgayBatDau)
);

IF OBJECT_ID(N'dbo.ThanhToanDoan',N'U') IS NULL
CREATE TABLE dbo.ThanhToanDoan (
    SoTT VARCHAR(20) NOT NULL CONSTRAINT PK_ThanhToanDoan PRIMARY KEY,
    SoPhieuDoan VARCHAR(20) NOT NULL,
    NgayThanhToan DATETIME2(0) NOT NULL CONSTRAINT DF_ThanhToanDoan_Ngay DEFAULT(SYSDATETIME()),
    SoTien DECIMAL(18,2) NOT NULL CONSTRAINT CK_ThanhToanDoan_SoTien CHECK(SoTien>0),
    GhiChu NVARCHAR(300) NULL,
    CONSTRAINT FK_ThanhToanDoan_DangKyDoan FOREIGN KEY(SoPhieuDoan) REFERENCES dbo.DangKyDoan(SoPhieu)
);

IF OBJECT_ID(N'dbo.KhaoSat',N'U') IS NULL
CREATE TABLE dbo.KhaoSat (
    MaKhaoSat VARCHAR(20) NOT NULL CONSTRAINT PK_KhaoSat PRIMARY KEY,
    SoPhieuLe VARCHAR(20) NULL,
    SoPhieuDoan VARCHAR(20) NULL,
    NgayGui DATE NOT NULL,
    NgayPhanHoi DATE NULL,
    DiemDanhGia TINYINT NULL,
    GopY NVARCHAR(1500) NULL,
    CONSTRAINT FK_KhaoSat_DangKyLe FOREIGN KEY(SoPhieuLe) REFERENCES dbo.DangKyLe(SoPhieu),
    CONSTRAINT FK_KhaoSat_DangKyDoan FOREIGN KEY(SoPhieuDoan) REFERENCES dbo.DangKyDoan(SoPhieu),
    CONSTRAINT CK_KhaoSat_DoiTuong CHECK(
        (SoPhieuLe IS NOT NULL AND SoPhieuDoan IS NULL)
        OR (SoPhieuLe IS NULL AND SoPhieuDoan IS NOT NULL)),
    CONSTRAINT CK_KhaoSat_Diem CHECK(DiemDanhGia IS NULL OR DiemDanhGia BETWEEN 1 AND 5),
    CONSTRAINT CK_KhaoSat_Ngay CHECK(NgayPhanHoi IS NULL OR NgayPhanHoi>=NgayGui)
);
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.PhanCongHDV') AND name=N'UX_PhanCongHDV_ChuyenLe')
    CREATE UNIQUE INDEX UX_PhanCongHDV_ChuyenLe ON dbo.PhanCongHDV(MaChuyen) WHERE MaChuyen IS NOT NULL;
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.PhanCongHDV') AND name=N'IX_PhanCongHDV_Lich')
    CREATE INDEX IX_PhanCongHDV_Lich ON dbo.PhanCongHDV(MaHDV,NgayBatDau,NgayKetThuc);
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.KhaoSat') AND name=N'UX_KhaoSat_DangKyLe')
    CREATE UNIQUE INDEX UX_KhaoSat_DangKyLe ON dbo.KhaoSat(SoPhieuLe) WHERE SoPhieuLe IS NOT NULL;
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.KhaoSat') AND name=N'UX_KhaoSat_DangKyDoan')
    CREATE UNIQUE INDEX UX_KhaoSat_DangKyDoan ON dbo.KhaoSat(SoPhieuDoan) WHERE SoPhieuDoan IS NOT NULL;
GO

CREATE OR ALTER TRIGGER dbo.TR_PhanCongHDV_KhongTrungLich
ON dbo.PhanCongHDV
AFTER INSERT,UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NOT EXISTS(SELECT 1 FROM inserted) RETURN;

    DECLARE @SoHDV BIGINT;
    SELECT @SoHDV=COUNT_BIG(*)
    FROM dbo.HuongDanVien AS h WITH(UPDLOCK,HOLDLOCK)
    JOIN (SELECT DISTINCT MaHDV FROM inserted) AS i ON i.MaHDV=h.MaHDV;

    IF EXISTS(
        SELECT 1 FROM inserted AS i
        JOIN dbo.PhanCongHDV AS p WITH(UPDLOCK,HOLDLOCK)
          ON p.MaHDV=i.MaHDV AND p.MaPC<>i.MaPC
         AND p.NgayBatDau<=i.NgayKetThuc AND i.NgayBatDau<=p.NgayKetThuc
    )
    BEGIN
        ;THROW 51001,N'Hướng dẫn viên đã có lịch phân công trùng thời gian.',1;
    END;
END;
GO

-- 2. NÂNG CẤP CẤU TRÚC CŨ
-- Bổ sung cột cho CSDL phiên bản cũ; giữ nguyên dữ liệu.
USE QuanLyCongTyDuLich;
GO
IF COL_LENGTH(N'dbo.ThanhToanDoan',N'GhiChu') IS NULL
    ALTER TABLE dbo.ThanhToanDoan ADD GhiChu NVARCHAR(300) NULL;
GO

-- 3. DỮ LIỆU MẪU
-- Dữ liệu minh họa; không ghi đè dữ liệu đã có.
USE QuanLyCongTyDuLich;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
SET XACT_ABORT ON;
GO
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @NgayDi DATE=DATEADD(DAY,10,CAST(GETDATE() AS DATE));

    IF NOT EXISTS(SELECT 1 FROM dbo.Tour WHERE MaTour='T001')
        INSERT dbo.Tour(MaTour,TenTour,SoNgay,SoDem,DonGiaKhach,MoTa,DangMoBan)
        VALUES('T001',N'TP.HCM - Đà Nẵng - TP.HCM',5,4,2000000,N'Tour minh họa',1);
    IF NOT EXISTS(SELECT 1 FROM dbo.Tour WHERE MaTour='T002')
        INSERT dbo.Tour(MaTour,TenTour,SoNgay,SoDem,DonGiaKhach,MoTa,DangMoBan)
        VALUES('T002',N'Tour đóng bán để kiểm thử',5,4,2000000,N'Dữ liệu kiểm thử',0);
    IF NOT EXISTS(SELECT 1 FROM dbo.PhuongTien WHERE MaPT='PT001')
        INSERT dbo.PhuongTien VALUES('PT001',N'Máy bay');
    IF NOT EXISTS(SELECT 1 FROM dbo.DiemThamQuan WHERE MaDiemTQ='DTQ001')
        INSERT dbo.DiemThamQuan VALUES('DTQ001',N'Bà Nà Hills',N'Đà Nẵng',N'Tham quan',N'Du lịch');
    IF NOT EXISTS(SELECT 1 FROM dbo.DiemDung WHERE MaTour='T001' AND ThuTu=1)
        INSERT dbo.DiemDung VALUES('T001',1,N'Đà Nẵng',0,1,1,3);
    IF NOT EXISTS(SELECT 1 FROM dbo.DiemDung WHERE MaTour='T001' AND ThuTu=2)
        INSERT dbo.DiemDung VALUES('T001',2,N'TP.HCM',0,0,0,NULL);
    IF NOT EXISTS(SELECT 1 FROM dbo.ChangPhuongTien WHERE MaTour='T001' AND ThuTuChang=1 AND MaPT='PT001')
        INSERT dbo.ChangPhuongTien VALUES('T001',1,'PT001',N'TP.HCM đến Đà Nẵng');
    IF NOT EXISTS(SELECT 1 FROM dbo.ChangPhuongTien WHERE MaTour='T001' AND ThuTuChang=2 AND MaPT='PT001')
        INSERT dbo.ChangPhuongTien VALUES('T001',2,'PT001',N'Đà Nẵng về TP.HCM');
    IF NOT EXISTS(SELECT 1 FROM dbo.TourDiemThamQuan WHERE MaTour='T001' AND MaDiemTQ='DTQ001')
        INSERT dbo.TourDiemThamQuan VALUES('T001','DTQ001',1);

    IF NOT EXISTS(SELECT 1 FROM dbo.DoanKhach WHERE MaDoan='D001')
        INSERT dbo.DoanKhach VALUES('D001',N'Công ty An Phú',N'10 Nguyễn Huệ, TP.HCM','0900000000',N'Nguyễn Văn An');
    IF NOT EXISTS(SELECT 1 FROM dbo.HuongDanVien WHERE MaHDV='H001')
        INSERT dbo.HuongDanVien VALUES('H001',N'HDV mẫu 01','0900000001',8000000);
    IF NOT EXISTS(SELECT 1 FROM dbo.HuongDanVien WHERE MaHDV='H002')
        INSERT dbo.HuongDanVien VALUES('H002',N'HDV mẫu 02','0900000002',9000000);
    IF NOT EXISTS(SELECT 1 FROM dbo.HuongDanVien WHERE MaHDV='H003')
        INSERT dbo.HuongDanVien VALUES('H003',N'HDV mẫu 03','0900000003',8500000);
    IF NOT EXISTS(SELECT 1 FROM dbo.DiemBanVe WHERE MaDiemBan='DBV001')
        INSERT dbo.DiemBanVe VALUES('DBV001',N'Điểm bán mẫu',N'TP.HCM','0900000000');

    IF NOT EXISTS(SELECT 1 FROM dbo.DangKyDoan WHERE SoPhieu='DK_DEMO')
        INSERT dbo.DangKyDoan(SoPhieu,MaDoan,MaTour,SoNguoi,NgayDi,NgayKetThucDuKien,DiaDiemDon,MuaBaoHiem,TienCoc,TongTienDuKien)
        VALUES('DK_DEMO','D001','T001',15,@NgayDi,DATEADD(DAY,4,@NgayDi),N'TP.HCM',0,10000000,30000000);
    IF NOT EXISTS(SELECT 1 FROM dbo.ChuyenLe WHERE MaChuyen='C001')
        INSERT dbo.ChuyenLe(MaChuyen,MaTour,NgayDi,NgayVe,DiaDiemDon)
        VALUES('C001','T001',DATEADD(DAY,10,@NgayDi),DATEADD(DAY,14,@NgayDi),N'TP.HCM');
    IF NOT EXISTS(SELECT 1 FROM dbo.PhanCongHDV WHERE MaPC='PC_DEMO')
        INSERT dbo.PhanCongHDV(MaPC,MaHDV,MaChuyen,SoPhieuDoan,NgayBatDau,NgayKetThuc,ThuLaoTour)
        SELECT 'PC_DEMO','H001',NULL,SoPhieu,NgayDi,NgayKetThucDuKien,1000000
        FROM dbo.DangKyDoan WHERE SoPhieu='DK_DEMO';

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK;
    THROW;
END CATCH;
GO
