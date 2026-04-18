CREATE DATABASE SE_LabAssignment2;
GO

USE SE_LabAssignment2;
GO

-- =============================================
-- TABLE: Item
-- =============================================
CREATE TABLE Item (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100) NOT NULL,
    Size NVARCHAR(30) NOT NULL
);
GO

-- =============================================
-- TABLE: Agent
-- =============================================
CREATE TABLE Agent (
    AgentID INT IDENTITY(1,1) PRIMARY KEY,
    AgentName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200) NOT NULL
);
GO

-- =============================================
-- TABLE: [Order]
-- =============================================
CREATE TABLE [Order] (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATETIME NOT NULL,
    AgentID INT NOT NULL,
    CONSTRAINT FK_Order_Agent FOREIGN KEY (AgentID) REFERENCES Agent(AgentID)
);
GO

-- =============================================
-- TABLE: OrderDetail
-- =============================================
CREATE TABLE OrderDetail (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ItemID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitAmount DECIMAL(18,2) NOT NULL CHECK (UnitAmount >= 0),
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderDetail_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
);
GO

-- =============================================
-- TABLE: Users
-- =============================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    [Password] NVARCHAR(100) NOT NULL,
    [Lock] BIT NOT NULL DEFAULT 0
);
GO

-- =============================================
-- INSERT DATA: Item (15 rows)
-- =============================================
INSERT INTO Item (ItemName, Size) VALUES
(N'Pen', N'Small'),
(N'Notebook', N'Medium'),
(N'Pencil', N'Small'),
(N'Eraser', N'Small'),
(N'Ruler', N'Medium'),
(N'Marker', N'Medium'),
(N'Paper A4', N'Large'),
(N'File Folder', N'Large'),
(N'Stapler', N'Medium'),
(N'Tape', N'Small'),
(N'Scissors', N'Medium'),
(N'Glue', N'Small'),
(N'Calculator', N'Medium'),
(N'Clipboard', N'Large'),
(N'Envelope', N'Medium');
GO

-- =============================================
-- INSERT DATA: Agent (15 rows)
-- =============================================
INSERT INTO Agent (AgentName, Address) VALUES
(N'Alpha Trading', N'1 Nguyen Trai, District 1'),
(N'Binh Minh Store', N'12 Le Loi, District 3'),
(N'Central Office Supply', N'25 Hai Ba Trung, District 1'),
(N'Delta Stationery', N'18 Vo Van Tan, District 3'),
(N'East Star Agent', N'7 Phan Xich Long, Phu Nhuan'),
(N'Future Mart', N'66 Hoang Van Thu, Tan Binh'),
(N'Golden Paper Co', N'101 Tran Hung Dao, District 5'),
(N'Happy Shop', N'44 Cach Mang Thang 8, District 10'),
(N'Ideal Goods', N'9 Dien Bien Phu, Binh Thanh'),
(N'Jade Supplier', N'11 Nguyen Thi Minh Khai, District 1'),
(N'Khanh An Store', N'23 Cong Hoa, Tan Binh'),
(N'Lotus Trading', N'88 Quang Trung, Go Vap'),
(N'Mekong Agent', N'5 Nguyen Oanh, Go Vap'),
(N'New Day Office', N'90 Xo Viet Nghe Tinh, Binh Thanh'),
(N'Orient Supply', N'77 Au Co, Tan Phu');
GO

-- =============================================
-- INSERT DATA: Users (15 rows)
-- =============================================
INSERT INTO Users (UserName, Email, [Password], [Lock]) VALUES
(N'admin', 'admin@gmail.com', N'123', 0),
(N'user01', 'user01@gmail.com', N'123', 0),
(N'user02', 'user02@gmail.com', N'123', 0),
(N'user03', 'user03@gmail.com', N'123', 0),
(N'user04', 'user04@gmail.com', N'123', 0),
(N'user05', 'user05@gmail.com', N'123', 0),
(N'user06', 'user06@gmail.com', N'123', 0),
(N'user07', 'user07@gmail.com', N'123', 0),
(N'user08', 'user08@gmail.com', N'123', 0),
(N'user09', 'user09@gmail.com', N'123', 0),
(N'user10', 'user10@gmail.com', N'123', 0),
(N'user11', 'user11@gmail.com', N'123', 1),
(N'user12', 'user12@gmail.com', N'123', 0),
(N'user13', 'user13@gmail.com', N'123', 1),
(N'user14', 'user14@gmail.com', N'123', 0);
GO

-- =============================================
-- INSERT DATA: [Order] (15 rows)
-- =============================================
INSERT INTO [Order] (OrderDate, AgentID) VALUES
('2025-03-01 08:30:00', 1),
('2025-03-02 09:15:00', 2),
('2025-03-03 10:00:00', 3),
('2025-03-04 10:45:00', 4),
('2025-03-05 11:20:00', 5),
('2025-03-06 13:10:00', 6),
('2025-03-07 14:00:00', 7),
('2025-03-08 14:40:00', 8),
('2025-03-09 15:05:00', 9),
('2025-03-10 15:45:00', 10),
('2025-03-11 16:10:00', 11),
('2025-03-12 08:50:00', 12),
('2025-03-13 09:25:00', 13),
('2025-03-14 10:35:00', 14),
('2025-03-15 11:55:00', 15);
GO

-- =============================================
-- INSERT DATA: OrderDetail (30 rows)
-- =============================================
INSERT INTO OrderDetail (OrderID, ItemID, Quantity, UnitAmount) VALUES
(1, 1, 10, 5.00),
(1, 2, 5, 20.00),
(2, 3, 12, 3.00),
(2, 4, 8, 2.00),
(3, 5, 6, 7.50),
(3, 6, 4, 15.00),
(4, 7, 10, 12.00),
(4, 8, 7, 18.00),
(5, 9, 3, 25.00),
(5, 10, 9, 4.00),
(6, 11, 5, 14.00),
(6, 12, 6, 6.50),
(7, 13, 2, 120.00),
(7, 14, 4, 22.00),
(8, 15, 20, 1.50),
(8, 1, 15, 5.00),
(9, 2, 10, 20.00),
(9, 3, 25, 3.00),
(10, 4, 18, 2.00),
(10, 5, 10, 7.50),
(11, 6, 7, 15.00),
(11, 7, 5, 12.00),
(12, 8, 9, 18.00),
(12, 9, 6, 25.00),
(13, 10, 14, 4.00),
(13, 11, 8, 14.00),
(14, 12, 11, 6.50),
(14, 13, 3, 120.00),
(15, 14, 5, 22.00),
(15, 15, 30, 1.50);
GO


-- 1. Best items
SELECT i.ItemID, i.ItemName, SUM(od.Quantity) AS TotalSold
FROM OrderDetail od
JOIN Item i ON od.ItemID = i.ItemID
GROUP BY i.ItemID, i.ItemName
ORDER BY TotalSold DESC;
GO

-- 2. Items purchased by customers (agents)
SELECT a.AgentName, i.ItemName, od.Quantity, od.UnitAmount, o.OrderDate
FROM [Order] o
JOIN Agent a ON o.AgentID = a.AgentID
JOIN OrderDetail od ON o.OrderID = od.OrderID
JOIN Item i ON od.ItemID = i.ItemID
ORDER BY a.AgentName, o.OrderDate;
GO

-- 3. Customer purchases items summary
SELECT a.AgentName, COUNT(DISTINCT o.OrderID) AS TotalOrders, SUM(od.Quantity) AS TotalQuantity
FROM Agent a
JOIN [Order] o ON a.AgentID = o.AgentID
JOIN OrderDetail od ON o.OrderID = od.OrderID
GROUP BY a.AgentName
ORDER BY TotalQuantity DESC;
GO
