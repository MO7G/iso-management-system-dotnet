using System.Collections.Generic;
using iso_management_system.Dto.General;
using iso_management_system.Models;

namespace iso_management_system.models;


public interface IUserRepository
{
    IEnumerable<User> GetAllUsers(int pageNumber, int pageSize, out int totalRecords);
    User GetUserByIdNotTracked(int userId);
    Task<bool> EmailExistsAsync(string email);
    Task AddUserAsync(User user);

    Task<bool> UserExistsAsync(int userId);
    Task<bool> HasRolesAsync(int userId);
    Task<bool> HasProjectAssignmentsAsync(int userId);
    Task DeleteUserByIdAsync(int userId);


    Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    IEnumerable<User> SearchUsers(string? query, int pageNumber, int pageSize, SortingParameters sorting , out int totalRecords);
    
}
