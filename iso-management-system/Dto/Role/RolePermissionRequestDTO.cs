using System.ComponentModel.DataAnnotations;

namespace iso_management_system.DTOs;

public class RolePermissionRequestDTO
{
    [Required]
    public int? RoleId { get; set; } 
    
    [Required]
    public int? PermissionId { get; set; }
}