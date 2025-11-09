using Backed.Domain.Entities;
using Backed.Domain.Interfaces;
using Backed.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Backed.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user == null)
            return null;
        
        var hashedPassword = ComputeMd5Hash(password);
        if (user.PasswordHash != hashedPassword)
            return null;

        return user;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(User user, string password)
    {
        // Hash the password using MD5
        user.PasswordHash = ComputeMd5Hash(password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        // 先从数据库获取现有用户实体
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser != null)
        {
            // 更新现有实体的属性，而不是附加新实体
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;
            existingUser.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException($"User with ID {user.Id} not found");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(int userId)
    {
        var roleIds = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        return await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .Distinct()
            .ToListAsync();
    }
    
    // User-role operations
    public async Task AssignRoleAsync(int userId, int roleId)
    {
        var existing = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (existing == null)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveRoleAsync(int userId, int roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole != null)
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
    
    // User-menu operations
    public async Task<IEnumerable<Menu>> GetUserMenusAsync(int userId)
    {
        // 获取用户的角色ID
        var roleIds = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        // 获取角色关联的菜单ID（包括所有父级菜单）
        var directMenuIds = await _context.RoleMenus
            .Where(rm => roleIds.Contains(rm.RoleId))
            .Select(rm => rm.MenuId)
            .Distinct()
            .ToListAsync();

        // 获取所有菜单以构建完整的菜单树
        var allMenus = await _context.Menus
            .Include(m => m.Children)
            .Where(m => m.IsActive)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();

        // 构建完整的菜单ID集合，包括所有父级菜单
        var allRelatedMenuIds = new HashSet<int>(directMenuIds);
        var menuDict = allMenus.ToDictionary(m => m.Id);

        // 添加所有父级菜单ID到集合中
        foreach (var menuId in directMenuIds)
        {
            var currentMenuId = menuId;
            while (menuDict.ContainsKey(currentMenuId) && menuDict[currentMenuId].ParentId.HasValue)
            {
                var parentId = menuDict[currentMenuId].ParentId.Value;
                allRelatedMenuIds.Add(parentId);
                currentMenuId = parentId;
            }
        }

        // 过滤出用户有权访问的菜单（包括所有相关的父级菜单）
        var userMenus = allMenus.Where(m => allRelatedMenuIds.Contains(m.Id)).ToList();

        // 构建菜单树结构
        var topLevelMenus = userMenus.Where(m => !m.ParentId.HasValue || !allRelatedMenuIds.Contains(m.ParentId.Value)).ToList();
        var userMenuDict = userMenus.ToDictionary(m => m.Id);

        // 清除子菜单集合以确保干净状态
        foreach (var menu in userMenus)
        {
            menu.Children.Clear();
        }

        // 构建父子关系
        foreach (var menu in userMenus)
        {
            if (menu.ParentId.HasValue && userMenuDict.ContainsKey(menu.ParentId.Value))
            {
                userMenuDict[menu.ParentId.Value].Children.Add(menu);
            }
        }

        return topLevelMenus;
    }

    // 计算MD5哈希值
    private string ComputeMd5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}