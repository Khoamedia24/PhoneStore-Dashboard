-- Verify Permissions data
SELECT 'Total permissions: ' + CAST(COUNT(*) AS VARCHAR) FROM Permissions;

-- Sample of permissions
SELECT TOP 5 PermissionId, Name, Area, Action FROM Permissions;

-- Count permissions per role
SELECT 'SuperAdmin permissions: ' + CAST(COUNT(*) AS VARCHAR) 
FROM RolePermissions 
WHERE RolesRoleId = 1;

SELECT 'Admin permissions: ' + CAST(COUNT(*) AS VARCHAR) 
FROM RolePermissions 
WHERE RolesRoleId = 2;

SELECT 'User permissions: ' + CAST(COUNT(*) AS VARCHAR) 
FROM RolePermissions 
WHERE RolesRoleId = 3;

-- Verify roles exist
SELECT RoleId, RoleName FROM Roles;
