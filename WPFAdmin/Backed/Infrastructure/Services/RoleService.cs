using Backed.Domain.Entities;
using Backed.Domain.Interfaces;
using Backed.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backed.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context;

    public RoleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Role> CreateAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task UpdateAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task AssignPermissionAsync(int roleId, int permissionId)
    {
        var existing = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (existing == null)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemovePermissionAsync(int roleId, int permissionId)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (rolePermission != null)
        {
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
        }
    }
    
    // Role-menu operations
    public async Task<IEnumerable<Menu>> GetRoleMenusAsync(int roleId)
    {
        return await _context.RoleMenus
            .Where(rm => rm.RoleId == roleId)
            .Include(rm => rm.Menu)
            .Select(rm => rm.Menu)
            .ToListAsync();
    }

    public async Task AssignMenuAsync(int roleId, int menuId)
    {
        var existing = await _context.RoleMenus
            .FirstOrDefaultAsync(rm => rm.RoleId == roleId && rm.MenuId == menuId);

        if (existing == null)
        {
            var roleMenu = new RoleMenu
            {
                RoleId = roleId,
                MenuId = menuId
            };

            _context.RoleMenus.Add(roleMenu);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveMenuAsync(int roleId, int menuId)
    {
        var roleMenu = await _context.RoleMenus
            .FirstOrDefaultAsync(rm => rm.RoleId == roleId && rm.MenuId == menuId);

        if (roleMenu != null)
        {
            _context.RoleMenus.Remove(roleMenu);
            await _context.SaveChangesAsync();
        }
    }
}