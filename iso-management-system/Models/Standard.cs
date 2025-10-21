using System;
using System.Collections.Generic;

namespace iso_management_system.Models
{
    public class Standard
    {
        public int StandardID { get; set; }
        public string Name { get; set; } = null!;
        public string? Version { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<StandardSection> Sections { get; set; } = new List<StandardSection>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}