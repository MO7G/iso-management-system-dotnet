using System;
using System.Collections.Generic;

namespace iso_management_system.Models.JoinEntities
{
    public class ProjectDocuments
    {
        public int ProjectDocumentID { get; set; }

        // Foreign Keys
        public int ProjectId { get; set; }
        public int TemplateID { get; set; }
        public int FileID { get; set; }
        public int StatusID { get; set; }
        public int? LastModifiedBy { get; set; }

        // Metadata
        public int VersionNumber { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Project Project { get; set; } = null!;
        public StandardTemplate Template { get; set; } = null!;
        public FileStorage File { get; set; } = null!;
        public DocumentStatus Status { get; set; } = null!;
        public User? LastModifiedUser { get; set; }

        // 🔹 One-to-Many: ProjectDocument → DocumentRevisions
        public ICollection<DocumentRevision> DocumentRevisions { get; set; } = new List<DocumentRevision>();
    }
}