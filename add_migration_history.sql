-- Script to add migration history records
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

-- Add Initial Migration record if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20250602153937_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250602153937_InitialCreate', '8.0.3');
END

-- Add second migration record if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20250605130438_AddOrderStatusAndPaymentFields')
BEGIN
    -- Check if we need to apply the actual migration by verifying if the columns exist
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'Status' AND Object_ID = Object_ID('Orders'))
    BEGIN
        -- Add Status column to Orders table if it doesn't exist
        ALTER TABLE Orders ADD Status nvarchar(50) NULL;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'PaymentMethod' AND Object_ID = Object_ID('Orders'))
    BEGIN
        -- Add PaymentMethod column to Orders table if it doesn't exist
        ALTER TABLE Orders ADD PaymentMethod nvarchar(50) NULL;
    END

    -- Record migration as applied
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250605130438_AddOrderStatusAndPaymentFields', '8.0.3');
END
