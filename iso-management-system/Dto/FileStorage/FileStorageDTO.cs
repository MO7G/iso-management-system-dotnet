
using Microsoft.AspNetCore.Http;
using System;

namespace iso_management_system.Dto.FileStorage

{
    public class FileUploadRequestDTO
    {
        
        
        public IFormFile File { get; set; } = null!;
        public int? UserID { get; set; }
        public int? CustomerID { get; set; }
        
        // here
    }

    
}
