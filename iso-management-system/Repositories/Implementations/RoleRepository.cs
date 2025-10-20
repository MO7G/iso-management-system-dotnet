using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using iso_management_system.Configurations.Db; // for AppDbContext
using Microsoft.EntityFrameworkCore;

namespace iso_management_system.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.ToList(); // simple query to get all roles
        }

        public Role? GetRoleById(int id)
        {
            return _context.Roles
                .Include(r => r.RolePermissionMappings)
                .ThenInclude(rp => rp.Permission) // load permissions of the mappings
                .Include(r => r.UserRoleAssignments) // load assigned users
                .FirstOrDefault(r => r.RoleID == id);
        }


        
        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        
        
        public void DeleteRole(Role role)
        {
            _context.Roles.Remove(role);
            _context.SaveChanges();
        }

        public bool RoleNameExists(string roleName)
        {
            bool exists = (from r in _context.Roles
                where r.RoleName.ToLower() == roleName.ToLower()
                select r).Any();

            return exists;
        }

    }
}