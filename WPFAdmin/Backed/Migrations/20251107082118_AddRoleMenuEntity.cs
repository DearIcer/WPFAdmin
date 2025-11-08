using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backed.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleMenuEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenus_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3213), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3214) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3217), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3218) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3221), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3221) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3224), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3224) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3226), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3227) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3229), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3230) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3232), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3233) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3235), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3235) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3238), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3238) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3241), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3241) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5143), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5144) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5149), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5149) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5152), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5152) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5155), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5155) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3105), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3106) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3109), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3111), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3112) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3114), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3114) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3116), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3116) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3160), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3161) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3164), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3164) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3166), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3166) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3168), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3168) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3172), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3173) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3174), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3175) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3170), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(3171) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(2848), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(2849) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(2851), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(2852) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5302), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5302) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5208), new DateTime(2025, 11, 7, 8, 21, 15, 84, DateTimeKind.Utc).AddTicks(5209) });

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_MenuId",
                table: "RoleMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_RoleId_MenuId",
                table: "RoleMenus",
                columns: new[] { "RoleId", "MenuId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMenus");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7847), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7848) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7851), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7851) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7854), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7854) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7857), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7857) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7860), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7860) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7863), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7864) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7866), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7867) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7869), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7870) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7872), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7872) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7875), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7876) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7878), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7879) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7881), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7882) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7884), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7884) });

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7887), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7888) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7753), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7754) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7757), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7757) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7759), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7759) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7761), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7762) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7764), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7764) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7793), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7795), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7796) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7798), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7798) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7800), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7801) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7806), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7806) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7808), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7808) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7803), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7803) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7532), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7532) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7535), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7536) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7956), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7956) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7910), new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7911) });
        }
    }
}
