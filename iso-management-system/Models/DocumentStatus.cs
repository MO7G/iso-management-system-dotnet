using System;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class DocumentStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ProjectDocuments> ProjectDocuments { get; set; } = new List<ProjectDocuments>();
    }
}