-- Script để thêm dữ liệu vào bảng Permissions và RolePermissions
-- Xóa dữ liệu cũ (nếu cần thiết)
DELETE FROM RolePermissions;
DELETE FROM Permissions;

-- Thêm dữ liệu vào bảng Permissions
INSERT INTO Permissions (PermissionId, Name, Description, Area, Action)
VALUES
-- Quyền quản lý sản phẩm
(1, 'ViewProducts', N'Xem danh sách sản phẩm', 'Product', 'Index'),
(2, 'CreateProduct', N'Thêm sản phẩm mới', 'Product', 'Create'),
(3, 'EditProduct', N'Sửa sản phẩm', 'Product', 'Edit'),
(4, 'DeleteProduct', N'Xóa sản phẩm', 'Product', 'Delete'),

-- Quyền quản lý danh mục
(5, 'ViewCategories', N'Xem danh sách danh mục', 'Category', 'Index'),
(6, 'CreateCategory', N'Thêm danh mục mới', 'Category', 'Create'),
(7, 'EditCategory', N'Sửa danh mục', 'Category', 'Edit'),
(8, 'DeleteCategory', N'Xóa danh mục', 'Category', 'Delete'),

-- Quyền quản lý màu sắc
(9, 'ViewColors', N'Xem danh sách màu sắc', 'Color', 'Index'),
(10, 'CreateColor', N'Thêm màu sắc mới', 'Color', 'Create'),
(11, 'EditColor', N'Sửa màu sắc', 'Color', 'Edit'),
(12, 'DeleteColor', N'Xóa màu sắc', 'Color', 'Delete'),

-- Quyền quản lý giảm giá
(13, 'ViewDiscounts', N'Xem chương trình giảm giá', 'Discount', 'Index'),
(14, 'CreateDiscount', N'Thêm chương trình giảm giá', 'Discount', 'Create'),
(15, 'EditDiscount', N'Sửa chương trình giảm giá', 'Discount', 'Edit'),
(16, 'DeleteDiscount', N'Xóa chương trình giảm giá', 'Discount', 'Delete'),

-- Quyền quản lý đơn hàng
(17, 'ViewOrders', N'Xem danh sách đơn hàng', 'Order', 'Index'),
(18, 'CreateOrder', N'Tạo đơn hàng mới', 'Order', 'Create'),
(19, 'EditOrder', N'Cập nhật đơn hàng', 'Order', 'Edit'),
(20, 'DeleteOrder', N'Hủy đơn hàng', 'Order', 'Delete'),

-- Quyền quản lý khách hàng
(21, 'ViewCustomers', N'Xem danh sách khách hàng', 'Customer', 'Index'),
(22, 'CreateCustomer', N'Thêm khách hàng mới', 'Customer', 'Create'),
(23, 'EditCustomer', N'Sửa thông tin khách hàng', 'Customer', 'Edit'),
(24, 'DeleteCustomer', N'Xóa khách hàng', 'Customer', 'Delete'),

-- Quyền quản lý tài khoản quản trị
(25, 'ViewAdmins', N'Xem danh sách tài khoản quản trị', 'AdminAccount', 'Index'),
(26, 'CreateAdmin', N'Thêm tài khoản quản trị mới', 'AdminAccount', 'Register'),
(27, 'EditAdmin', N'Sửa tài khoản quản trị', 'AdminAccount', 'Edit'),
(28, 'DeleteAdmin', N'Xóa tài khoản quản trị', 'AdminAccount', 'Delete'),

-- Quyền quản lý phân quyền
(29, 'ViewRoles', N'Xem danh sách vai trò', 'Role', 'View'),
(30, 'CreateRole', N'Tạo vai trò mới', 'Role', 'Create'),
(31, 'EditRole', N'Sửa vai trò', 'Role', 'Edit'),
(32, 'DeleteRole', N'Xóa vai trò', 'Role', 'Delete'),

-- Quyền quản lý dashboard
(33, 'ViewDashboard', N'Xem trang tổng quan', 'Dashboard', 'Index'),
(34, 'GenerateFakeOrders', N'Tạo đơn hàng mẫu', 'Dashboard', 'GenerateFakeOrders');

-- Thêm dữ liệu vào bảng RolePermissions - gán quyền cho SuperAdmin (mã 1)
-- SuperAdmin có tất cả các quyền
INSERT INTO RolePermissions (PermissionsPermissionId, RolesRoleId)
SELECT PermissionId, 1 FROM Permissions;

-- Gán quyền cho Admin (mã 2) - Admin có tất cả quyền ngoại trừ quản lý phân quyền và quản trị viên
INSERT INTO RolePermissions (PermissionsPermissionId, RolesRoleId)
SELECT PermissionId, 2 FROM Permissions 
WHERE Area NOT IN ('Role', 'AdminAccount') OR (Area = 'AdminAccount' AND Action = 'Profile');

-- Gán quyền cho User (mã 3) - User chỉ có quyền xem
INSERT INTO RolePermissions (PermissionsPermissionId, RolesRoleId)
SELECT PermissionId, 3 FROM Permissions 
WHERE Action = 'Index' OR Action = 'View';
