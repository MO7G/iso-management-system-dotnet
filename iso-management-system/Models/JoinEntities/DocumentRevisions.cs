using System;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class DocumentRevision
    {
        public int RevisionID { get; set; }

        // Foreign Keys
        public int ProjectDocumentID { get; set; }
        public int FileID { get; set; }
        public int? ModifiedByUserID { get; set; }

        // Metadata
        public int VersionNumber { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ChangeNote { get; set; }

        // Navigation Properties
        public ProjectDocuments ProjectDocument { get; set; } = null!;
        public FileStorage File { get; set; } = null!;
        public User? ModifiedByUser { get; set; }
    }
}