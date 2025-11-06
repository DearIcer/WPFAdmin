using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backed.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuManagementPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8942), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8943) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8946), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8946) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8948), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8948) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8950), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8951) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { 5, new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8953), "Can manage menus", "ManageMenus", new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8953) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8984), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8984) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8987), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8987) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8989), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8989) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8991), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8991) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8995), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8996) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8998) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8618), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8619) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8621), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8622) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(9050), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(9051) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(9011), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(9011) });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[] { 7, new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8993), 5, 1, new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8994) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

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

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5058), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5059) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5017), new DateTime(2025, 11, 6, 3, 31, 23, 144, DateTimeKind.Utc).AddTicks(5017) });
        }
    }
}
