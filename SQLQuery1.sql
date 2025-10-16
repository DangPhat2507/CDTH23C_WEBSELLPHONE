-- Tạo database
CREATE DATABASE DB_WEBSELLPHONE;
GO
USE DB_WEBSELLPHONE;
GO

-- Bảng danh mục (Categories)
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);

-- Bảng sản phẩm (Products)
CREATE TABLE Products (
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(200) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    DonGiaKhuyenMai DECIMAL(18,2) NULL,
    HinhAnh NVARCHAR(255),
    MoTa NVARCHAR(MAX),
    CategoryID INT NOT NULL,
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID)
        REFERENCES Categories(CategoryID)
);

-- Bảng người dùng (nếu dùng Identity thì không cần tạo thủ công, nhưng để sẵn đây cho bạn hiểu)
CREATE TABLE NguoiDung (
    UserID NVARCHAR(450) PRIMARY KEY,
    HoTen NVARCHAR(150),
    Email NVARCHAR(150),
    DiaChi NVARCHAR(255),
    BiKhoa BIT DEFAULT 0
);

-- Bảng đơn hàng (Orders)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450),
    CreatedAt DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Pending',
    CONSTRAINT FK_Order_User FOREIGN KEY (UserId)
        REFERENCES NguoiDung(UserID)
);

-- Chi tiết đơn hàng (OrderDetails)
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGiaLucMua DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID)
        REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (MaSP)
        REFERENCES Products(MaSP)
);

-- Bảng đánh giá sản phẩm (Reviews)
CREATE TABLE Reviews (
    ReviewID INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL,
    UserId NVARCHAR(450),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    NoiDung NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    DaDuyet BIT DEFAULT 0,
    CONSTRAINT FK_Review_Product FOREIGN KEY (MaSP)
        REFERENCES Products(MaSP),
    CONSTRAINT FK_Review_User FOREIGN KEY (UserId)
        REFERENCES NguoiDung(UserID)
);
-- Danh mục mẫu
INSERT INTO Categories (CategoryName)
VALUES (N'iPhone'), (N'Samsung'), (N'Xiaomi'), (N'Oppo');

-- Sản phẩm mẫu
INSERT INTO Products (TenSP, DonGia, DonGiaKhuyenMai, HinhAnh, MoTa, CategoryID)
VALUES
(N'iPhone 15 Pro Max', 35000000, 33000000, 'iphone15.jpg', N'Mẫu cao cấp mới nhất của Apple', 1),
(N'Samsung Galaxy S24 Ultra', 29000000, 27000000, 's24ultra.jpg', N'Mẫu flagship của Samsung', 2),
(N'Xiaomi 14', 18000000, NULL, 'xiaomi14.jpg', N'Giá rẻ, cấu hình mạnh', 3),
(N'Oppo Find X7', 21000000, 20000000, 'findx7.jpg', N'Thiết kế đẹp, camera tốt', 4);
