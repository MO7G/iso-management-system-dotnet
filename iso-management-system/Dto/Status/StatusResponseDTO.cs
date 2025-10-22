namespace iso_management_system.Dto.Status;



public class StatusResponseDTO
{
    public int StatusId { get; set; }
    public string StatusName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}