/* =========================================================
   CSDL: QuanLyHoSoBenhAn
   Bai 5 - Quan ly ho so benh an benh nhan
   ========================================================= */
IF DB_ID('QuanLyHoSoBenhAn') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyHoSoBenhAn SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyHoSoBenhAn;
END
GO
CREATE DATABASE QuanLyHoSoBenhAn;
GO
USE QuanLyHoSoBenhAn;
GO

/* ================== DANH MUC ================== */
CREATE TABLE Khoa (
    MaKhoa      VARCHAR(10)  PRIMARY KEY,
    TenKhoa     NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE BacSi (
    MaBacSi     VARCHAR(10)  PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    MaKhoa      VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES Khoa(MaKhoa),
    ChuyenMon   NVARCHAR(100) NULL
);
GO

CREATE TABLE Thuoc (
    MaThuoc     VARCHAR(10)  PRIMARY KEY,
    TenThuoc    NVARCHAR(150) NOT NULL,
    DonViTinh   NVARCHAR(30) NOT NULL,
    GhiChu      NVARCHAR(255) NULL
);
GO

/* ================== BENH NHAN / HO SO ================== */
CREATE TABLE BenhNhan (
    MaBN        VARCHAR(10)  PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    DiaChi      NVARCHAR(255) NULL,
    NgaySinh    DATE NOT NULL,
    DienThoai   VARCHAR(20) NOT NULL,
    AnhChupPath NVARCHAR(255) NULL,
    NgayDangKy  DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    CONSTRAINT UQ_BenhNhan_DienThoai UNIQUE (DienThoai)
);
GO

CREATE TABLE LePhiNam (
    MaLePhi     VARCHAR(10)  PRIMARY KEY,
    MaBN        VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES BenhNhan(MaBN),
    NamApDung   INT NOT NULL CHECK (NamApDung >= 2000),
    NgayDong    DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    SoTien      DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    CONSTRAINT UQ_LePhi_BN_Nam UNIQUE (MaBN, NamApDung)
);
GO

CREATE TABLE HoSoBenhAn (
    MaHoSo      VARCHAR(10)  PRIMARY KEY,
    MaBN        VARCHAR(10)  NOT NULL UNIQUE FOREIGN KEY REFERENCES BenhNhan(MaBN),
    NgayCap     DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    TrangThai   NVARCHAR(30) NOT NULL DEFAULT N'Dang hoat dong'
);
GO

/* ================== KHAM BENH ================== */
CREATE TABLE PhieuKhamBenh (
    MaPhieuKham VARCHAR(10)  PRIMARY KEY,
    MaHoSo      VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES HoSoBenhAn(MaHoSo),
    NgayGioKham DATETIME NOT NULL DEFAULT GETDATE(),
    MaBacSi     VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES BacSi(MaBacSi),
    MaKhoa      VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES Khoa(MaKhoa),
    LoaiKham    VARCHAR(10)  NOT NULL CHECK (LoaiKham IN ('DIEU_TRI','DINH_KY')),
    KetLuan     NVARCHAR(500) NULL,
    TrangThai   NVARCHAR(30) NOT NULL DEFAULT N'Dang kham'
);
GO

CREATE TABLE TrieuChung (
    MaTrieuChung VARCHAR(15) PRIMARY KEY,
    MaPhieuKham VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES PhieuKhamBenh(MaPhieuKham),
    TenTrieuChung NVARCHAR(150) NOT NULL,
    ThanNhiet   DECIMAL(4,1) NULL CHECK (ThanNhiet IS NULL OR ThanNhiet BETWEEN 30 AND 45),
    HuyetAp     VARCHAR(20) NULL,
    MoTaThem    NVARCHAR(255) NULL
);
GO

CREATE TABLE XetNghiem (
    MaXetNghiem VARCHAR(10) PRIMARY KEY,
    MaPhieuKham VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES PhieuKhamBenh(MaPhieuKham),
    LoaiXetNghiem NVARCHAR(100) NOT NULL,
    KetQua      NVARCHAR(500) NULL,
    NgayCoKetQua DATETIME NULL
);
GO

CREATE TABLE ToaThuoc (
    MaToa       VARCHAR(15) PRIMARY KEY,
    MaPhieuKham VARCHAR(10) NOT NULL UNIQUE FOREIGN KEY REFERENCES PhieuKhamBenh(MaPhieuKham),
    NgayLap     DATETIME NOT NULL DEFAULT GETDATE(),
    MaBacSi     VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES BacSi(MaBacSi),
    GhiChu      NVARCHAR(255) NULL
);
GO

CREATE TABLE ChiTietToaThuoc (
    MaCT        INT IDENTITY(1,1) PRIMARY KEY,
    MaToa       VARCHAR(15) NOT NULL FOREIGN KEY REFERENCES ToaThuoc(MaToa),
    MaThuoc     VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    LieuDung    NVARCHAR(100) NOT NULL,
    SoLuong     INT NOT NULL CHECK (SoLuong > 0),
    CONSTRAINT UQ_ChiTiet_Toa_Thuoc UNIQUE (MaToa, MaThuoc)
);
GO

CREATE TABLE LichHenDinhKy (
    MaLichHen   VARCHAR(10) PRIMARY KEY,
    MaHoSo      VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES HoSoBenhAn(MaHoSo),
    NgayHen     DATE NOT NULL,
    GioHen      TIME NOT NULL,
    TrangThai   NVARCHAR(30) NOT NULL DEFAULT N'Da dat lich',
    MaPhieuKham VARCHAR(10) NULL FOREIGN KEY REFERENCES PhieuKhamBenh(MaPhieuKham)
);
GO

CREATE TABLE ThanhToanVienPhi (
    MaThanhToan VARCHAR(10) PRIMARY KEY,
    MaPhieuKham VARCHAR(10) NOT NULL UNIQUE FOREIGN KEY REFERENCES PhieuKhamBenh(MaPhieuKham),
    SoTien      DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE()
);
GO

/* ================================================================
   DU LIEU MAU
   ================================================================ */
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
('K01', N'Noi tong quat'),
('K02', N'Tai mui hong'),
('K03', N'Nhi khoa');
GO

INSERT INTO BacSi (MaBacSi, HoTen, MaKhoa, ChuyenMon) VALUES
('BS01', N'Nguyen Van Minh', 'K01', N'Noi khoa'),
('BS02', N'Tran Thi Lan', 'K02', N'Tai mui hong'),
('BS03', N'Le Van Hung', 'K03', N'Nhi khoa');
GO

INSERT INTO Thuoc (MaThuoc, TenThuoc, DonViTinh, GhiChu) VALUES
('TH01', N'Paracetamol 500mg', N'Vien', N'Ha sot, giam dau'),
('TH02', N'Amoxicillin 500mg', N'Vien', N'Khang sinh'),
('TH03', N'Vitamin C 1000mg', N'Vien', N'Bo sung vitamin');
GO

INSERT INTO BenhNhan (MaBN, HoTen, DiaChi, NgaySinh, DienThoai, AnhChupPath, NgayDangKy) VALUES
('BN001', N'Pham Thi Hoa', N'12 Le Loi, Q1, TP.HCM', '1990-03-12', '0909111222', NULL, '2026-09-01'),
('BN002', N'Vo Van Nam', N'45 Nguyen Trai, Q5, TP.HCM', '1985-07-20', '0909333444', NULL, '2026-09-10');
GO

INSERT INTO LePhiNam (MaLePhi, MaBN, NamApDung, NgayDong, SoTien) VALUES
('LP001', 'BN001', 2026, '2026-09-01', 200000),
('LP002', 'BN002', 2026, '2026-09-10', 200000);
GO

INSERT INTO HoSoBenhAn (MaHoSo, MaBN, NgayCap, TrangThai) VALUES
('HS001', 'BN001', '2026-09-01', N'Dang hoat dong'),
('HS002', 'BN002', '2026-09-10', N'Dang hoat dong');
GO

INSERT INTO PhieuKhamBenh (MaPhieuKham, MaHoSo, NgayGioKham, MaBacSi, MaKhoa, LoaiKham, KetLuan, TrangThai) VALUES
('PK001', 'HS001', '2026-10-01 08:30', 'BS01', 'K01', 'DIEU_TRI', N'Cam cum thong thuong', N'Da lap toa'),
('PK002', 'HS002', '2026-10-05 09:00', 'BS02', 'K02', 'DIEU_TRI', N'Viem hong', N'Dang kham');
GO

INSERT INTO TrieuChung (MaTrieuChung, MaPhieuKham, TenTrieuChung, ThanNhiet, HuyetAp, MoTaThem) VALUES
('TCPK001', 'PK001', N'Nhuc dau, so mui', 38.2, '120/80', N'Met moi nhe'),
('TCPK002', 'PK002', N'Dau hong, ho', 37.5, '110/75', NULL);
GO

INSERT INTO ToaThuoc (MaToa, MaPhieuKham, NgayLap, MaBacSi, GhiChu) VALUES
('TOA001', 'PK001', '2026-10-01 09:00', 'BS01', N'Uong sau an');
GO

INSERT INTO ChiTietToaThuoc (MaToa, MaThuoc, LieuDung, SoLuong) VALUES
('TOA001', 'TH01', N'2 lan/ngay, 1 vien/lan', 10),
('TOA001', 'TH03', N'1 lan/ngay, 1 vien/lan', 5);
GO

INSERT INTO ThanhToanVienPhi (MaThanhToan, MaPhieuKham, SoTien, NgayThanhToan) VALUES
('TT001', 'PK001', 150000, '2026-10-01 09:30');
GO

INSERT INTO LichHenDinhKy (MaLichHen, MaHoSo, NgayHen, GioHen, TrangThai) VALUES
('LH001', 'HS001', '2026-12-15', '09:00', N'Da dat lich');
GO
