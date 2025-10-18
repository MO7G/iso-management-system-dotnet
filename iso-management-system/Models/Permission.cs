using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models;

public class Permission
{
    public int PermissionID { get; set; }
    public string PermissionName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<RolePermissionMapping> RolePermissionMappings { get; set; } = new List<RolePermissionMapping>();
}