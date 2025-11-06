namespace Backed.Domain.Entities;

// Many-to-many relationship between User and Role
public class UserRole : EntityBase
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}