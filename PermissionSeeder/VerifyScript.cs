using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhoneStore.Models;

// Simple program to verify that permissions were properly added to the database
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
        
        // Count role permissions by directly querying RolePermissions
        var superAdminPermissionsCount = context.Database
            .SqlQueryRaw<int>("SELECT COUNT(*) FROM RolePermissions WHERE RolesRoleId = 1")
            .ToList().FirstOrDefault();
        
        var adminPermissionsCount = context.Database
            .SqlQueryRaw<int>("SELECT COUNT(*) FROM RolePermissions WHERE RolesRoleId = 2")
            .ToList().FirstOrDefault();
        
        var userPermissionsCount = context.Database
            .SqlQueryRaw<int>("SELECT COUNT(*) FROM RolePermissions WHERE RolesRoleId = 3")
            .ToList().FirstOrDefault();
        
        Console.WriteLine("\nPermissions per role:");
        Console.WriteLine($"- SuperAdmin: {superAdminPermissionsCount} permissions");
        Console.WriteLine($"- Admin: {adminPermissionsCount} permissions");
        Console.WriteLine($"- User: {userPermissionsCount} permissions");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    }
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();
