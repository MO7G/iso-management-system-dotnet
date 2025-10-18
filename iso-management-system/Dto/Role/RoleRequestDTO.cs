using System.ComponentModel.DataAnnotations;

namespace iso_management_system.DTOs
{
    public class RoleRequestDTO
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(100, ErrorMessage = "Role name can't be longer than 100 characters")]
        public string RoleName { get; set; }

        
        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string? Description { get; set; } // optional
    }
}