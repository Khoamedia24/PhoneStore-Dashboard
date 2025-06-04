-- Thêm role SuperAdmin nếu chưa tồn tại
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'SuperAdmin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Roles] ([RoleName], [Description], [IsSystem]) 
    VALUES ('SuperAdmin', 'Có toàn quyền trên hệ thống', 1)
END

-- Lấy RoleId của SuperAdmin
DECLARE @SuperAdminRoleId INT
SELECT @SuperAdminRoleId = RoleId FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'SuperAdmin'

-- Set RoleId và IsApproved = 1 cho tài khoản admin
UPDATE [PhoneStore].[dbo].[Admins]
SET [IsApproved] = 1,
    [RoleId] = @SuperAdminRoleId
WHERE [Username] = 'admin1';
