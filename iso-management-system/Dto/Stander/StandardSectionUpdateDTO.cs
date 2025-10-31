using iso_management_system.Attributes;

namespace iso_management_system.Dto.Stander;

[AtLeastOneFieldRequired]
public class StandardSectionUpdateDTO
{
    public string? Title { get; set; }
    public bool TitleHasValue { get; set; }

    public string? Number { get; set; }
    public bool NumberHasValue { get; set; }

    // public int? ParentSectionID { get; set; }
    // public bool ParentSectionIDHasValue { get; set; }
    //
    // public int? OrderIndex { get; set; }
    // public bool OrderIndexHasValue { get; set; }
}