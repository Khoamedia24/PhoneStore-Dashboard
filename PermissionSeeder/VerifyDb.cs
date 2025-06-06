// Simple script to query the database directly
using Microsoft.Data.SqlClient;

// Connection string
string connectionString = "Server=127.0.0.1,1433;Database=PhoneStore;User Id=sa;Password=Khoa0606@;TrustServerCertificate=True;";

Console.WriteLine("Verifying database content...");

try 
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Connected to database successfully!");
        
        // Check permissions count
        using (var command = new SqlCommand("SELECT COUNT(*) FROM Permissions", connection))
        {
            var permissionCount = (int)command.ExecuteScalar();
            Console.WriteLine($"Total permissions in database: {permissionCount}");
        }
        
        // List some permissions
        Console.WriteLine("\nSample permissions:");
        using (var command = new SqlCommand("SELECT TOP 5 PermissionId, Name, Area, Action FROM Permissions", connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["PermissionId"]}: {reader["Name"]} ({reader["Area"]}/{reader["Action"]})");
                }
            }
        }
        
        // Count permissions per role
        Console.WriteLine("\nPermissions per role:");
        using (var command = new SqlCommand(@"
            SELECT r.RoleName, COUNT(rp.PermissionsPermissionId) as PermissionCount
            FROM Roles r
            LEFT JOIN RolePermissions rp ON r.RoleId = rp.RolesRoleId
            GROUP BY r.RoleName", connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["RoleName"]}: {reader["PermissionCount"]} permissions");
                }
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();
