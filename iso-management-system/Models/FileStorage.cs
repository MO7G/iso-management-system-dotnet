using System;
using System.Collections.Generic;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Models
{
    public class FileStorage
    {
        public int FileID { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public long? FileSize { get; set; }

        // Foreign Keys
        public int? UploadedByUserID { get; set; }
        public int? UploadedByCustomerID { get; set; }

        // Metadata
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User? UploadedByUser { get; set; }
        public Customer? UploadedByCustomer { get; set; }

        // Navigation collections for other relationships (if needed)
        public ICollection<DocumentRevision> DocumentRevisions { get; set; } = new List<DocumentRevision>();
        public ICollection<ProjectDocuments> ProjectDocuments { get; set; } = new List<ProjectDocuments>();
        public ICollection<StandardTemplate> StandardTemplates { get; set; } = new List<StandardTemplate>();
    }
}