namespace iso_management_system.Dto.Stander;

public class StandardSectionRequestDTO
{
    public int StandardID { get; set; }
    public int? ParentSectionID { get; set; }
    public string Number { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int OrderIndex { get; set; } = 0;
}