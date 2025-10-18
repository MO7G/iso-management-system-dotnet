using iso_management_system.Configurations.Db;
using iso_management_system.DTOs;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    
    public IEnumerable<Role> getAllRoles()
    {
        Console.WriteLine("Get all roles");
        return _roleRepository.GetAllRoles();
    }
    
    
    public RoleResponseDTO GetRoleById(int roleId)
    {
        var role = _roleRepository.GetRoleById(roleId);
        return RoleMapper.ToResponseDTO(role);
    }
    
    
    public RoleResponseDTO CreateRole(RoleRequestDTO roleRequest)
    {
        var role = RoleMapper.ToEntity(roleRequest); // Map DTO → entity
        _roleRepository.AddRole(role);               // Save to DB
        return RoleMapper.ToResponseDTO(role);       // Map entity → response DTO
    }
    
    
    public void DeleteRole(int roleId)
    {
        var role = _roleRepository.GetRoleById(roleId);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");

        // Business rule: cannot delete role assigned to users
        if (role.UserRoleAssignments != null && role.UserRoleAssignments.Any())
            throw new InvalidOperationException("Cannot delete role because it is assigned to one or more users.");
        
        _roleRepository.DeleteRole(role);
    }

}