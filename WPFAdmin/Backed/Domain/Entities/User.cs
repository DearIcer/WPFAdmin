using System.ComponentModel.DataAnnotations;

namespace Backed.Domain.Entities;

public class User : EntityBase
{
    [MaxLength(50)] public required string Username { get; set; }

    [MaxLength(100)] public required string Email { get; set; }

    [MaxLength(256)] public required string PasswordHash { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}