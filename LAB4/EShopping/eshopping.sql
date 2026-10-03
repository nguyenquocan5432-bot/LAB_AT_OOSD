-- =====================================================================
-- e-SHOPPING: tạo CSDL SQL Server (chạy toàn bộ file một lần)
-- Hệ thống quản lý sản phẩm là hệ thống NGOÀI nên KHÔNG có bảng Product ở đây.
-- Bảng OrderItem lưu "ảnh chụp" tên và đơn giá tại thời điểm đặt (giá có thể thay đổi).
-- =====================================================================
IF DB_ID(N'EShoppingDB') IS NULL CREATE DATABASE EShoppingDB;
GO
USE EShoppingDB;
GO

IF OBJECT_ID(N'OrderItem') IS NOT NULL DROP TABLE OrderItem;
IF OBJECT_ID(N'Orders') IS NOT NULL DROP TABLE Orders;
IF OBJECT_ID(N'Customer') IS NOT NULL DROP TABLE Customer;
IF OBJECT_ID(N'ShippingRate') IS NOT NULL DROP TABLE ShippingRate;
IF OBJECT_ID(N'CardType') IS NOT NULL DROP TABLE CardType;
GO

CREATE TABLE Customer (
    CustomerId   INT IDENTITY(1,1) PRIMARY KEY,
    FullName     NVARCHAR(100) NOT NULL,
    BirthDate    DATE          NOT NULL,
    IdNumber     NVARCHAR(30)  NOT NULL,          -- CMND / Passport
    Address      NVARCHAR(200) NOT NULL,
    Phone        NVARCHAR(20)  NOT NULL,
    Username     NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    Email        NVARCHAR(100) NULL               -- không bắt buộc
);

CREATE TABLE CardType (
    Code         NVARCHAR(10) PRIMARY KEY,
    Name         NVARCHAR(30) NOT NULL,
    NumberLength INT NOT NULL,                    -- 16 hoặc 15 chữ số
    CsvLength    INT NOT NULL,                    -- 3 hoặc 4 chữ số
    Fee          DECIMAL(18,0) NOT NULL           -- lệ phí mỗi lần thanh toán
);

CREATE TABLE ShippingRate (
    Region       NVARCHAR(30) NOT NULL,
    ShippingType INT NOT NULL,                    -- 1 Thường, 2 Nhanh, 3 Nhanh trong ngày
    Fee          DECIMAL(18,0) NOT NULL,
    CONSTRAINT PK_ShippingRate PRIMARY KEY (Region, ShippingType),
    CONSTRAINT CK_ShippingRate_Type CHECK (ShippingType IN (1,2,3))
);

CREATE TABLE Orders (
    OrderId         INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId      INT NOT NULL REFERENCES Customer(CustomerId),
    OrderTime       DATETIME2 NOT NULL,
    ShippingType    INT NOT NULL,
    Region          NVARCHAR(30) NOT NULL,
    ReceiverName    NVARCHAR(100) NOT NULL,       -- người nhận có thể khác người mua
    ReceiverAddress NVARCHAR(200) NOT NULL,
    ReceiverPhone   NVARCHAR(20)  NOT NULL,
    CardTypeCode    NVARCHAR(10)  NOT NULL REFERENCES CardType(Code),
    CardMasked      NVARCHAR(20)  NOT NULL,       -- chỉ lưu 4 số cuối, không lưu số thẻ đầy đủ và CSV
    CardHolder      NVARCHAR(100) NOT NULL,
    Subtotal        DECIMAL(18,0) NOT NULL,
    ShippingFee     DECIMAL(18,0) NOT NULL,
    CardFee         DECIMAL(18,0) NOT NULL,
    Total           DECIMAL(18,0) NOT NULL,
    Status          NVARCHAR(30)  NOT NULL
);

CREATE TABLE OrderItem (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId     INT NOT NULL REFERENCES Orders(OrderId),
    ProductId   NVARCHAR(20)  NOT NULL,           -- mã sản phẩm bên hệ thống quản lý sản phẩm
    ProductName NVARCHAR(200) NOT NULL,
    UnitPrice   DECIMAL(18,0) NOT NULL,
    Quantity    INT NOT NULL CHECK (Quantity > 0)
);
GO

-- Dữ liệu tra cứu
INSERT INTO CardType(Code,Name,NumberLength,CsvLength,Fee) VALUES
 (N'VISA',  N'Visa',             16, 3,  5000),
 (N'MASTER',N'Mastercard',       16, 3,  5000),
 (N'DISC',  N'Discover',         16, 3,  7000),
 (N'AMEX',  N'American Express', 15, 4, 10000);

INSERT INTO ShippingRate(Region,ShippingType,Fee) VALUES
 (N'Nội thành',1,15000),(N'Nội thành',2,30000),(N'Nội thành',3,50000),
 (N'Ngoại thành',1,25000),(N'Ngoại thành',2,45000),(N'Ngoại thành',3,70000),
 (N'Tỉnh khác',1,35000),(N'Tỉnh khác',2,60000),(N'Tỉnh khác',3,90000);
GO
