using System;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class ProjectAssignment
    {
        public int AssignmentId { get; set; }

        // Foreign Keys
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        // Metadata
        public DateTime AssignedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Project Project { get; set; } = null!;
        public User User { get; set; } = null!;

        // ==============================
        // 🔹 Many-to-Many: Assignment ↔ Role (ProjectRoles)
        // ==============================
        public ICollection<ProjectRole> ProjectRoles { get; set; } = new List<ProjectRole>();
    }
}