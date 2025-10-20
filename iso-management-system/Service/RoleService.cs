using iso_management_system.Configurations.Db;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
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
    
    
    public IEnumerable<RoleResponseDTO> getAllRoles()
    {
        Console.WriteLine("Get all roles");

        // Get all Role entities from the repository
        IEnumerable<Role> roles = _roleRepository.GetAllRoles();
        
        // Use the mapper to convert each Role to RoleResponseDTO
        var roleDTOs = roles.Select(RoleMapper.ToResponseDTO);

        return roleDTOs;
    }
    
    
    public RoleResponseDTO GetRoleById(int roleId)
    {
        var role = _roleRepository.GetRoleById(roleId);

        if (role == null)
            throw new NotFoundException($"Role with ID {roleId} not found."); // use existing exception

        return RoleMapper.ToResponseDTO(role);
    }

    
    
    public RoleResponseDTO CreateRole(RoleRequestDTO roleRequest)
    {   
        bool roleNameExists = _roleRepository.RoleNameExists(roleRequest.RoleName);
        if (roleNameExists)
        {
            throw new BusinessRuleException("Role with name already exists.");
        }
        var role = RoleMapper.ToEntity(roleRequest); // Map DTO → entity
        _roleRepository.AddRole(role);               // Save to DB
        return RoleMapper.ToResponseDTO(role);       // Map entity → response DTO
    }
    
    
    public void DeleteRole(int roleId)
    {
        var role = _roleRepository.GetRoleById(roleId);
        if (role == null)
            throw new NotFoundException($"Role with ID {roleId} not found.");

        // Business rule: cannot delete role assigned to users
        if (role.UserRoleAssignments.Any())
        {
            Console.WriteLine("Delete role assignments not working");
            throw new BusinessRuleException("Cannot delete role because it is assigned to users.");

        }

        _roleRepository.DeleteRole(role);
    }

}