
namespace iso_management_system.Models;

public class User
{
    public int UserID { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    // Navigation
    // ==============================
    // 🔹 Many-to-Many: User ↔ Role (implicit)
    // ==============================
    public ICollection<Role> Roles { get; set; }

    // ==============================
    // 🔹 Many-to-Many: User ↔ Project (explicit via ProjectAssignment)
    // ==============================
    public ICollection<ProjectAssignment> ProjectAssignments { get; set; }

    // ==============================
    // 🔹 One-to-Many: User → DocumentRevision
    // ==============================
    public ICollection<DocumentRevision> DocumentRevisions { get; set; }

    // ==============================
    // 🔹 One-to-Many: User → FileStorage
    // ==============================
    public ICollection<FileStorage> UploadedFiles { get; set; }
    
    
}