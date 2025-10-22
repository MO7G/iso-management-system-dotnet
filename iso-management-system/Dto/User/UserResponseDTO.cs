namespace iso_management_system.Dto.User;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public List<string>? Roles { get; set; }
}