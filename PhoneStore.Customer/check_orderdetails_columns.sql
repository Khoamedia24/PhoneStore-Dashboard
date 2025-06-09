USE [PhoneStore]
GO

-- Check OrderDetails table structure
SELECT
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.IS_NULLABLE,
    c.CHARACTER_MAXIMUM_LENGTH,
    c.NUMERIC_PRECISION,
    c.NUMERIC_SCALE
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_NAME = 'OrderDetails'
ORDER BY c.ORDINAL_POSITION;

PRINT 'Checking for UnitPrice and TotalPrice columns specifically:';

SELECT
    CASE
        WHEN EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OrderDetails' AND COLUMN_NAME = 'UnitPrice')
        THEN 'UnitPrice column EXISTS'
        ELSE 'UnitPrice column MISSING'
    END AS UnitPriceStatus,
    CASE
        WHEN EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OrderDetails' AND COLUMN_NAME = 'TotalPrice')
        THEN 'TotalPrice column EXISTS'
        ELSE 'TotalPrice column MISSING'
    END AS TotalPriceStatus;
