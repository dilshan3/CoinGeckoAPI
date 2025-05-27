-- Run this in SQL Server to create the table
CREATE TABLE Coins (
    Id NVARCHAR(100) PRIMARY KEY,
    Symbol NVARCHAR(20),
    Name NVARCHAR(100),
    CurrentPrice DECIMAL(18, 8),
    MarketCap BIGINT
);