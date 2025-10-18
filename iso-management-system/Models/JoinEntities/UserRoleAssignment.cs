namespace iso_management_system.Models.JoinEntities;

public class UserRoleAssignment
{
    public int UserID { get; set; }
    public User User { get; set; } = null!;

    public int RoleID { get; set; }
    public Role Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}