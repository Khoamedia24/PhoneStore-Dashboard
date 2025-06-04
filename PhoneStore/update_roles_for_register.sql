-- Thêm role Admin nếu chưa có
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'Admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Roles] ([RoleName], [Description], [IsSystem]) 
    VALUES ('Admin', N'Quản trị viên thông thường', 0)
END

-- Thêm role User nếu chưa có
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'User')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Roles] ([RoleName], [Description], [IsSystem]) 
    VALUES ('User', N'Người dùng hệ thống', 0)
END
