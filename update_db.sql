-- Add missing columns to Orders table
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'Status' AND Object_ID = Object_ID('Orders'))
BEGIN
    ALTER TABLE Orders ADD Status nvarchar(max) NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'PaymentMethod' AND Object_ID = Object_ID('Orders'))
BEGIN
    ALTER TABLE Orders ADD PaymentMethod nvarchar(max) NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'Notes' AND Object_ID = Object_ID('Orders'))
BEGIN
    ALTER TABLE Orders ADD Notes nvarchar(max) NULL;
END

-- Create EF Migrations History table if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

-- Add migration records
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20250602153937_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250602153937_InitialCreate', '8.0.3');
END

IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20250605130438_AddOrderStatusAndPaymentFields')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250605130438_AddOrderStatusAndPaymentFields', '8.0.3');
END
