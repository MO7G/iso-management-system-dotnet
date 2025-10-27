using Microsoft.AspNetCore.Http;

namespace iso_management_system.Dto.Project;

public class FileUploadCustomerRequestDTO
{
    public int CustomerID { get; set; }             // ID of the uploading customer
    public int ProjectDocumentID { get; set; }      // ID of the project document to update
    public IFormFile File { get; set; } = null!;   // Uploaded file
    public string? ChangeNote { get; set; }        // Optional note about this version
}