using Backed.Domain.Entities;

namespace Backed.Domain.Interfaces;

public interface IRoleService
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role> CreateAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int id);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId);
    Task AssignPermissionAsync(int roleId, int permissionId);
    Task RemovePermissionAsync(int roleId, int permissionId);
    
    // Role-menu operations
    Task<IEnumerable<Menu>> GetRoleMenusAsync(int roleId);
    Task AssignMenuAsync(int roleId, int menuId);
    Task RemoveMenuAsync(int roleId, int menuId);
}