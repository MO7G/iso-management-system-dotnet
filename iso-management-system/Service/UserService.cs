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
        var user = _userRepository.GetUserByIdNotTracked(userId);

        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        return UserMapper.ToResponseDTO(user);
    }

    
    
    public async Task<UserResponseDTO> CreateUserAsync(UserRequestDTO userRequest)
    {
        if (await _userRepository.EmailExistsAsync(userRequest.Email))
            throw new BusinessRuleException("A user with this email already exists.");

        var user = UserMapper.ToEntity(userRequest);
        await _userRepository.AddUserAsync(user);

        return UserMapper.ToResponseDTO(user);
    }



    /// <summary>
    /// Updates an existing user's profile with the provided fields.
    /// </summary>
    /// <remarks>
    /// This method performs a **partial update**, applying only the properties explicitly sent
    /// in the <see cref="UserUpdateDTO"/>. Each property in the DTO includes a corresponding
    /// `HasValue` flag that indicates whether the client intended to modify that field.
    ///
    /// Validation and null-safety are enforced at the DTO level to ensure
    /// only meaningful, non-null values are applied. The method updates the `ModifiedAt`
    /// timestamp and persists the changes asynchronously.
    ///
    /// Exceptions:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="NotFoundException"/> — Thrown if the user does not exist.</description>
    /// </item>
    /// </list>
    ///
    /// Design notes:
    /// - Promotes **idempotency** and **explicit intent** by updating only specified fields.
    /// - Uses pattern matching for clean and expressive null/value checks.
    /// - Delegates persistence to the repository to preserve separation of concerns.
    ///
    /// </remarks>
    /// <param name="userId">The unique identifier of the user to update.</param>
    /// <param name="dto">The DTO containing the fields to update and their HasValue flags.</param>
    /// <returns>A <see cref="UserResponseDTO"/> representing the updated user state.</returns>
    public async Task<UserResponseDTO> UpdateUserAsync(int userId, UserUpdateDTO dto)
    {
        // Retrieve the existing user
        var user = _userRepository.GetUserByIdNotTracked(userId);
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
 