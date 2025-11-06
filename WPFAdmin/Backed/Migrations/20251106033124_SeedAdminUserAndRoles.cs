using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backed.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4954), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4955) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4958), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4958) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4960), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4961) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4962), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4963) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4990), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4990) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4993), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4993) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4995), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4995) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4997), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4997) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4999), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5000) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5001), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5002) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4659), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4660) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4662), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(4663) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5017), "admin@example.com", true, "21232f297a57a5a743894a0e4a801fc3", new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5017), "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedAt", "RoleId", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5058), 1, new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5059), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(924), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(925) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(928), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(929) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(931), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(931) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(933), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(934) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(958), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(958) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(960), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(961) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(962), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(963) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(965), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(965) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(967), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(967) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(1008), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(1008) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(675), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(679) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(681), new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(682) });
        }
    }
}
