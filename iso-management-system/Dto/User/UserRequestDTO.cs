namespace iso_management_system.Dto.User;

public class UserRequestDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

