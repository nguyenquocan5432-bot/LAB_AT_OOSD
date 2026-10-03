-- 1. Bảng NHAN_VIEN (Gom chung tất cả các loại nhân viên)
CREATE TABLE NHAN_VIEN (
    MA_NV VARCHAR(20) PRIMARY KEY,
    TEN_NV NVARCHAR(100),
    DIA_CHI NVARCHAR(255),
    LOAI_NV VARCHAR(50), -- Dùng để phân biệt: 'Thuong', 'KD_LauNam', 'KD_TapSu'
    MA_NV_HUONG_DAN VARCHAR(20),
    
    -- Khóa ngoại tham chiếu đến chính bảng NHAN_VIEN (NV lâu năm hướng dẫn NV tập sự)
    CONSTRAINT FK_NV_HUONGDAN FOREIGN KEY (MA_NV_HUONG_DAN) REFERENCES NHAN_VIEN(MA_NV)
);

-- 2. Bảng QUA_TRINH_TANG_LUONG (Thực thể yếu / đa trị)
CREATE TABLE QUA_TRINH_TANG_LUONG (
    MA_NV VARCHAR(20),
    NGAY_HIEU_LUC DATE,
    LUONG_MOI DECIMAL(18, 2),
    
    -- Khóa chính gồm 2 thuộc tính
    PRIMARY KEY (MA_NV, NGAY_HIEU_LUC),
    
    -- Khóa ngoại tham chiếu đến bảng NHAN_VIEN
    CONSTRAINT FK_QTTL_NV FOREIGN KEY (MA_NV) REFERENCES NHAN_VIEN(MA_NV)
);

-- 3. Bảng DON_HANG (Thực thể độc lập)
CREATE TABLE DON_HANG (
    SO_DDH VARCHAR(20) PRIMARY KEY,
    KHACH_HANG NVARCHAR(100),
    TRI_GIA DECIMAL(18, 2)
);

-- 4. Bảng THUONG_LUONG (Lớp liên kết n-n)
CREATE TABLE THUONG_LUONG (
    MA_NV VARCHAR(20),
    SO_DDH VARCHAR(20),
    HOA_HONG DECIMAL(18, 2),
    
    -- Khóa chính gồm 2 thuộc tính
    PRIMARY KEY (MA_NV, SO_DDH),
    
    -- Khóa ngoại tham chiếu đến NHAN_VIEN và DON_HANG
    CONSTRAINT FK_TL_NV FOREIGN KEY (MA_NV) REFERENCES NHAN_VIEN(MA_NV),
    CONSTRAINT FK_TL_DH FOREIGN KEY (SO_DDH) REFERENCES DON_HANG(SO_DDH)
);