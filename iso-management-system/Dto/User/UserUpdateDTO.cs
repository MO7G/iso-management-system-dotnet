using iso_management_system.Attributes;

namespace iso_management_system.Dto.User;


[AtLeastOneFieldRequired]
public class UserUpdateDTO
{
    public string? FirstName { get; set; }
    public bool FirstNameHasValue { get; set; }

    public string? LastName { get; set; }
    public bool LastNameHasValue { get; set; }

    public string? Email { get; set; }
    public bool EmailHasValue { get; set; }

    public bool? IsActive { get; set; }
    public bool IsActiveHasValue { get; set; }
}
