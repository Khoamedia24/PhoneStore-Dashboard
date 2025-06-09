using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhoneStore.Models;

namespace PermissionSeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            SeedPermissions();
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        static void SeedPermissions()
        {
            Console.WriteLine("Starting to add data to Permissions and RolePermissions tables...");
            
            try 
            {
                // Create connection string
                var connectionString = "Server=127.0.0.1,1433;Database=PhoneStore;User Id=sa;Password=Khoa0606@;TrustServerCertificate=True;";
                
                // Create DbContext
                var optionsBuilder = new DbContextOptionsBuilder<PhoneStoreContext>();
                optionsBuilder.UseSqlServer(connectionString);
                
                using (var context = new PhoneStoreContext(optionsBuilder.Options))
                {// Delete existing data
                    Console.WriteLine("Deleting existing data from RolePermissions table...");
                    context.Database.ExecuteSqlRaw("DELETE FROM RolePermissions");
                    
                    Console.WriteLine("Deleting existing data from Permissions table...");
                    context.Database.ExecuteSqlRaw("DELETE FROM Permissions");
                    
                    // Add data to Permissions table with IDENTITY_INSERT enabled
                    Console.WriteLine("Adding data to Permissions table...");
                    
                    // Use a transaction to ensure all statements execute in the same connection
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {                            // Enable IDENTITY_INSERT
                            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Permissions ON");
                            
                            // Insert Permissions data
                            InsertPermissions(context);
                            
                            // Disable IDENTITY_INSERT
                            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Permissions OFF");
                            
                            // Commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception innerEx)
                        {
                            // Roll back the transaction in case of an error
                            transaction.Rollback();
                            Console.WriteLine($"Transaction error: {innerEx.Message}");
                            throw;
                        }
                    }
                    // Add data to RolePermissions table
                    Console.WriteLine("Adding data to RolePermissions table...");
                    
                    // Assign permissions to SuperAdmin (RoleId = 1)
                    Console.WriteLine("Assigning permissions to SuperAdmin...");
                    context.Database.ExecuteSqlRaw("INSERT INTO RolePermissions (PermissionsPermissionId, RolesRoleId) SELECT PermissionId, 1 FROM Permissions");
                    
                    // Assign permissions to Admin (RoleId = 2)
                    Console.WriteLine("Assigning permissions to Admin...");
                    context.Database.ExecuteSqlRaw(@"
                    INSERT INTO RolePermissions (PermissionsPermissionId, RolesRoleId)
                    SELECT PermissionId, 2 FROM Permissions 
                    WHERE Area NOT IN ('Role', 'AdminAccount') OR (Area = 'AdminAccount' AND Action = 'Profile')");
                    
                    // Assign permissions to User (RoleId = 3)
                    Console.WriteLine("Assigning permissions to User...");
                    context.Database.ExecuteSqlRaw(@"
                    INSERT INTO RolePermissions (PermissionsPermissionId, RolesRoleId)
                    SELECT PermissionId, 3 FROM Permissions 
                    WHERE Action = 'Index' OR Action = 'View'");
                      Console.WriteLine("Successfully completed adding data to Permissions and RolePermissions tables!");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
        
        private static void InsertPermissions(PhoneStoreContext context)
        {
            // Product permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (1, 'ViewProducts', N'Xem danh sách sản phẩm', 'Product', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (2, 'CreateProduct', N'Thêm sản phẩm mới', 'Product', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (3, 'EditProduct', N'Sửa sản phẩm', 'Product', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (4, 'DeleteProduct', N'Xóa sản phẩm', 'Product', 'Delete')");
            
            // Category permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (5, 'ViewCategories', N'Xem danh sách danh mục', 'Category', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (6, 'CreateCategory', N'Thêm danh mục mới', 'Category', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (7, 'EditCategory', N'Sửa danh mục', 'Category', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (8, 'DeleteCategory', N'Xóa danh mục', 'Category', 'Delete')");
            
            // Color permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (9, 'ViewColors', N'Xem danh sách màu sắc', 'Color', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (10, 'CreateColor', N'Thêm màu sắc mới', 'Color', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (11, 'EditColor', N'Sửa màu sắc', 'Color', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (12, 'DeleteColor', N'Xóa màu sắc', 'Color', 'Delete')");
            
            // Discount permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (13, 'ViewDiscounts', N'Xem chương trình giảm giá', 'Discount', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (14, 'CreateDiscount', N'Thêm chương trình giảm giá', 'Discount', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (15, 'EditDiscount', N'Sửa chương trình giảm giá', 'Discount', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (16, 'DeleteDiscount', N'Xóa chương trình giảm giá', 'Discount', 'Delete')");
            
            // Order permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (17, 'ViewOrders', N'Xem danh sách đơn hàng', 'Order', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (18, 'CreateOrder', N'Tạo đơn hàng mới', 'Order', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (19, 'EditOrder', N'Cập nhật đơn hàng', 'Order', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (20, 'DeleteOrder', N'Hủy đơn hàng', 'Order', 'Delete')");
            
            // Customer permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (21, 'ViewCustomers', N'Xem danh sách khách hàng', 'Customer', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (22, 'CreateCustomer', N'Thêm khách hàng mới', 'Customer', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (23, 'EditCustomer', N'Sửa thông tin khách hàng', 'Customer', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (24, 'DeleteCustomer', N'Xóa khách hàng', 'Customer', 'Delete')");
            
            // Admin account permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (25, 'ViewAdmins', N'Xem danh sách tài khoản quản trị', 'AdminAccount', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (26, 'CreateAdmin', N'Thêm tài khoản quản trị mới', 'AdminAccount', 'Register')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (27, 'EditAdmin', N'Sửa tài khoản quản trị', 'AdminAccount', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (28, 'DeleteAdmin', N'Xóa tài khoản quản trị', 'AdminAccount', 'Delete')");
            
            // Role permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (29, 'ViewRoles', N'Xem danh sách vai trò', 'Role', 'View')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (30, 'CreateRole', N'Tạo vai trò mới', 'Role', 'Create')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (31, 'EditRole', N'Sửa vai trò', 'Role', 'Edit')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (32, 'DeleteRole', N'Xóa vai trò', 'Role', 'Delete')");
              // Dashboard permissions
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (33, 'ViewDashboard', N'Xem trang tổng quan', 'Dashboard', 'Index')");
            context.Database.ExecuteSqlRaw(@"INSERT INTO Permissions (PermissionId, Name, Description, Area, Action) VALUES (34, 'GenerateFakeOrders', N'Tạo đơn hàng mẫu', 'Dashboard', 'GenerateFakeOrders')");
        }
    }
}
