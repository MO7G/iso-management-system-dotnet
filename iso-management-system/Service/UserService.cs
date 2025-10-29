using iso_management_system.Dto.General;
using iso_management_system.Dto.User;
using iso_management_system.Exceptions;
using iso_management_system.Helpers;
using iso_management_system.Mappers;
using iso_management_system.models;

namespace iso_management_system.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;

    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    
    
    public PagedResponse<UserResponseDTO> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = _userRepository.GetAllUsers(pageNumber, pageSize, out int totalRecords);

        var dtoList = users.Select(UserMapper.ToResponseDTO);

        return new PagedResponse<UserResponseDTO>(dtoList, totalRecords, pageNumber, pageSize);
    }

    public PagedResponse<UserResponseDTO> SearchUsers(
        string? query,
        int pageNumber,
        int pageSize,
        SortingParameters sorting) // add sorting
    {
        Console.WriteLine($"PageNumber: {pageNumber}");
        Console.WriteLine($"PageSize: {pageSize}");
        Console.WriteLine($"SortBy: {sorting.SortBy}");
        Console.WriteLine($"SortDirection: {sorting.SortDirection}");

        // Fetch users with sorting
        var users = _userRepository.SearchUsers(query, pageNumber, pageSize, sorting, out int totalRecords);
        var dtoList = users.Select(UserMapper.ToResponseDTO).ToList();

        return new PagedResponse<UserResponseDTO>(dtoList, totalRecords, pageNumber, pageSize);
    }

    
    public UserResponseDTO GetUserById(int userId)
    {
        var user = _userRepository.GetUserById(userId);

        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        return UserMapper.ToResponseDTO(user);
    }

    
    
    public UserResponseDTO CreateUser(UserRequestDTO userRequest)
    {
        // Example business rule: prevent duplicate emails
        bool emailExists = _userRepository.EmailExists(userRequest.Email);
        if (emailExists)
        {
            throw new BusinessRuleException("A user with this email already exists.");
        }

        var user = UserMapper.ToEntity(userRequest);  // DTO → Entity
        _userRepository.AddUser(user);                // Save to DB
        return UserMapper.ToResponseDTO(user);        // Entity → DTO
    }

    
    public async Task<UserResponseDTO> UpdateUserAsync(int userId, UserUpdateDTO dto)
    {
        // Retrieve the existing user
        var user = _userRepository.GetUserById(userId);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        // Apply changes only if explicitly provided and non-null
        if (dto is { FirstNameHasValue: true, FirstName: not null })
            user.FirstName = dto.FirstName;

        if (dto is { LastNameHasValue: true, LastName: not null })
            user.LastName = dto.LastName;

        if (dto is { EmailHasValue: true, Email: not null })
            user.Email = dto.Email;

        if (dto is { IsActiveHasValue: true, IsActive: not null })
            user.IsActive = dto.IsActive.Value;

        // Update modification timestamp
        user.ModifiedAt = DateTime.UtcNow;

        // Save changes asynchronously
        await _userRepository.UpdateUserAsync(user);

        // Map and return the updated DTO
        return UserMapper.ToResponseDTO(user);
    }


    
    
    /// <summary>
    /// Deletes a user by ID after verifying that they are not assigned
    /// to any active roles or project assignments.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <exception cref="NotFoundException">Thrown if the user does not exist.</exception>
    /// <exception cref="BusinessRuleException">
    /// Thrown if the user has assigned roles or active project assignments.
    /// </exception>
    public async Task DeleteUserAsync(int userId)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            throw new NotFoundException($"User with ID {userId} not found.");

        if (await _userRepository.HasRolesAsync(userId))
            throw new BusinessRuleException("Cannot delete user assigned to roles.");

        if (await _userRepository.HasProjectAssignmentsAsync(userId))
            throw new BusinessRuleException("Cannot delete a user assigned to active project.");

        await _userRepository.DeleteUserByIdAsync(userId);
    }

}
 