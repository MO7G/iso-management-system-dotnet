using System;

namespace iso_management_system.Dto.FileStorage
{
    public class FileStorageResponseDTO
    {
        public int FileID { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? UserID { get; set; }
        public int? CustomerID { get; set; }
    }
}
