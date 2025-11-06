namespace Backed.Domain.Entities;

// Many-to-many relationship between Role and Permission
public class RolePermission : EntityBase
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }

    // Navigation properties
    public Role Role { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}