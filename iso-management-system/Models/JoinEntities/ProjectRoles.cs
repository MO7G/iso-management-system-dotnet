using System;

namespace iso_management_system.Models.JoinEntities;

public class ProjectRole
{
    // Composite key: AssignmentId + RoleId
    public int AssignmentId { get; set; }
    public int RoleId { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    // Navigation
    public ProjectAssignment ProjectAssignment { get; set; } = null!;
    public Role Role { get; set; } = null!;
}