using Backed.Domain.Entities;

namespace Backed.Domain.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user, string password);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<bool> UserExistsAsync(string username);
    Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(int userId);
}