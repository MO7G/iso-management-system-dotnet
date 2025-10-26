using System.Collections.Generic;
using iso_management_system.Models;


namespace iso_management_system.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role? GetRoleById(int id);
        Role? GetRoleByIdWithPermissions(int id);
        void AddRole(Role role);
        public void DeleteRole(Role role);
        
        bool RoleNameExists(string roleName);
        // void UpdateRole(Role role);
        void AttachEntity<T>(T entity) where T : class;

        public Role? GetRoleWithUsers(int id);

        void SaveChanges();

    }
}