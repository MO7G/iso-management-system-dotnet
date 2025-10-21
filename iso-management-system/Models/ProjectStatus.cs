using System;
using System.Collections.Generic;

namespace iso_management_system.Models
{
    public class ProjectStatus
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        
      

        
        // Navigation property
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}