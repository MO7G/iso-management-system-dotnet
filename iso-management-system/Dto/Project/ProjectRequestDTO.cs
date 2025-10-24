using System.ComponentModel.DataAnnotations;

namespace iso_management_system.Dto.Project;

public class ProjectRequestDTO
{
    [Required]
    public int? CustomerID { get; set; }

    [Required]
    public int? StandardID { get; set; }
}
