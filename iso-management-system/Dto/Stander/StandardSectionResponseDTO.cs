using System;
using System.Collections.Generic;

namespace iso_management_system.Dto.Stander;

public class StandardSectionResponseDTO
{
    public int SectionID { get; set; }
    public int StandardID { get; set; }
    public int? ParentSectionID { get; set; }
    public string Number { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<StandardSectionResponseDTO>? Children { get; set; } = new();
}

