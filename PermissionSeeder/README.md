# Permission Seeder

This is a utility application for seeding the Permissions and RolePermissions tables in the PhoneStore database.

## Purpose

The PermissionSeeder populates the Permissions table with predefined permission records and assigns these permissions to different roles (SuperAdmin, Admin, User) according to their access levels.

## What It Does

1. **Deletes Existing Data**: 
   - Removes all existing records from the RolePermissions and Permissions tables to ensure a clean slate.

2. **Adds Permission Records**:
   - Uses `IDENTITY_INSERT` to add permissions with specific IDs.
   - Includes permissions for managing Products, Categories, Colors, Discounts, Orders, Customers, Admin accounts, Roles, and Dashboard.
   - Each permission has an ID, Name, Description, Area, and Action.

3. **Assigns Permissions to Roles**:
   - **SuperAdmin (RoleId = 1)**: Gets all permissions (34 total).
   - **Admin (RoleId = 2)**: Gets all permissions except those for Role management and AdminAccount management (26 total).
   - **User (RoleId = 3)**: Gets only view permissions (Index/View actions, 9 total).

## How to Run

```bash
cd /path/to/PhoneStore/PermissionSeeder
dotnet run
```

## Verification

After running the seeder, you can verify the data was correctly inserted using the DbVerifier tool:

```bash
cd /path/to/PhoneStore/DbVerifier
dotnet run
```

## Database Connection

The application connects to a SQL Server database using the following connection string:
```
Server=127.0.0.1,1433;Database=PhoneStore;User Id=sa;Password=Khoa0606@;TrustServerCertificate=True;
```

## Technical Implementation

- Uses Entity Framework Core with raw SQL commands for precise control.
- Implements a transaction to ensure the `IDENTITY_INSERT` operations are atomic.
- Individual `INSERT` statements for each permission record to avoid batch issues.
