using System;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;

namespace PermissionSeeder
{
    class VerifyProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Verifying permissions data...");
            
            try
            {
                // Create connection string
                var connectionString = "Server=127.0.0.1,1433;Database=PhoneStore;User Id=sa;Password=Khoa0606@;TrustServerCertificate=True;";
                
                // Create DbContext
                var optionsBuilder = new DbContextOptionsBuilder<PhoneStoreContext>();
                optionsBuilder.UseSqlServer(connectionString);
                
                using (var context = new PhoneStoreContext(optionsBuilder.Options))
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
                    
                    // Count RolePermissions by role
                    var rolePermissionCounts = context.Database
                        .ExecuteSqlRaw("SELECT RolesRoleId, COUNT(*) FROM RolePermissions GROUP BY RolesRoleId");
                    
                    // Run direct SQL queries to get counts
                    var superAdminCount = context.Database.ExecuteSqlRaw(
                        "SELECT COUNT(*) FROM RolePermissions WHERE RolesRoleId = 1");
                    var adminCount = context.Database.ExecuteSqlRaw(
                        "SELECT COUNT(*) FROM RolePermissions WHERE RolesRoleId = 2");
                    var userCount = context.Database.ExecuteSqlRaw(
                        "SELECT COUNT(*) FROM RolePermissions WHERE RolesRoleId = 3");
                    
                    Console.WriteLine("\nPermissions per role (based on ExecuteSqlRaw return value):");
                    Console.WriteLine($"- SuperAdmin: {superAdminCount}");
                    Console.WriteLine($"- Admin: {adminCount}");
                    Console.WriteLine($"- User: {userCount}");
                    
                    // Run COUNT query directly using FromSqlRaw with a result type
                    Console.WriteLine("\nVerifying if roles exist:");
                    var roles = context.Roles.ToList();
                    foreach (var role in roles)
                    {
                        Console.WriteLine($"- Role: {role.RoleName} (ID: {role.RoleId})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
