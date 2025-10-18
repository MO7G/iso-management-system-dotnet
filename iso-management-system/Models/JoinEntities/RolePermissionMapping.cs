namespace iso_management_system.Models.JoinEntities;

public class RolePermissionMapping
{
    public int RoleID { get; set; }
    public Role Role { get; set; } = null!;

    public int PermissionID { get; set; }
    public Permission Permission { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}