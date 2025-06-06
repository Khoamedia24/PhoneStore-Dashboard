using System;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;

namespace PermissionSeeder
{
    class VerifyPermissions
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Verifying permissions data...");
            
            // Create connection string
            var connectionString = "Server=127.0.0.1,1433;Database=PhoneStore;User Id=sa;Password=Khoa0606@;TrustServerCertificate=True;";
            
            // Create DbContext
            var optionsBuilder = new DbContextOptionsBuilder<PhoneStoreContext>();
            optionsBuilder.UseSqlServer(connectionString);
            
            using (var context = new PhoneStoreContext(optionsBuilder.Options))
            {
                try
                {
                    // Count Permissions
                    var permissionsCount = context.Permissions.Count();
                    Console.WriteLine($"Total permissions: {permissionsCount}");
                    
                    // List some permissions
                    Console.WriteLine("\nSample permissions:");
                    var samplePermissions = context.Permissions.Take(5).ToList();
                    foreach (var permission in samplePermissions)
                    {
                        Console.WriteLine($"- {permission.PermissionId}: {permission.Name} ({permission.Area}/{permission.Action})");
                    }
                    
                    // Count role permissions
                    var superAdminPermissionsCount = context.Roles
                        .Include(r => r.Permissions)
                        .FirstOrDefault(r => r.RoleName == "SuperAdmin")
                        ?.Permissions.Count ?? 0;
                    
                    var adminPermissionsCount = context.Roles
                        .Include(r => r.Permissions)
                        .FirstOrDefault(r => r.RoleName == "Admin")
                        ?.Permissions.Count ?? 0;
                    
                    var userPermissionsCount = context.Roles
                        .Include(r => r.Permissions)
                        .FirstOrDefault(r => r.RoleName == "User")
                        ?.Permissions.Count ?? 0;
                    
                    Console.WriteLine("\nPermissions per role:");
                    Console.WriteLine($"- SuperAdmin: {superAdminPermissionsCount} permissions");
                    Console.WriteLine($"- Admin: {adminPermissionsCount} permissions");
                    Console.WriteLine($"- User: {userPermissionsCount} permissions");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                }
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
