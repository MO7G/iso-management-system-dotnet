using iso_management_system.Models;

namespace iso_management_system.models;


public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(int userId);
    bool EmailExists(string email);
    void AddUser(User user);
    User GetUserWithRoles(int userId);
    void DeleteUser(User user);
}
