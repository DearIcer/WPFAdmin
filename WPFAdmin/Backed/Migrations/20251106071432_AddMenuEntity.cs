using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backed.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Code", "CreatedAt", "Icon", "IsActive", "Name", "ParentId", "Path", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "dashboard", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7847), "ViewDashboard", true, "仪表盘", null, "/dashboard", 1, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7848) },
                    { 2, "products", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7851), "PackageVariant", true, "商品管理", null, "/products", 2, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7851) },
                    { 5, "orders", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7860), "ClipboardList", true, "订单管理", null, "/orders", 3, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7860) },
                    { 8, "members", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7869), "AccountMultiple", true, "会员管理", null, "/members", 4, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7870) },
                    { 11, "settings", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7878), "Cog", true, "系统设置", null, "/settings", 5, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7879) }
                });

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

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Code", "CreatedAt", "Icon", "IsActive", "Name", "ParentId", "Path", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, "product_list", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7854), "FormatListBulleted", true, "商品列表", 2, "/products/list", 1, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7854) },
                    { 4, "product_category", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7857), "Folder", true, "商品分类", 2, "/products/category", 2, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7857) },
                    { 6, "order_list", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7863), "FormatListBulleted", true, "订单列表", 5, "/orders/list", 1, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7864) },
                    { 7, "order_returns", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7866), "PackageDown", true, "退货申请", 5, "/orders/returns", 2, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7867) },
                    { 9, "member_list", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7872), "FormatListBulleted", true, "会员列表", 8, "/members/list", 1, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7872) },
                    { 10, "member_levels", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7875), "Star", true, "会员等级", 8, "/members/levels", 2, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7876) },
                    { 12, "user_management", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7881), "Account", true, "用户管理", 11, "/settings/users", 1, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7882) },
                    { 13, "role_management", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7884), "AccountArrowRight", true, "角色管理", 11, "/settings/roles", 2, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7884) },
                    { 14, "menu_management", new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7887), "Menu", true, "菜单管理", 11, "/settings/menus", 3, new DateTime(2025, 11, 6, 7, 14, 30, 854, DateTimeKind.Utc).AddTicks(7888) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Code",
                table: "Menus",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus");

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

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8953), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8953) });

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
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8993), new DateTime(2025, 11, 6, 6, 41, 8, 287, DateTimeKind.Utc).AddTicks(8994) });

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
        }
    }
}
