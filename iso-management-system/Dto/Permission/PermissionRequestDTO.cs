using System.ComponentModel.DataAnnotations;

namespace iso_management_system.Dto.Permission;

public class PermissionRequestDTO
{
    [Required]
    public string PermissionName { get; set; } = null!;
    public string? Description { get; set; }
}
