using System;
using System.Collections.Generic;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        // DB columns
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? CompletionDate { get; set; }

        // Foreign Keys
        public int CustomerId { get; set; }
        public int StatusID { get; set; }
        public int StandardID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Customer Customer { get; set; } = null!;
        public ProjectStatus ProjectStatus { get; set; } = null!;
        public Standard Standard { get; set; } = null!;

        public ICollection<ProjectAssignments> ProjectAssignments { get; set; } = new List<ProjectAssignments>();
        public ICollection<ProjectDocuments> ProjectDocuments { get; set; } = new List<ProjectDocuments>();
        public ICollection<DocumentRevision> DocumentRevisions { get; set; } = new List<DocumentRevision>();
    }
}