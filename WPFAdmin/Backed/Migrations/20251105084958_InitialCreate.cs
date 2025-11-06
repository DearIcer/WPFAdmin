using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backed.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(924), "Can view dashboard", "ViewDashboard", new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(925) },
                    { 2, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(928), "Can manage users", "ManageUsers", new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(929) },
                    { 3, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(931), "Can manage roles", "ManageRoles", new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(931) },
                    { 4, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(933), "Can manage products", "ManageProducts", new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(934) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(675), "Administrator role with full access", "Admin", new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(679) },
                    { 2, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(681), "Regular user role", "User", new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(682) }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(958), 1, 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(958) },
                    { 2, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(960), 2, 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(961) },
                    { 3, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(962), 3, 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(963) },
                    { 4, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(965), 4, 1, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(965) },
                    { 5, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(967), 1, 2, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(967) },
                    { 6, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(1008), 4, 2, new DateTime(2025, 11, 5, 8, 49, 57, 531, DateTimeKind.Utc).AddTicks(1008) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
