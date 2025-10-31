using iso_management_system.Attributes;

namespace iso_management_system.Dto.Stander;

[AtLeastOneFieldRequired]
public class StandardUpdateDTO
{
    public string? Name { get; set; }
    public bool NameHasValue { get; set; }

    public string? Version { get; set; }
    public bool VersionHasValue { get; set; }
    
}