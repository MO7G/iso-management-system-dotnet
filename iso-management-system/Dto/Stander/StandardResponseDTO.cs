namespace iso_management_system.Dto.Stander;

public class StandardResponseDTO
{
    public int StandardID { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public DateTime? PublishedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}