using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;

namespace PhoneStore
{
    public class PermissionSeeder
    {
        // Removed Main method to avoid entry point conflicts
        
        public static void SeedPermissions()
        {
            try
            {
                // Sử dụng connection string từ appsettings.json
                var optionsBuilder = new DbContextOptionsBuilder<PhoneStoreContext>();
                optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=PhoneStore;User Id=sa;Password=Khoa0606@;TrustServerCertificate=True;");
                
                using (var context = new PhoneStoreContext(optionsBuilder.Options))
                {
                    Console.WriteLine("Bắt đầu thêm dữ liệu vào bảng Permissions và RolePermissions...");
                    
                    // Xóa dữ liệu cũ nếu có
                    var rolePermissions = context.Set<Dictionary<string, object>>("RolePermissions").ToList();
                    if (rolePermissions.Any())
                    {
                        Console.WriteLine("Xóa dữ liệu cũ trong bảng RolePermissions...");
                        context.Database.ExecuteSqlRaw("DELETE FROM RolePermissions");
                    }
                    
                    var permissions = context.Permissions.ToList();
                    if (permissions.Any())
                    {
                        Console.WriteLine("Xóa dữ liệu cũ trong bảng Permissions...");
                        context.Permissions.RemoveRange(permissions);
                        context.SaveChanges();
                    }
                    
                    // Thêm dữ liệu vào bảng Permissions
                    Console.WriteLine("Thêm dữ liệu vào bảng Permissions...");
                    var newPermissions = new List<Permission>
                    {
                        // Quyền quản lý sản phẩm
                        new Permission { PermissionId = 1, Name = "ViewProducts", Description = "Xem danh sách sản phẩm", Area = "Product", Action = "Index" },
                        new Permission { PermissionId = 2, Name = "CreateProduct", Description = "Thêm sản phẩm mới", Area = "Product", Action = "Create" },
                        new Permission { PermissionId = 3, Name = "EditProduct", Description = "Sửa sản phẩm", Area = "Product", Action = "Edit" },
                        new Permission { PermissionId = 4, Name = "DeleteProduct", Description = "Xóa sản phẩm", Area = "Product", Action = "Delete" },
                        
                        // Quyền quản lý danh mục
                        new Permission { PermissionId = 5, Name = "ViewCategories", Description = "Xem danh sách danh mục", Area = "Category", Action = "Index" },
                        new Permission { PermissionId = 6, Name = "CreateCategory", Description = "Thêm danh mục mới", Area = "Category", Action = "Create" },
                        new Permission { PermissionId = 7, Name = "EditCategory", Description = "Sửa danh mục", Area = "Category", Action = "Edit" },
                        new Permission { PermissionId = 8, Name = "DeleteCategory", Description = "Xóa danh mục", Area = "Category", Action = "Delete" },
                        
                        // Quyền quản lý màu sắc
                        new Permission { PermissionId = 9, Name = "ViewColors", Description = "Xem danh sách màu sắc", Area = "Color", Action = "Index" },
                        new Permission { PermissionId = 10, Name = "CreateColor", Description = "Thêm màu sắc mới", Area = "Color", Action = "Create" },
                        new Permission { PermissionId = 11, Name = "EditColor", Description = "Sửa màu sắc", Area = "Color", Action = "Edit" },
                        new Permission { PermissionId = 12, Name = "DeleteColor", Description = "Xóa màu sắc", Area = "Color", Action = "Delete" },
                        
                        // Quyền quản lý giảm giá
                        new Permission { PermissionId = 13, Name = "ViewDiscounts", Description = "Xem chương trình giảm giá", Area = "Discount", Action = "Index" },
                        new Permission { PermissionId = 14, Name = "CreateDiscount", Description = "Thêm chương trình giảm giá", Area = "Discount", Action = "Create" },
                        new Permission { PermissionId = 15, Name = "EditDiscount", Description = "Sửa chương trình giảm giá", Area = "Discount", Action = "Edit" },
                        new Permission { PermissionId = 16, Name = "DeleteDiscount", Description = "Xóa chương trình giảm giá", Area = "Discount", Action = "Delete" },
                        
                        // Quyền quản lý đơn hàng
                        new Permission { PermissionId = 17, Name = "ViewOrders", Description = "Xem danh sách đơn hàng", Area = "Order", Action = "Index" },
                        new Permission { PermissionId = 18, Name = "CreateOrder", Description = "Tạo đơn hàng mới", Area = "Order", Action = "Create" },
                        new Permission { PermissionId = 19, Name = "EditOrder", Description = "Cập nhật đơn hàng", Area = "Order", Action = "Edit" },
                        new Permission { PermissionId = 20, Name = "DeleteOrder", Description = "Hủy đơn hàng", Area = "Order", Action = "Delete" },
                        
                        // Quyền quản lý khách hàng
                        new Permission { PermissionId = 21, Name = "ViewCustomers", Description = "Xem danh sách khách hàng", Area = "Customer", Action = "Index" },
                        new Permission { PermissionId = 22, Name = "CreateCustomer", Description = "Thêm khách hàng mới", Area = "Customer", Action = "Create" },
                        new Permission { PermissionId = 23, Name = "EditCustomer", Description = "Sửa thông tin khách hàng", Area = "Customer", Action = "Edit" },
                        new Permission { PermissionId = 24, Name = "DeleteCustomer", Description = "Xóa khách hàng", Area = "Customer", Action = "Delete" },
                        
                        // Quyền quản lý tài khoản quản trị
                        new Permission { PermissionId = 25, Name = "ViewAdmins", Description = "Xem danh sách tài khoản quản trị", Area = "AdminAccount", Action = "Index" },
                        new Permission { PermissionId = 26, Name = "CreateAdmin", Description = "Thêm tài khoản quản trị mới", Area = "AdminAccount", Action = "Register" },
                        new Permission { PermissionId = 27, Name = "EditAdmin", Description = "Sửa tài khoản quản trị", Area = "AdminAccount", Action = "Edit" },
                        new Permission { PermissionId = 28, Name = "DeleteAdmin", Description = "Xóa tài khoản quản trị", Area = "AdminAccount", Action = "Delete" },
                        
                        // Quyền quản lý phân quyền
                        new Permission { PermissionId = 29, Name = "ViewRoles", Description = "Xem danh sách vai trò", Area = "Role", Action = "View" },
                        new Permission { PermissionId = 30, Name = "CreateRole", Description = "Tạo vai trò mới", Area = "Role", Action = "Create" },
                        new Permission { PermissionId = 31, Name = "EditRole", Description = "Sửa vai trò", Area = "Role", Action = "Edit" },
                        new Permission { PermissionId = 32, Name = "DeleteRole", Description = "Xóa vai trò", Area = "Role", Action = "Delete" },
                        
                        // Quyền quản lý dashboard
                        new Permission { PermissionId = 33, Name = "ViewDashboard", Description = "Xem trang tổng quan", Area = "Dashboard", Action = "Index" },
                        new Permission { PermissionId = 34, Name = "GenerateFakeOrders", Description = "Tạo đơn hàng mẫu", Area = "Dashboard", Action = "GenerateFakeOrders" }
                    };
                    
                    context.Permissions.AddRange(newPermissions);
                    context.SaveChanges();
                    
                    // Lấy các vai trò từ cơ sở dữ liệu
                    var superAdminRole = context.Roles.FirstOrDefault(r => r.RoleName == "SuperAdmin");
                    var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
                    var userRole = context.Roles.FirstOrDefault(r => r.RoleName == "User");
                    
                    if (superAdminRole == null || adminRole == null || userRole == null)
                    {
                        Console.WriteLine("Không tìm thấy các vai trò cần thiết. Hãy đảm bảo rằng các vai trò đã được thêm vào cơ sở dữ liệu.");
                        return;
                    }
                    
                    // Thêm quyền cho SuperAdmin - tất cả các quyền
                    Console.WriteLine("Gán quyền cho vai trò SuperAdmin...");
                    foreach (var permission in context.Permissions.ToList())
                    {
                        superAdminRole.Permissions.Add(permission);
                    }
                    
                    // Thêm quyền cho Admin - tất cả quyền ngoại trừ quản lý phân quyền và quản trị viên
                    Console.WriteLine("Gán quyền cho vai trò Admin...");
                    foreach (var permission in context.Permissions.Where(p => 
                        p.Area != "Role" && 
                        (p.Area != "AdminAccount" || p.Action == "Profile")).ToList())
                    {
                        adminRole.Permissions.Add(permission);
                    }
                    
                    // Thêm quyền cho User - chỉ quyền xem
                    Console.WriteLine("Gán quyền cho vai trò User...");
                    foreach (var permission in context.Permissions.Where(p => 
                        p.Action == "Index" || p.Action == "View").ToList())
                    {
                        userRole.Permissions.Add(permission);
                    }
                    
                    context.SaveChanges();
                    
                    Console.WriteLine("Hoàn thành việc thêm dữ liệu vào bảng Permissions và RolePermissions!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
