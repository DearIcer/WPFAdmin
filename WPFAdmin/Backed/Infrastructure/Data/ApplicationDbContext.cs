using Backed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backed.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Menu> Menus => Set<Menu>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity => { entity.HasIndex(e => e.Name).IsUnique(); });

        // Configure Permission entity
        modelBuilder.Entity<Permission>(entity => { entity.HasIndex(e => e.Name).IsUnique(); });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => ur.Id);
            entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RolePermission entity
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(rp => rp.Id);
            entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Menu entity
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed default data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed default roles
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1, Name = "Admin", Description = "Administrator role with full access",
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = 2, Name = "User", Description = "Regular user role", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        // Seed default permissions
        modelBuilder.Entity<Permission>().HasData(
            new Permission
            {
                Id = 1, Name = "ViewDashboard", Description = "Can view dashboard", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = 2, Name = "ManageUsers", Description = "Can manage users", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = 3, Name = "ManageRoles", Description = "Can manage roles", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = 4, Name = "ManageProducts", Description = "Can manage products", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = 5, Name = "ManageMenus", Description = "Can manage menus", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        // Assign permissions to roles
        modelBuilder.Entity<RolePermission>().HasData(
            // Admin role has all permissions
            new RolePermission
                { Id = 1, RoleId = 1, PermissionId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new RolePermission
                { Id = 2, RoleId = 1, PermissionId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new RolePermission
                { Id = 3, RoleId = 1, PermissionId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new RolePermission
                { Id = 4, RoleId = 1, PermissionId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new RolePermission
                { Id = 7, RoleId = 1, PermissionId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // User role has limited permissions
            new RolePermission
                { Id = 5, RoleId = 2, PermissionId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new RolePermission
                { Id = 6, RoleId = 2, PermissionId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );

        // Seed default menu items
        modelBuilder.Entity<Menu>().HasData(
            new Menu
            {
                Id = 1,
                Name = "仪表盘",
                Code = "dashboard",
                Path = "/dashboard",
                Icon = "ViewDashboard",
                SortOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 2,
                Name = "商品管理",
                Code = "products",
                Path = "/products",
                Icon = "PackageVariant",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 3,
                Name = "商品列表",
                Code = "product_list",
                Path = "/products/list",
                Icon = "FormatListBulleted",
                ParentId = 2,
                SortOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 4,
                Name = "商品分类",
                Code = "product_category",
                Path = "/products/category",
                Icon = "Folder",
                ParentId = 2,
                SortOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 5,
                Name = "订单管理",
                Code = "orders",
                Path = "/orders",
                Icon = "ClipboardList",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 6,
                Name = "订单列表",
                Code = "order_list",
                Path = "/orders/list",
                Icon = "FormatListBulleted",
                ParentId = 5,
                SortOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 7,
                Name = "退货申请",
                Code = "order_returns",
                Path = "/orders/returns",
                Icon = "PackageDown",
                ParentId = 5,
                SortOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 8,
                Name = "会员管理",
                Code = "members",
                Path = "/members",
                Icon = "AccountMultiple",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 9,
                Name = "会员列表",
                Code = "member_list",
                Path = "/members/list",
                Icon = "FormatListBulleted",
                ParentId = 8,
                SortOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 10,
                Name = "会员等级",
                Code = "member_levels",
                Path = "/members/levels",
                Icon = "Star",
                ParentId = 8,
                SortOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 11,
                Name = "系统设置",
                Code = "settings",
                Path = "/settings",
                Icon = "Cog",
                SortOrder = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 12,
                Name = "用户管理",
                Code = "user_management",
                Path = "/settings/users",
                Icon = "Account",
                ParentId = 11,
                SortOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 13,
                Name = "角色管理",
                Code = "role_management",
                Path = "/settings/roles",
                Icon = "AccountArrowRight",
                ParentId = 11,
                SortOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Menu
            {
                Id = 14,
                Name = "菜单管理",
                Code = "menu_management",
                Path = "/settings/menus",
                Icon = "Menu",
                ParentId = 11,
                SortOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        // Seed default users
        var adminUser = new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = "21232f297a57a5a743894a0e4a801fc3", // MD5 hash of "admin"
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        modelBuilder.Entity<User>().HasData(adminUser);

        // Assign roles to users
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                Id = 1,
                UserId = 1,
                RoleId = 1, // Admin role
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}