using System.Collections.Generic;
using System.Linq;
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
            return _context.Roles
                .Include(r => r.Permissions)
                .ToList(); // simple query to get all roles
        }

        public Role? GetRoleById(int id)
        {
            return _context.Roles
                .Include(r => r.Permissions) // navigation collection on Role
                .FirstOrDefault(r => r.RoleID == id);
        }


        public Role? GetRoleWithUsers(int id)
        {
            return _context.Roles
                .Include(r => r.Users)       // include assigned users
                .Include(r => r.Permissions) // optional, if needed
                .FirstOrDefault(r => r.RoleID == id);
        }

        public void AttachEntity<T>(T entity) where T : class
{
    _context.Attach(entity);
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
        public Role? GetRoleByIdWithPermissions(int id)
        {
            return _context.Roles
                .Include(r => r.Permissions) // Eager load permissions
                .FirstOrDefault(r => r.RoleID == id);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}