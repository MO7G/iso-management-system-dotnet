using System;
using System.Collections.Generic;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class StandardTemplate
    {
        public int TemplateID { get; set; }
        public int SectionID { get; set; }
        public int FileID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation
        public StandardSection Section { get; set; } = null!;
        public FileStorage File { get; set; } = null!;
        public ICollection<ProjectDocuments> ProjectDocuments { get; set; } = new List<ProjectDocuments>();
    }
}