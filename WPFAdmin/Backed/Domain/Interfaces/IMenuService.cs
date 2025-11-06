using Backed.Domain.Entities;

namespace Backed.Domain.Interfaces;

public interface IMenuService
{
    Task<IEnumerable<Menu>> GetAllAsync();
    Task<Menu?> GetByIdAsync(int id);
    Task<Menu> CreateAsync(Menu menu);
    Task UpdateAsync(Menu menu);
    Task DeleteAsync(int id);
    Task<IEnumerable<Menu>> GetActiveMenusAsync();
    Task<IEnumerable<Menu>> GetMenuTreeAsync();
}