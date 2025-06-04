-- Xóa dữ liệu cũ để tạo mới (nếu cần)
-- DELETE FROM [PhoneStore].[dbo].[RolePermissions];
-- DELETE FROM [PhoneStore].[dbo].[Permissions];

-- Tạo các quyền theo từng khu vực quản lý
-- 1. Quản lý sản phẩm
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem sản phẩm')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem sản phẩm', N'Quyền xem danh sách và chi tiết sản phẩm', 'Product', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm sản phẩm')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm sản phẩm', N'Quyền thêm sản phẩm mới', 'Product', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa sản phẩm')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa sản phẩm', N'Quyền chỉnh sửa thông tin sản phẩm', 'Product', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa sản phẩm')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa sản phẩm', N'Quyền xóa sản phẩm', 'Product', 'Delete')
END

-- 2. Quản lý danh mục
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem danh mục')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem danh mục', N'Quyền xem danh sách danh mục', 'Category', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm danh mục')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm danh mục', N'Quyền thêm danh mục mới', 'Category', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa danh mục')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa danh mục', N'Quyền chỉnh sửa danh mục', 'Category', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa danh mục')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa danh mục', N'Quyền xóa danh mục', 'Category', 'Delete')
END

-- 3. Quản lý màu sắc
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem màu sắc')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem màu sắc', N'Quyền xem danh sách màu sắc', 'Color', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm màu sắc')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm màu sắc', N'Quyền thêm màu sắc mới', 'Color', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa màu sắc')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa màu sắc', N'Quyền chỉnh sửa màu sắc', 'Color', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa màu sắc')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa màu sắc', N'Quyền xóa màu sắc', 'Color', 'Delete')
END

-- 4. Quản lý giảm giá
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem giảm giá')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem giảm giá', N'Quyền xem danh sách giảm giá', 'Discount', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm giảm giá')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm giảm giá', N'Quyền thêm chương trình giảm giá mới', 'Discount', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa giảm giá')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa giảm giá', N'Quyền chỉnh sửa chương trình giảm giá', 'Discount', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa giảm giá')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa giảm giá', N'Quyền xóa chương trình giảm giá', 'Discount', 'Delete')
END

-- 5. Quản lý đơn hàng
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem đơn hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem đơn hàng', N'Quyền xem danh sách và chi tiết đơn hàng', 'Order', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm đơn hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm đơn hàng', N'Quyền tạo đơn hàng mới', 'Order', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa đơn hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa đơn hàng', N'Quyền chỉnh sửa đơn hàng', 'Order', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa đơn hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa đơn hàng', N'Quyền xóa đơn hàng', 'Order', 'Delete')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Cập nhật trạng thái đơn hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Cập nhật trạng thái đơn hàng', N'Quyền cập nhật trạng thái đơn hàng', 'Order', 'UpdateStatus')
END

-- 6. Quản lý khách hàng
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem khách hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem khách hàng', N'Quyền xem danh sách và thông tin khách hàng', 'Customer', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm khách hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm khách hàng', N'Quyền thêm khách hàng mới', 'Customer', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa khách hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa khách hàng', N'Quyền chỉnh sửa thông tin khách hàng', 'Customer', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa khách hàng')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa khách hàng', N'Quyền xóa khách hàng', 'Customer', 'Delete')
END

-- 7. Quản lý tài khoản admin
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem tài khoản admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem tài khoản admin', N'Quyền xem danh sách tài khoản admin', 'AdminAccount', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm tài khoản admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm tài khoản admin', N'Quyền thêm tài khoản admin mới', 'AdminAccount', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa tài khoản admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa tài khoản admin', N'Quyền chỉnh sửa tài khoản admin', 'AdminAccount', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa tài khoản admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa tài khoản admin', N'Quyền xóa tài khoản admin', 'AdminAccount', 'Delete')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Phê duyệt tài khoản admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Phê duyệt tài khoản admin', N'Quyền phê duyệt tài khoản admin', 'AdminAccount', 'Approve')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Khóa/Mở khóa tài khoản admin')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Khóa/Mở khóa tài khoản admin', N'Quyền khóa hoặc mở khóa tài khoản admin', 'AdminAccount', 'Block')
END

-- 8. Quản lý phân quyền
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem role')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem role', N'Quyền xem danh sách các role', 'Role', 'View')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Thêm role')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Thêm role', N'Quyền tạo role mới', 'Role', 'Create')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Sửa role')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Sửa role', N'Quyền chỉnh sửa role', 'Role', 'Edit')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xóa role')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xóa role', N'Quyền xóa role', 'Role', 'Delete')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Gán quyền cho role')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Gán quyền cho role', N'Quyền gán quyền cho role', 'Role', 'AssignPermissions')
END

-- 9. Thống kê báo cáo
IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem thống kê doanh thu')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem thống kê doanh thu', N'Quyền xem thống kê doanh thu', 'Statistics', 'ViewRevenue')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xem thống kê sản phẩm')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xem thống kê sản phẩm', N'Quyền xem thống kê sản phẩm', 'Statistics', 'ViewProducts')
END

IF NOT EXISTS (SELECT * FROM [PhoneStore].[dbo].[Permissions] WHERE [Name] = 'Xuất báo cáo')
BEGIN
    INSERT INTO [PhoneStore].[dbo].[Permissions] ([Name], [Description], [Area], [Action]) 
    VALUES ('Xuất báo cáo', N'Quyền xuất báo cáo', 'Statistics', 'ExportReports')
END

-- 10. Tạo các role mặc định và gán quyền tương ứng

-- SuperAdmin đã có và có toàn quyền
DECLARE @SuperAdminRoleId INT
SELECT @SuperAdminRoleId = RoleId FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'SuperAdmin'

-- Admin role (nếu chưa có)
DECLARE @AdminRoleId INT
SELECT @AdminRoleId = RoleId FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'Admin'

-- User role (nếu chưa có)
DECLARE @UserRoleId INT
SELECT @UserRoleId = RoleId FROM [PhoneStore].[dbo].[Roles] WHERE [RoleName] = 'User'

-- Gán toàn quyền cho SuperAdmin (trừ các quyền đã có)
INSERT INTO [PhoneStore].[dbo].[RolePermissions] (RolesRoleId, PermissionsPermissionId)
SELECT @SuperAdminRoleId, p.PermissionId
FROM [PhoneStore].[dbo].[Permissions] p
WHERE NOT EXISTS (
    SELECT 1 FROM [PhoneStore].[dbo].[RolePermissions] rp 
    WHERE rp.RolesRoleId = @SuperAdminRoleId AND rp.PermissionsPermissionId = p.PermissionId
);

-- Gán quyền cho Admin (quyền xem tất cả, thêm/sửa/xóa phần lớn ngoại trừ phân quyền)
INSERT INTO [PhoneStore].[dbo].[RolePermissions] (RolesRoleId, PermissionsPermissionId)
SELECT @AdminRoleId, p.PermissionId
FROM [PhoneStore].[dbo].[Permissions] p
WHERE 
    (p.Action = 'View' OR p.Area != 'Role' AND p.Area != 'AdminAccount') 
    AND NOT EXISTS (
        SELECT 1 FROM [PhoneStore].[dbo].[RolePermissions] rp 
        WHERE rp.RolesRoleId = @AdminRoleId AND rp.PermissionsPermissionId = p.PermissionId
    );

-- Gán quyền cho User (chỉ xem, không sửa gì cả)
INSERT INTO [PhoneStore].[dbo].[RolePermissions] (RolesRoleId, PermissionsPermissionId)
SELECT @UserRoleId, p.PermissionId
FROM [PhoneStore].[dbo].[Permissions] p
WHERE 
    p.Action = 'View' AND p.Area != 'Role' AND p.Area != 'AdminAccount'
    AND NOT EXISTS (
        SELECT 1 FROM [PhoneStore].[dbo].[RolePermissions] rp 
        WHERE rp.RolesRoleId = @UserRoleId AND rp.PermissionsPermissionId = p.PermissionId
    );
