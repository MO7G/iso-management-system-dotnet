using iso_management_system.Dto.User;
using iso_management_system.Helpers;
using iso_management_system.Models;

namespace iso_management_system.Mappers;

public static class UserMapper
{
    public static UserResponseDTO ToResponseDTO(User user)
    {
        return new UserResponseDTO
        {
            Id = user.UserID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles?.Select(r => r.RoleName).ToList()
        };
    }

     
    public static User ToEntity(UserRequestDTO dto)
    {
        return new User
        {   FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = PasswordHelper.HashPassword(dto.Password)
        };
    }
}