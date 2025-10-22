namespace iso_management_system.Dto.Stander;

public class StandardRequestDTO
{
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public DateTime? PublishedDate { get; set; }
}