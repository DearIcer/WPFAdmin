using System.ComponentModel.DataAnnotations;

namespace Backed.Domain.Entities;

public class Role : EntityBase
{
    [MaxLength(50)] public required string Name { get; set; }

    [MaxLength(200)] public string? Description { get; set; }

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}