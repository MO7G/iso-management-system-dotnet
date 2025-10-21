using iso_management_system.Models;


namespace iso_management_system.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role? GetRoleById(int id);
        void AddRole(Role role);
        public void DeleteRole(Role role);
        
        bool RoleNameExists(string roleName);
        // void UpdateRole(Role role);

        public Role? GetRoleWithUsers(int id);


    }
}