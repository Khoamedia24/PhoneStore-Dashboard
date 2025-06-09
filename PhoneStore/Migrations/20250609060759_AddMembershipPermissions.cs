using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhoneStore.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Action", "Area", "Description", "Name" },
                values: new object[,]
                {
                    { 6, "Index", "Membership", "Xem danh sách gói thành viên", "ViewMemberships" },
                    { 7, "Create", "Membership", "Thêm gói thành viên mới", "CreateMembership" },
                    { 8, "Edit", "Membership", "Sửa gói thành viên", "EditMembership" },
                    { 9, "Delete", "Membership", "Xóa gói thành viên", "DeleteMembership" },
                    { 10, "ToggleStatus", "Membership", "Bật/tắt trạng thái gói thành viên", "ToggleMembershipStatus" },
                    { 11, "GetDetails", "Membership", "Xem chi tiết gói thành viên", "GetMembershipDetails" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 11);
        }
    }
}
