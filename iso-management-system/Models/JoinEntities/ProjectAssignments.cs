using System;
using System.Collections.Generic;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class ProjectAssignments
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
        public ICollection<ProjectRoles> ProjectRoles { get; set; } = new List<ProjectRoles>();
    }
}