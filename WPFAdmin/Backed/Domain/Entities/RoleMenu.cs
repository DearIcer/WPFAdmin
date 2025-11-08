namespace Backed.Domain.Entities;

// Many-to-many relationship between Role and Menu
public class RoleMenu : EntityBase
{
    public int RoleId { get; set; }
    public int MenuId { get; set; }

    // Navigation properties
    public Role Role { get; set; } = default!;
    public Menu Menu { get; set; } = default!;
}