using System.ComponentModel.DataAnnotations;

namespace iso_management_system.Dto.Project;

public class ProjectRoleAssignmentDTO
{
    [Required]
    public int? ProjectId { get; set; }
    
    [Required]
    public int? UserId { get; set; }
    
    [Required]
    public int? RoleId { get; set; }
}