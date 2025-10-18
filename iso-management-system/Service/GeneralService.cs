using iso_management_system.Configurations.Db;
using iso_management_system.Models;

namespace iso_management_system.Services
{
    public class GeneralService
    {
        private readonly AppDbContext _dbContext;

        public GeneralService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Example: get all users
        public IEnumerable<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        // Example: get all roles
        public IEnumerable<Role> GetAllRoles()
        {
            return _dbContext.Roles.ToList();
        }

        // You can add more general methods here
    }
}