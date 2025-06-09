using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhoneStore.Migrations
{
    /// <inheritdoc />
    public partial class SeedMembershipData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipID", "CreatedDate", "Description", "DiscountPercentage", "IsActive", "MinimumSpend", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gói thành viên cơ bản cho khách hàng mới", 0m, true, 0m, "Khách hàng thường", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gói thành viên VIP với ưu đãi đặc biệt", 10m, true, 1000000m, "Khách hàng VIP", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gói thành viên cao cấp với nhiều quyền lợi", 15m, true, 5000000m, "Khách hàng Premium", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 3);
        }
    }
}
