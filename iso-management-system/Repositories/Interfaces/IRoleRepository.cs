using iso_management_system.Models;


namespace iso_management_system.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role? GetRoleById(int id);
        void AddRole(Role role);
        public void DeleteRole(Role role);

    }
}