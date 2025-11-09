using Backed.Domain.Entities;

namespace Backed.Domain.Interfaces;

public interface IPermissionService
{
    Task<Permission?> GetByIdAsync(int id);
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<Permission> CreateAsync(Permission permission);
    Task UpdateAsync(Permission permission);
    Task DeleteAsync(int id);
}