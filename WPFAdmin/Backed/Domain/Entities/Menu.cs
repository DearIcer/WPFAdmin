using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backed.Domain.Entities;

public class Menu : EntityBase
{
    [MaxLength(50)] 
    public required string Name { get; set; }
    
    [MaxLength(50)]
    public required string Code { get; set; }
    
    [MaxLength(100)]
    public string? Path { get; set; }
    
    [MaxLength(50)]
    public string? Icon { get; set; }
    
    public int? ParentId { get; set; }
    
    public int SortOrder { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    [JsonIgnore]
    [ForeignKey("ParentId")]
    public virtual Menu? Parent { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Menu> Children { get; set; } = new List<Menu>();
    
    [JsonIgnore]
    public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}