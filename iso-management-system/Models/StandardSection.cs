using System;
using System.Collections.Generic;

namespace iso_management_system.Models
{
    public class StandardSection
    {
        public int SectionID { get; set; }
        public int StandardID { get; set; }
        public int? ParentSectionID { get; set; }
        public string Number { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int OrderIndex { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation
        public Standard Standard { get; set; } = null!;
        public StandardSection? ParentSection { get; set; }
        public ICollection<StandardSection> ChildSections { get; set; } = new List<StandardSection>();
        public ICollection<StandardTemplate> Templates { get; set; } = new List<StandardTemplate>();
    }
}