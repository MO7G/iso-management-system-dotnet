namespace iso_management_system.Models;

public class Customer
{
    public int CustomerID { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    // Navigation Properties

    // One-to-Many: Customer → Projects
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    // One-to-Many: Customer → FileStorage (uploaded by customer)
    public ICollection<FileStorage> UploadedFiles { get; set; } = new List<FileStorage>();
}