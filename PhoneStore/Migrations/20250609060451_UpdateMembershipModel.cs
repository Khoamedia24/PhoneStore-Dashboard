using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembershipModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Memberships",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Memberships",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: true);            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumSpend",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MinimumSpend",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Memberships");
        }
    }
}
