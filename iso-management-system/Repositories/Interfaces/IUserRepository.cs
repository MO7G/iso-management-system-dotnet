using System.Collections.Generic;
using iso_management_system.Dto.General;
using iso_management_system.Models;

namespace iso_management_system.models;


public interface IUserRepository
{
    IEnumerable<User> GetAllUsers(int pageNumber, int pageSize, out int totalRecords);
    User GetUserById(int userId);
    bool EmailExists(string email);
    void AddUser(User user);
    User GetUserWithRoles(int userId);
    void UpdateUser(User user);
    IEnumerable<User> SearchUsers(string? query, int pageNumber, int pageSize, SortingParameters sorting , out int totalRecords);

    void DeleteUser(User user);
}
