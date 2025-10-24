using System;
using System.ComponentModel.DataAnnotations;

namespace iso_management_system.Dto.Stander;

public class StandardRequestDTO
{
    public string Name { get; set; } = null!;
    
    [Required]
    public string? Version { get; set; }
    public DateTime? PublishedDate { get; set; } 
}