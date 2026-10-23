/* =========================================================
   CSDL: QuanLyNhanSu
   Bai 7 - Quan ly nhan su cong ty son Dong A
   ========================================================= */
IF DB_ID('QuanLyNhanSu') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyNhanSu SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyNhanSu;
END
GO
CREATE DATABASE QuanLyNhanSu;
GO
USE QuanLyNhanSu;
GO

/* ================== DANH MUC ================== */
CREATE TABLE PhongBan (
    MaPB        VARCHAR(10)   PRIMARY KEY,
    TenPB       NVARCHAR(100) NOT NULL,
    NhuCauTuyen INT NOT NULL DEFAULT 0 CHECK (NhuCauTuyen >= 0)
);
GO

CREATE TABLE ChucVu (
    MaCV          VARCHAR(10)   PRIMARY KEY,
    TenCV         NVARCHAR(100) NOT NULL,
    PhuCapMacDinh DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (PhuCapMacDinh >= 0)
);
GO

CREATE TABLE CaLamViec (
    MaCa        VARCHAR(10)  PRIMARY KEY,
    TenCa       NVARCHAR(50) NOT NULL,
    GioBatDau   TIME NOT NULL,
    GioKetThuc  TIME NOT NULL
);
GO

/* ================== NHAN VIEN ================== */
CREATE TABLE NhanVien (
    MaNV        VARCHAR(20)   PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    NgaySinh    DATE NOT NULL,
    GioiTinh    NVARCHAR(5)   NOT NULL CHECK (GioiTinh IN (N'Nam', N'Nu')),
    CCCD        VARCHAR(12)   NOT NULL,
    DiaChi      NVARCHAR(255) NULL,
    DienThoai   VARCHAR(20)   NULL,
    MaPB        VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    NgayVaoLam  DATE NOT NULL,
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'Thu viec'
                CHECK (TrangThai IN ('Thu viec', 'Chinh thuc', 'Nghi viec')),
    CONSTRAINT UQ_NhanVien_CCCD UNIQUE (CCCD)
);
GO

/* ================== QUYET DINH - HOP DONG ================== */
CREATE TABLE QuyetDinhTuyenDung (
    SoQD           VARCHAR(20)   PRIMARY KEY,
    MaNV           VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaPB           VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    NgayKy         DATE NOT NULL,
    NgayBatDauTV   DATE NOT NULL,
    NgayKetThucTV  DATE NOT NULL,
    LuongThuViec   DECIMAL(18,2) NOT NULL CHECK (LuongThuViec > 0),
    DieuKhoanCongTy   NVARCHAR(500) NOT NULL,
    DieuKhoanNhanVien NVARCHAR(500) NOT NULL,
    NguoiKy        NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_QDTD_MaNV UNIQUE (MaNV),
    CONSTRAINT CK_QDTD_ThoiGian CHECK (NgayKetThucTV > NgayBatDauTV)
);
GO

CREATE TABLE HopDongLaoDong (
    SoHD        VARCHAR(20)   PRIMARY KEY,
    MaNV        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    LoaiHD      VARCHAR(10)   NOT NULL CHECK (LoaiHD IN ('DAI_HAN', 'NGAN_HAN')),
    NgayKy      DATE NOT NULL,
    NgayBatDau  DATE NOT NULL,
    NgayKetThuc DATE NULL,
    NoiDung     NVARCHAR(500) NULL,
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'Hieu luc'
                CHECK (TrangThai IN ('Hieu luc', 'Het hieu luc')),
    CONSTRAINT CK_HD_NganHan CHECK (LoaiHD = 'DAI_HAN' OR (NgayKetThuc IS NOT NULL AND NgayKetThuc > NgayBatDau))
);
GO

CREATE TABLE QuyetDinhLuong (
    SoQD          VARCHAR(20)   PRIMARY KEY,
    MaNV          VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    NgayKy        DATE NOT NULL,
    NgayHieuLuc   DATE NOT NULL,
    LuongCoBan    DECIMAL(18,2) NOT NULL CHECK (LuongCoBan > 0),
    PhuCapLuong   DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (PhuCapLuong >= 0),
    PhuCapDocHai  DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (PhuCapDocHai >= 0),
    NguoiKy       NVARCHAR(100) NOT NULL,
    GhiChu        NVARCHAR(255) NULL
);
GO

CREATE TABLE QuyetDinhChucVu (
    SoQD          VARCHAR(20)   PRIMARY KEY,
    MaNV          VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaCV          VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES ChucVu(MaCV),
    NgayKy        DATE NOT NULL,
    NgayHieuLuc   DATE NOT NULL,
    PhuCapChucVu  DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (PhuCapChucVu >= 0),
    NguoiKy       NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE HoSoBaoHiem (
    MaHSBH      VARCHAR(20)  PRIMARY KEY,
    MaNV        VARCHAR(20)  NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    SoHD        VARCHAR(20)  NOT NULL FOREIGN KEY REFERENCES HopDongLaoDong(SoHD),
    SoBHXH      VARCHAR(20)  NOT NULL,
    SoBHYT      VARCHAR(20)  NOT NULL,
    NoiKCB      NVARCHAR(150) NULL,
    NgayLap     DATE NOT NULL,
    TyLeBHXH    DECIMAL(5,2) NOT NULL DEFAULT 8   CHECK (TyLeBHXH BETWEEN 0 AND 100),
    TyLeBHYT    DECIMAL(5,2) NOT NULL DEFAULT 1.5 CHECK (TyLeBHYT BETWEEN 0 AND 100),
    CONSTRAINT UQ_HSBH_MaNV UNIQUE (MaNV),
    CONSTRAINT UQ_HSBH_BHXH UNIQUE (SoBHXH),
    CONSTRAINT UQ_HSBH_BHYT UNIQUE (SoBHYT)
);
GO

/* ================== PHAN CONG - CHAM CONG - PHU TROI ================== */
CREATE TABLE KeHoachPhanCong (
    MaKH        VARCHAR(20)   PRIMARY KEY,
    MaPB        VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    TuNgay      DATE NOT NULL,
    DenNgay     DATE NOT NULL,
    NguoiLap    NVARCHAR(100) NOT NULL,
    NgayLap     DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    CONSTRAINT CK_KH_Tuan CHECK (DATEDIFF(DAY, TuNgay, DenNgay) = 6)
);
GO

CREATE TABLE ChiTietPhanCong (
    MaCTPC      INT IDENTITY(1,1) PRIMARY KEY,
    MaKH        VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES KeHoachPhanCong(MaKH),
    MaNV        VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    NgayLam     DATE NOT NULL,
    MaCa        VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES CaLamViec(MaCa),
    CONSTRAINT UQ_CTPC UNIQUE (MaNV, NgayLam, MaCa)
);
GO

CREATE TABLE BangChamCong (
    MaBCC          VARCHAR(30)  PRIMARY KEY,
    MaNV           VARCHAR(20)  NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    Thang          INT NOT NULL CHECK (Thang BETWEEN 1 AND 12),
    Nam            INT NOT NULL CHECK (Nam >= 2000),
    SoNgayCong     DECIMAL(4,1) NOT NULL CHECK (SoNgayCong BETWEEN 0 AND 31),
    SoNgayNghiPhep DECIMAL(4,1) NOT NULL DEFAULT 0 CHECK (SoNgayNghiPhep >= 0),
    SoNgayNghiKP   DECIMAL(4,1) NOT NULL DEFAULT 0 CHECK (SoNgayNghiKP >= 0),
    CONSTRAINT UQ_BCC_NV_Thang UNIQUE (MaNV, Thang, Nam)
);
GO

CREATE TABLE PhieuPhuTroi (
    SoPhieu     VARCHAR(20)   PRIMARY KEY,
    NgayPhuTroi DATE NOT NULL,
    HinhThuc    VARCHAR(10)   NOT NULL CHECK (HinhThuc IN ('TANG_CA', 'NGAY_LE', 'NGAY_NGHI')),
    MaPB        VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    NguoiLap    NVARCHAR(100) NOT NULL,
    GhiChu      NVARCHAR(255) NULL
);
GO

CREATE TABLE ChiTietPhuTroi (
    SoPhieu     VARCHAR(20)  NOT NULL FOREIGN KEY REFERENCES PhieuPhuTroi(SoPhieu),
    MaNV        VARCHAR(20)  NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    SoGio       DECIMAL(4,1) NOT NULL CHECK (SoGio > 0 AND SoGio <= 12),
    CONSTRAINT PK_ChiTietPhuTroi PRIMARY KEY (SoPhieu, MaNV)
);
GO

/* ================== KHEN THUONG - KY LUAT ================== */
CREATE TABLE QuyetDinhKhenThuong (
    SoQD          VARCHAR(20)   PRIMARY KEY,
    NgayKy        DATE NOT NULL,
    LoaiKT        VARCHAR(15)   NOT NULL CHECK (LoaiKT IN ('THANH_TICH', 'NGAY_LE', 'CUOI_NAM', 'LUONG_13')),
    DoiTuong      VARCHAR(10)   NOT NULL CHECK (DoiTuong IN ('CA_NHAN', 'TAP_THE')),
    MaNV          VARCHAR(20)   NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaPB          VARCHAR(10)   NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    LyDo          NVARCHAR(255) NOT NULL,
    SoTien        DECIMAL(18,2) NOT NULL CHECK (SoTien > 0),
    HinhThucChi   VARCHAR(10)   NOT NULL CHECK (HinhThucChi IN ('PHIEU_CHI', 'BANG_LUONG')),
    NguoiKy       NVARCHAR(100) NOT NULL,
    CONSTRAINT CK_QDKT_DoiTuong CHECK (
        (DoiTuong = 'CA_NHAN' AND MaNV IS NOT NULL AND MaPB IS NULL) OR
        (DoiTuong = 'TAP_THE' AND MaPB IS NOT NULL AND MaNV IS NULL))
);
GO

CREATE TABLE PhieuChiKhenThuong (
    SoPhieuChi  VARCHAR(20)   PRIMARY KEY,
    SoQD        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES QuyetDinhKhenThuong(SoQD),
    NgayChi     DATE NOT NULL,
    SoTien      DECIMAL(18,2) NOT NULL CHECK (SoTien > 0),
    NguoiNhan   NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_PhieuChi_QD UNIQUE (SoQD)
);
GO

CREATE TABLE QuyetDinhKyLuat (
    SoQD          VARCHAR(20)   PRIMARY KEY,
    NgayKy        DATE NOT NULL,
    DoiTuong      VARCHAR(10)   NOT NULL CHECK (DoiTuong IN ('CA_NHAN', 'TAP_THE')),
    MaNV          VARCHAR(20)   NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaPB          VARCHAR(10)   NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    LyDo          NVARCHAR(255) NOT NULL,
    HinhThucKL    NVARCHAR(50)  NOT NULL,
    SoTienPhat    DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (SoTienPhat >= 0),
    HinhThucThu   VARCHAR(10)   NOT NULL CHECK (HinhThucThu IN ('PHIEU_THU', 'TRU_LUONG')),
    NguoiKy       NVARCHAR(100) NOT NULL,
    CONSTRAINT CK_QDKL_DoiTuong CHECK (
        (DoiTuong = 'CA_NHAN' AND MaNV IS NOT NULL AND MaPB IS NULL) OR
        (DoiTuong = 'TAP_THE' AND MaPB IS NOT NULL AND MaNV IS NULL))
);
GO

CREATE TABLE PhieuThuNopPhat (
    SoPhieuThu  VARCHAR(20)   PRIMARY KEY,
    SoQD        VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES QuyetDinhKyLuat(SoQD),
    NgayThu     DATE NOT NULL,
    SoTien      DECIMAL(18,2) NOT NULL CHECK (SoTien > 0),
    NguoiNop    NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_PhieuThu_QD UNIQUE (SoQD)
);
GO

/* ================== BANG LUONG ================== */
CREATE TABLE BangLuong (
    MaBL          VARCHAR(30)   PRIMARY KEY,
    MaNV          VARCHAR(20)   NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    Thang         INT NOT NULL CHECK (Thang BETWEEN 1 AND 12),
    Nam           INT NOT NULL,
    LuongCoBan    DECIMAL(18,2) NOT NULL,
    SoNgayCong    DECIMAL(4,1)  NOT NULL,
    LuongTheoCong DECIMAL(18,2) NOT NULL,
    PhuCapChucVu  DECIMAL(18,2) NOT NULL DEFAULT 0,
    PhuCapLuong   DECIMAL(18,2) NOT NULL DEFAULT 0,
    PhuCapDocHai  DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoGioPhuTroi  DECIMAL(6,1)  NOT NULL DEFAULT 0,
    TienPhuTroi   DECIMAL(18,2) NOT NULL DEFAULT 0,
    TienThuong    DECIMAL(18,2) NOT NULL DEFAULT 0,
    TienPhat      DECIMAL(18,2) NOT NULL DEFAULT 0,
    TruBHXH       DECIMAL(18,2) NOT NULL DEFAULT 0,
    TruBHYT       DECIMAL(18,2) NOT NULL DEFAULT 0,
    ThucLanh      DECIMAL(18,2) NOT NULL,
    NgayTinh      DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_BangLuong_NV_Thang UNIQUE (MaNV, Thang, Nam)
);
GO

/* ================== DU LIEU MAU ================== */
INSERT INTO PhongBan(MaPB, TenPB, NhuCauTuyen) VALUES
('PKT', N'Phong tai chinh ke toan', 1),
('PKY', N'Phong ky thuat', 2),
('PSX', N'Phong san xuat', 5),
('PNS', N'Phong nhan su', 0);

INSERT INTO ChucVu(MaCV, TenCV, PhuCapMacDinh) VALUES
('GD', N'Giam doc', 5000000),
('TP', N'Truong phong', 3000000),
('PP', N'Pho phong', 2000000),
('TT', N'To truong', 1000000),
('NV', N'Nhan vien', 0);

INSERT INTO CaLamViec(MaCa, TenCa, GioBatDau, GioKetThuc) VALUES
('HC',  N'Hanh chinh', '08:00', '17:00'),
('CA1', N'Ca sang',    '06:00', '14:00'),
('CA2', N'Ca chieu',   '14:00', '22:00'),
('CA3', N'Ca dem',     '22:00', '06:00');

INSERT INTO NhanVien(MaNV, HoTen, NgaySinh, GioiTinh, CCCD, DiaChi, DienThoai, MaPB, NgayVaoLam, TrangThai) VALUES
('NV001', N'Tran Van Hung',     '1990-05-12', N'Nam', '079090001111', N'Q. Binh Tan, TP.HCM', '0909000111', 'PSX', '2026-01-05', 'Chinh thuc'),
('NV002', N'Le Thi Lan',        '1995-08-20', N'Nu',  '079095002222', N'Q.10, TP.HCM',        '0909000222', 'PKT', '2026-03-02', 'Chinh thuc'),
('NV003', N'Nguyen Minh Tuan',  '2000-11-02', N'Nam', '079200003333', N'Thu Duc, TP.HCM',     '0909000333', 'PKY', '2026-09-01', 'Thu viec'),
('NV004', N'Pham Thu Ha',       '2001-02-14', N'Nu',  '079201004444', N'Q.8, TP.HCM',         '0909000444', 'PSX', '2026-10-01', 'Thu viec');

INSERT INTO QuyetDinhTuyenDung(SoQD, MaNV, MaPB, NgayKy, NgayBatDauTV, NgayKetThucTV, LuongThuViec, DieuKhoanCongTy, DieuKhoanNhanVien, NguoiKy) VALUES
('QDTD001', 'NV001', 'PSX', '2025-11-01', '2025-11-05', '2026-01-04', 6500000, N'Tra luong thu viec dung han, trang bi bao ho lao dong', N'Chap hanh noi quy, hoan thanh cong viec duoc giao', N'Giam doc Vo Thanh Son'),
('QDTD002', 'NV002', 'PKT', '2026-01-01', '2026-01-02', '2026-03-01', 6000000, N'Tra luong thu viec dung han', N'Chap hanh noi quy, bao mat so lieu tai chinh', N'Giam doc Vo Thanh Son'),
('QDTD003', 'NV003', 'PKY', '2026-08-25', '2026-09-01', '2026-10-31', 6000000, N'Tra luong thu viec dung han, ho tro dao tao', N'Chap hanh noi quy, hoan thanh cong viec duoc giao', N'Giam doc Vo Thanh Son'),
('QDTD004', 'NV004', 'PSX', '2026-09-25', '2026-10-01', '2026-11-30', 5500000, N'Tra luong thu viec dung han, trang bi bao ho lao dong', N'Chap hanh noi quy an toan lao dong', N'Giam doc Vo Thanh Son');

INSERT INTO HopDongLaoDong(SoHD, MaNV, LoaiHD, NgayKy, NgayBatDau, NgayKetThuc, NoiDung) VALUES
('HD001', 'NV001', 'DAI_HAN',  '2026-01-05', '2026-01-05', NULL,         N'Hop dong khong xac dinh thoi han - cong nhan pha son'),
('HD002', 'NV002', 'NGAN_HAN', '2026-03-02', '2026-03-02', '2027-03-01', N'Hop dong 12 thang - ke toan vien');

INSERT INTO QuyetDinhLuong(SoQD, MaNV, NgayKy, NgayHieuLuc, LuongCoBan, PhuCapLuong, PhuCapDocHai, NguoiKy, GhiChu) VALUES
('QDL001', 'NV001', '2026-01-05', '2026-01-05', 8000000, 500000, 700000, N'Giam doc Vo Thanh Son', N'Luong khi ky hop dong'),
('QDL002', 'NV002', '2026-03-02', '2026-03-02', 7000000, 500000, 0,      N'Giam doc Vo Thanh Son', N'Luong khi ky hop dong'),
('QDL003', 'NV001', '2026-07-01', '2026-07-01', 8800000, 500000, 700000, N'Giam doc Vo Thanh Son', N'Tang luong dinh ky');

INSERT INTO QuyetDinhChucVu(SoQD, MaNV, MaCV, NgayKy, NgayHieuLuc, PhuCapChucVu, NguoiKy) VALUES
('QDCV001', 'NV001', 'TT', '2026-07-01', '2026-07-01', 1000000, N'Giam doc Vo Thanh Son');

INSERT INTO HoSoBaoHiem(MaHSBH, MaNV, SoHD, SoBHXH, SoBHYT, NoiKCB, NgayLap) VALUES
('BH001', 'NV001', 'HD001', '7926000111', 'DN4797926000111', N'Benh vien quan Binh Tan', '2026-01-10');

INSERT INTO KeHoachPhanCong(MaKH, MaPB, TuNgay, DenNgay, NguoiLap) VALUES
('KH001', 'PSX', '2026-09-07', '2026-09-13', N'Truong phong san xuat');
INSERT INTO ChiTietPhanCong(MaKH, MaNV, NgayLam, MaCa) VALUES
('KH001', 'NV001', '2026-09-07', 'CA1'), ('KH001', 'NV001', '2026-09-08', 'CA1'),
('KH001', 'NV001', '2026-09-09', 'CA2'), ('KH001', 'NV001', '2026-09-12', 'CA1');

INSERT INTO BangChamCong(MaBCC, MaNV, Thang, Nam, SoNgayCong, SoNgayNghiPhep, SoNgayNghiKP) VALUES
('CC001', 'NV001', 9, 2026, 26, 0, 0),
('CC002', 'NV002', 9, 2026, 24, 1, 1),
('CC003', 'NV003', 9, 2026, 25, 1, 0);

INSERT INTO PhieuPhuTroi(SoPhieu, NgayPhuTroi, HinhThuc, MaPB, NguoiLap, GhiChu) VALUES
('PT001', '2026-09-02', 'NGAY_LE', 'PSX', N'Ban cham cong', N'Lam ngay Quoc khanh'),
('PT002', '2026-09-12', 'TANG_CA', 'PSX', N'Ban cham cong', N'Tang ca don hang son');
INSERT INTO ChiTietPhuTroi(SoPhieu, MaNV, SoGio) VALUES
('PT001', 'NV001', 8), ('PT001', 'NV002', 8), ('PT002', 'NV001', 4);

INSERT INTO QuyetDinhKhenThuong(SoQD, NgayKy, LoaiKT, DoiTuong, MaNV, MaPB, LyDo, SoTien, HinhThucChi, NguoiKy) VALUES
('QDKT001', '2026-09-15', 'THANH_TICH', 'CA_NHAN', 'NV001', NULL,  N'Sang kien giam hao hut son', 2000000, 'BANG_LUONG', N'Giam doc Vo Thanh Son'),
('QDKT002', '2026-09-20', 'THANH_TICH', 'TAP_THE', NULL,    'PKT', N'Hoan thanh quyet toan quy 3', 3000000, 'PHIEU_CHI', N'Giam doc Vo Thanh Son');
INSERT INTO PhieuChiKhenThuong(SoPhieuChi, SoQD, NgayChi, SoTien, NguoiNhan) VALUES
('PC001', 'QDKT002', '2026-09-21', 3000000, N'Phong tai chinh ke toan');

INSERT INTO QuyetDinhKyLuat(SoQD, NgayKy, DoiTuong, MaNV, MaPB, LyDo, HinhThucKL, SoTienPhat, HinhThucThu, NguoiKy) VALUES
('QDKL001', '2026-09-22', 'CA_NHAN', 'NV002', NULL, N'Nghi khong phep 1 ngay', N'Khien trach', 200000, 'TRU_LUONG', N'Giam doc Vo Thanh Son');
GO
