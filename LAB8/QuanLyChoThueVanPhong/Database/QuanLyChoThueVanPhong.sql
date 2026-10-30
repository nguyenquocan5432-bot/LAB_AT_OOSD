/* =========================================================
   CSDL: QuanLyChoThueVanPhong
   Bai 4 - Quan ly cho thue van phong o cao oc
   ========================================================= */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO
IF DB_ID('QuanLyChoThueVanPhong') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyChoThueVanPhong SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyChoThueVanPhong;
END
GO
CREATE DATABASE QuanLyChoThueVanPhong;
GO
USE QuanLyChoThueVanPhong;
GO

/* ================== TANG - PHONG ================== */
CREATE TABLE Tang (
    MaTang      VARCHAR(10)  PRIMARY KEY,
    TenTang     NVARCHAR(50) NOT NULL,
    SoTang      INT NOT NULL CHECK (SoTang >= 1),
    HeSoGia     DECIMAL(4,2) NOT NULL CHECK (HeSoGia > 0),
    CONSTRAINT UQ_Tang_SoTang UNIQUE (SoTang)
);
GO

CREATE TABLE Phong (
    MaPhong     VARCHAR(10)   PRIMARY KEY,
    MaTang      VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Tang(MaTang),
    DienTich    DECIMAL(8,2)  NOT NULL CHECK (DienTich > 0),
    Huong       NVARCHAR(20)  NOT NULL,
    GiaThue     DECIMAL(18,2) NOT NULL CHECK (GiaThue > 0),
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'Trong'
                CHECK (TrangThai IN ('Trong', 'Dang thue', 'Sua chua'))
);
GO

/* ================== KHACH HANG - HOP DONG ================== */
CREATE TABLE KhachHang (
    MaKH         VARCHAR(20)   PRIMARY KEY,
    TenCongTy    NVARCHAR(150) NOT NULL,
    MaSoThue     VARCHAR(20)   NOT NULL,
    NguoiDaiDien NVARCHAR(100) NOT NULL,
    DienThoai    VARCHAR(20)   NOT NULL,
    Email        VARCHAR(100)  NULL,
    DiaChi       NVARCHAR(255) NULL,
    CONSTRAINT UQ_KhachHang_MST UNIQUE (MaSoThue)
);
GO

CREATE TABLE HopDongThue (
    SoHD        VARCHAR(20)   PRIMARY KEY,
    MaKH        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES KhachHang(MaKH),
    NgayKy      DATE NOT NULL,
    NgayBatDau  DATE NOT NULL,
    SoThang     INT  NOT NULL CHECK (SoThang >= 6),
    NgayKetThuc DATE NOT NULL,
    NoiDung     NVARCHAR(1000) NULL,
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'Hieu luc'
                CHECK (TrangThai IN ('Hieu luc', 'Da thanh ly')),
    CONSTRAINT CK_HD_Ngay CHECK (NgayKetThuc > NgayBatDau AND NgayBatDau >= NgayKy)
);
GO

CREATE TABLE ChiTietHopDong (
    SoHD        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES HopDongThue(SoHD),
    MaPhong     VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Phong(MaPhong),
    GiaThue     DECIMAL(18,2) NOT NULL CHECK (GiaThue > 0),
    NgayBatDau  DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    CONSTRAINT PK_ChiTietHopDong PRIMARY KEY (SoHD, MaPhong),
    CONSTRAINT CK_CTHD_Ngay CHECK (NgayKetThuc > NgayBatDau)
);
GO

CREATE TABLE PhieuGiaHan (
    SoPhieu       VARCHAR(20)   PRIMARY KEY,
    SoHD          VARCHAR(20)   NOT NULL,
    MaPhong       VARCHAR(10)   NOT NULL,
    NgayLap       DATE NOT NULL,
    SoThangGiaHan INT  NOT NULL CHECK (SoThangGiaHan > 0),
    NgayHetHanCu  DATE NOT NULL,
    NgayHetHanMoi DATE NOT NULL,
    NguoiLap      NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_PGH_CTHD FOREIGN KEY (SoHD, MaPhong) REFERENCES ChiTietHopDong(SoHD, MaPhong),
    CONSTRAINT CK_PGH_Ngay CHECK (NgayHetHanMoi > NgayHetHanCu)
);
GO

/* Ky k = thang thu k cua hop dong; ky 1-6 tra truoc khi ky, tu ky 7 tra dau moi thang */
CREATE TABLE PhieuThuTienThue (
    SoPhieu     VARCHAR(20)   PRIMARY KEY,
    SoHD        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES HopDongThue(SoHD),
    TuKy        INT NOT NULL CHECK (TuKy >= 1),
    DenKy       INT NOT NULL,
    SoTien      DECIMAL(18,2) NOT NULL CHECK (SoTien > 0),
    NgayThu     DATE NOT NULL,
    LoaiThu     VARCHAR(10)   NOT NULL CHECK (LoaiThu IN ('TRA_TRUOC', 'HANG_THANG')),
    NguoiThu    NVARCHAR(100) NOT NULL,
    CONSTRAINT CK_PTT_Ky CHECK (DenKy >= TuKy),
    CONSTRAINT UQ_PTT_HD_Ky UNIQUE (SoHD, TuKy)
);
GO

/* ================== DICH VU ================== */
CREATE TABLE DichVu (
    MaDV        VARCHAR(10)   PRIMARY KEY,
    TenDV       NVARCHAR(100) NOT NULL,
    DonViTinh   NVARCHAR(20)  NOT NULL,
    DonGia      DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0)
);
GO

CREATE TABLE HoaDonDichVu (
    MaHoaDon      VARCHAR(20)   PRIMARY KEY,
    SoHD          VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES HopDongThue(SoHD),
    Thang         INT NOT NULL CHECK (Thang BETWEEN 1 AND 12),
    Nam           INT NOT NULL CHECK (Nam >= 2000),
    NgayLap       DATE NOT NULL,
    TongTien      DECIMAL(18,2) NOT NULL CHECK (TongTien >= 0),
    TrangThai     VARCHAR(20)   NOT NULL DEFAULT 'Chua thanh toan'
                  CHECK (TrangThai IN ('Chua thanh toan', 'Da thanh toan')),
    NgayThanhToan DATE NULL,
    CONSTRAINT UQ_HDDV_HD_Thang UNIQUE (SoHD, Thang, Nam)
);
GO

CREATE TABLE ChiTietHoaDonDV (
    MaHoaDon    VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES HoaDonDichVu(MaHoaDon),
    MaDV        VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES DichVu(MaDV),
    SoLuong     DECIMAL(12,2) NOT NULL CHECK (SoLuong > 0),
    DonGia      DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    ThanhTien   DECIMAL(18,2) NOT NULL CHECK (ThanhTien >= 0),
    CONSTRAINT PK_ChiTietHoaDonDV PRIMARY KEY (MaHoaDon, MaDV)
);
GO

/* ================== NHAN VIEN CONG TY - THE GIU XE ================== */
CREATE TABLE NhanVienCongTy (
    MaNV        VARCHAR(20)   PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    CCCD        VARCHAR(12)   NOT NULL,
    HinhAnh     NVARCHAR(255) NULL,
    MaKH        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES KhachHang(MaKH),
    ChucVu      NVARCHAR(100) NOT NULL,
    MaPhong     VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Phong(MaPhong),
    DienThoai   VARCHAR(20)   NULL,
    NgayBaoCao  DATE NOT NULL,
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'Dang lam' CHECK (TrangThai IN ('Dang lam', 'Da nghi')),
    CONSTRAINT UQ_NVCT_CCCD UNIQUE (CCCD)
);
GO

CREATE TABLE TheGiuXe (
    MaThe       VARCHAR(20)   PRIMARY KEY,
    MaNV        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES NhanVienCongTy(MaNV),
    LoaiXe      NVARCHAR(20)  NOT NULL CHECK (LoaiXe IN (N'Xe may', N'O to', N'Xe dap')),
    BienSo      VARCHAR(20)   NULL,
    HieuXe      NVARCHAR(50)  NULL,
    MauXe       NVARCHAR(30)  NULL,
    NgayCap     DATE NOT NULL,
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'Dang dung' CHECK (TrangThai IN ('Dang dung', 'Da huy'))
);
GO
-- Moi nhan vien chi co 1 the dang dung; bien so khong trung giua cac the dang dung
CREATE UNIQUE INDEX UX_The_NV_DangDung ON TheGiuXe(MaNV) WHERE TrangThai = 'Dang dung';
CREATE UNIQUE INDEX UX_The_BienSo_DangDung ON TheGiuXe(BienSo) WHERE TrangThai = 'Dang dung' AND BienSo IS NOT NULL;
GO

/* ================== KIEM TRA PHONG - PHIEU PHAT ================== */
CREATE TABLE KiemTraPhong (
    MaKT         VARCHAR(20)   PRIMARY KEY,
    MaPhong      VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Phong(MaPhong),
    NgayKiemTra  DATE NOT NULL,
    Nam          INT  NOT NULL,
    TinhTrang    NVARCHAR(20)  NOT NULL CHECK (TinhTrang IN (N'Tot', N'Can sua chua', N'Hu hong')),
    MoTa         NVARCHAR(500) NULL,
    KeHoachTuSua NVARCHAR(500) NULL,
    NguoiKiemTra NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_KTP_Phong_Nam UNIQUE (MaPhong, Nam)
);
GO

CREATE TABLE PhieuPhat (
    MaPhieuPhat   VARCHAR(20)   PRIMARY KEY,
    MaPhong       VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Phong(MaPhong),
    SoHD          VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES HopDongThue(SoHD),
    NgayLap       DATE NOT NULL,
    NoiDungViPham NVARCHAR(500) NOT NULL,
    SoTienPhat    DECIMAL(18,2) NOT NULL CHECK (SoTienPhat > 0),
    TrangThai     VARCHAR(20)   NOT NULL DEFAULT 'Chua nop' CHECK (TrangThai IN ('Chua nop', 'Da nop')),
    NgayNop       DATE NULL
);
GO

/* ================== DU LIEU MAU ================== */
-- Gia thue = dien tich x 250.000 d/m2/thang x he so tang (tang cang cao he so cang lon)
INSERT INTO Tang(MaTang, TenTang, SoTang, HeSoGia) VALUES
('T01', N'Tang 1', 1, 1.00), ('T02', N'Tang 2', 2, 1.05), ('T03', N'Tang 3', 3, 1.10),
('T04', N'Tang 4', 4, 1.15), ('T05', N'Tang 5', 5, 1.25);

INSERT INTO Phong(MaPhong, MaTang, DienTich, Huong, GiaThue, TrangThai) VALUES
('P101', 'T01', 50, N'Dong',     12500000, 'Dang thue'),
('P102', 'T01', 80, N'Tay',      20000000, 'Trong'),
('P201', 'T02', 60, N'Nam',      15750000, 'Dang thue'),
('P202', 'T02', 40, N'Bac',      10500000, 'Dang thue'),
('P301', 'T03', 70, N'Dong Nam', 19250000, 'Dang thue'),
('P302', 'T03', 45, N'Tay Bac',  12375000, 'Trong'),
('P401', 'T04', 100, N'Nam',     28750000, 'Trong'),
('P501', 'T05', 120, N'Dong',    37500000, 'Trong');

INSERT INTO KhachHang(MaKH, TenCongTy, MaSoThue, NguoiDaiDien, DienThoai, Email, DiaChi) VALUES
('KH001', N'Cong ty TNHH Phan mem ABC', '0312345678', N'Nguyen Van Khoa', '0909111001', 'contact@abc.vn',   N'Q.1, TP.HCM'),
('KH002', N'Cong ty CP Thuong mai Minh Phat', '0312345679', N'Tran Thi Thu', '0909111002', 'info@minhphat.vn', N'Q.3, TP.HCM'),
('KH003', N'Van phong Luat Viet Tin', '0312345680', N'Le Quang Vinh', '0909111003', 'luat@viettin.vn', N'Q.Binh Thanh, TP.HCM');

-- HD001: 12 thang tu 01/03/2026, da tra du den ky 8 (thang 10/2026), da thanh toan dich vu -> du dieu kien gia han
-- HD002: 8 thang tu 01/04/2026, chua tra ky 7 (thang 10/2026), con hoa don dich vu thang 9 chua thanh toan -> khong duoc gia han
-- HD003: 6 thang tu 01/10/2026, vua tra truoc 6 thang
INSERT INTO HopDongThue(SoHD, MaKH, NgayKy, NgayBatDau, SoThang, NgayKetThuc, NoiDung, TrangThai) VALUES
('HD001', 'KH001', '2026-02-20', '2026-03-01', 12, '2027-03-01', N'Thue 2 van phong tang 2; tra truoc 6 thang, tu thang 7 tra dau thang; dich vu thanh toan cuoi thang', 'Hieu luc'),
('HD002', 'KH002', '2026-03-25', '2026-04-01', 8,  '2026-12-01', N'Thue van phong P301', 'Hieu luc'),
('HD003', 'KH003', '2026-09-28', '2026-10-01', 6,  '2027-04-01', N'Thue van phong P101', 'Hieu luc');

INSERT INTO ChiTietHopDong(SoHD, MaPhong, GiaThue, NgayBatDau, NgayKetThuc) VALUES
('HD001', 'P201', 15750000, '2026-03-01', '2027-03-01'),
('HD001', 'P202', 10500000, '2026-03-01', '2027-03-01'),
('HD002', 'P301', 19250000, '2026-04-01', '2026-12-01'),
('HD003', 'P101', 12500000, '2026-10-01', '2027-04-01');

INSERT INTO PhieuThuTienThue(SoPhieu, SoHD, TuKy, DenKy, SoTien, NgayThu, LoaiThu, NguoiThu) VALUES
('PT001', 'HD001', 1, 6, 157500000, '2026-02-20', 'TRA_TRUOC',  N'Ke toan Mai'),
('PT002', 'HD001', 7, 7,  26250000, '2026-09-01', 'HANG_THANG', N'Ke toan Mai'),
('PT003', 'HD001', 8, 8,  26250000, '2026-10-01', 'HANG_THANG', N'Ke toan Mai'),
('PT004', 'HD002', 1, 6, 115500000, '2026-03-25', 'TRA_TRUOC',  N'Ke toan Mai'),
('PT005', 'HD003', 1, 6,  75000000, '2026-09-28', 'TRA_TRUOC',  N'Ke toan Mai');

INSERT INTO DichVu(MaDV, TenDV, DonViTinh, DonGia) VALUES
('DIEN', N'Dien',            N'kWh',   3500),
('NUOC', N'Nuoc',            N'm3',    15000),
('NET',  N'Internet',        N'thang', 300000),
('VSINH', N'Ve sinh chung',  N'thang', 500000);

INSERT INTO HoaDonDichVu(MaHoaDon, SoHD, Thang, Nam, NgayLap, TongTien, TrangThai, NgayThanhToan) VALUES
('HDV001', 'HD001', 8, 2026, '2026-08-31', 4800000, 'Da thanh toan',   '2026-09-03'),
('HDV002', 'HD001', 9, 2026, '2026-09-30', 4975000, 'Da thanh toan',   '2026-10-02'),
('HDV003', 'HD002', 9, 2026, '2026-09-30', 3060000, 'Chua thanh toan', NULL);

INSERT INTO ChiTietHoaDonDV(MaHoaDon, MaDV, SoLuong, DonGia, ThanhTien) VALUES
('HDV001', 'DIEN', 1100, 3500, 3850000), ('HDV001', 'NUOC', 10, 15000, 150000),
('HDV001', 'NET', 1, 300000, 300000),    ('HDV001', 'VSINH', 1, 500000, 500000),
('HDV002', 'DIEN', 1150, 3500, 4025000), ('HDV002', 'NUOC', 10, 15000, 150000),
('HDV002', 'NET', 1, 300000, 300000),    ('HDV002', 'VSINH', 1, 500000, 500000),
('HDV003', 'DIEN', 640, 3500, 2240000),  ('HDV003', 'NUOC', 4, 15000, 60000),
('HDV003', 'NET', 1, 300000, 300000),    ('HDV003', 'VSINH', 1, 500000, 500000);

INSERT INTO NhanVienCongTy(MaNV, HoTen, CCCD, HinhAnh, MaKH, ChucVu, MaPhong, DienThoai, NgayBaoCao, TrangThai) VALUES
('NV001', N'Pham Minh Duc',  '079090101010', N'Images\NV001.jpg', 'KH001', N'Truong nhom phat trien', 'P201', '0909222001', '2026-03-02', 'Dang lam'),
('NV002', N'Vo Thi Hanh',    '079095202020', N'Images\NV002.jpg', 'KH001', N'Ke toan',                'P202', '0909222002', '2026-03-02', 'Dang lam'),
('NV003', N'Bui Van Long',   '079088303030', NULL,                 'KH002', N'Giam doc kinh doanh',    'P301', '0909222003', '2026-04-02', 'Dang lam');

INSERT INTO TheGiuXe(MaThe, MaNV, LoaiXe, BienSo, HieuXe, MauXe, NgayCap, TrangThai) VALUES
('TX001', 'NV001', N'Xe may', '59X1-12345', N'Honda Vision', N'Trang', '2026-03-03', 'Dang dung'),
('TX002', 'NV003', N'O to',   '51H-67890',  N'Toyota Vios',  N'Den',   '2026-04-03', 'Dang dung');

INSERT INTO KiemTraPhong(MaKT, MaPhong, NgayKiemTra, Nam, TinhTrang, MoTa, KeHoachTuSua, NguoiKiemTra) VALUES
('KT001', 'P201', '2025-12-10', 2025, N'Tot',          N'Ket cau on dinh',            NULL,                          N'Ky thuat Hoang'),
('KT002', 'P301', '2025-12-10', 2025, N'Can sua chua', N'Tham nuoc tran goc phai',    N'Chong tham truoc 03/2026',   N'Ky thuat Hoang');

INSERT INTO PhieuPhat(MaPhieuPhat, MaPhong, SoHD, NgayLap, NoiDungViPham, SoTienPhat, TrangThai, NgayNop) VALUES
('PP001', 'P301', 'HD002', '2026-08-15', N'Tu y duc tuong ngan giua van phong', 5000000, 'Chua nop', NULL);
GO
