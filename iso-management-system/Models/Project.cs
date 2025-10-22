using System;
using System.Collections.Generic;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }

        // Foreign Keys
        public int CustomerId { get; set; }
        public int ProjectStatusId { get; set; }
        public int StandardID { get; set; } // <--- Add this

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public Customer Customer { get; set; } = null!;
        public ProjectStatus ProjectStatus { get; set; } = null!;
        public Standard Standard { get; set; } = null!; // <--- Add this

        // Collections
        public ICollection<ProjectAssignments> ProjectAssignments { get; set; } = new List<ProjectAssignments>();
        public ICollection<ProjectDocuments> ProjectDocuments { get; set; } = new List<ProjectDocuments>();
        public ICollection<DocumentRevision> DocumentRevisions { get; set; } = new List<DocumentRevision>();
    }
}