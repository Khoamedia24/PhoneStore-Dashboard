-- Create PhoneStore database if it doesn't exist
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'PhoneStore')
BEGIN
    CREATE DATABASE PhoneStore;
    PRINT 'PhoneStore database created successfully.';
END
ELSE
BEGIN
    PRINT 'PhoneStore database already exists.';
END

-- Switch to the PhoneStore database
USE PhoneStore;

-- Create tables based on the migrations
-- Categories table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Categories')
BEGIN
    CREATE TABLE Categories(
        CategoryId INT IDENTITY(1,1) PRIMARY KEY,
        CategoryName NVARCHAR(100),
        Description NVARCHAR(500) NULL,
        ParentCategoryId INT NULL
    );
    PRINT 'Categories table created successfully.';
END

-- Colors table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Colors')
BEGIN
    CREATE TABLE Colors(
        ColorId INT IDENTITY(1,1) PRIMARY KEY,
        ColorName NVARCHAR(100),
        ColorCode NVARCHAR(50) NULL
    );
    PRINT 'Colors table created successfully.';

    -- Add a default color
    INSERT INTO Colors (ColorName, ColorCode) VALUES ('Default', '#000000');
    PRINT 'Default color added.';
END

-- DiscountPrograms table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DiscountPrograms')
BEGIN
    CREATE TABLE DiscountPrograms(
        DiscountId INT IDENTITY(1,1) PRIMARY KEY,
        DiscountName NVARCHAR(100),
        DiscountPercent DECIMAL(5,2) NULL,
        StartDate DATETIME NULL,
        EndDate DATETIME NULL,
        IsActive BIT DEFAULT 1
    );
    PRINT 'DiscountPrograms table created successfully.';
END

-- Products table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products')
BEGIN
    CREATE TABLE Products(
        ProductId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        ShortDescription NVARCHAR(500) NULL,
        DetailDescription NVARCHAR(MAX) NULL,
        Price DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL DEFAULT 0,
        CategoryId INT NOT NULL,
        DiscountId INT NULL,
        IsPublished BIT NOT NULL DEFAULT 1,
        Brand NVARCHAR(255) NULL,
        Model NVARCHAR(255) NULL,
        OperatingSystem NVARCHAR(255) NULL,
        ScreenSize NVARCHAR(100) NULL,
        Storage NVARCHAR(100) NULL,
        RAM NVARCHAR(100) NULL,
        CameraResolution NVARCHAR(100) NULL,
        BatteryCapacity NVARCHAR(100) NULL,
        Specifications NVARCHAR(2000) NULL,
        OriginalPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        DiscountPrice DECIMAL(18,2) NULL,
        CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
        CONSTRAINT FK_Products_DiscountPrograms FOREIGN KEY (DiscountId) REFERENCES DiscountPrograms(DiscountId)
    );
    PRINT 'Products table created successfully.';
END

-- ProductImages table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProductImages')
BEGIN
    CREATE TABLE ProductImages(
        ImageId INT IDENTITY(1,1) PRIMARY KEY,
        ProductId INT NOT NULL,
        ColorId INT NULL,
        ImageData VARBINARY(MAX) NULL,
        ImageMimeType NVARCHAR(100) NULL,
        ImageUrl NVARCHAR(255) NULL,
        AltText NVARCHAR(255) NULL,
        IsPrimary BIT NOT NULL DEFAULT 0,
        DisplayOrder INT NOT NULL DEFAULT 0,
        CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_ProductImages_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
        CONSTRAINT FK_ProductImages_Colors FOREIGN KEY (ColorId) REFERENCES Colors(ColorId)
    );
    PRINT 'ProductImages table created successfully.';
END

-- Customers table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Customers')
BEGIN
    CREATE TABLE Customers(
        CustomerId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NULL,
        Email NVARCHAR(100) NULL,
        PasswordHash NVARCHAR(255) NULL,
        Phone NVARCHAR(15) NULL,
        RegistrationDate DATETIME NULL DEFAULT GETDATE(),
        LastLoginDate DATETIME NULL
    );
    PRINT 'Customers table created successfully.';
END

-- ShippingAddresses table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ShippingAddresses')
BEGIN
    CREATE TABLE ShippingAddresses(
        AddressId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId INT NULL,
        RecipientName NVARCHAR(100) NULL,
        Phone NVARCHAR(15) NULL,
        AddressLine NVARCHAR(500) NULL,
        Ward NVARCHAR(100) NULL,
        District NVARCHAR(100) NULL,
        Province NVARCHAR(100) NULL,
        IsDefault BIT NULL,
        CONSTRAINT FK_ShippingAddresses_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
    PRINT 'ShippingAddresses table created successfully.';
END

-- Orders table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders')
BEGIN
    CREATE TABLE Orders(
        OrderId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId INT NULL,
        TotalAmount DECIMAL(18,2) NULL,
        Status NVARCHAR(50) NULL,
        PaymentMethod NVARCHAR(50) NULL,
        Notes NVARCHAR(500) NULL,
        OrderDate DATETIME NULL,
        CouponId INT NULL,
        ShippingAddressId INT NULL,
        CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
        CONSTRAINT FK_Orders_ShippingAddresses FOREIGN KEY (ShippingAddressId) REFERENCES ShippingAddresses(AddressId)
    );
    PRINT 'Orders table created successfully.';
END

-- OrderDetails table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderDetails')
BEGIN
    CREATE TABLE OrderDetails(
        OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NULL,
        ProductId INT NULL,
        ColorId INT NULL,
        Quantity INT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        TotalPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
        CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
        CONSTRAINT FK_OrderDetails_Colors FOREIGN KEY (ColorId) REFERENCES Colors(ColorId)
    );
    PRINT 'OrderDetails table created successfully.';
END

PRINT 'Database setup completed successfully.';
