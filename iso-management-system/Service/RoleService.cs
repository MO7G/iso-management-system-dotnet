using System;
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Helpers;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    
    public RoleService(IRoleRepository roleRepository , IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }
    
    
    public IEnumerable<RoleResponseDTO> getAllRoles()
    {
        Console.WriteLine("Get all roles");

        // Get all Role entities from the repository
        IEnumerable<Role> roles = _roleRepository.GetAllRoles();
        
        // DebugPrinter.PrintRoles(roles);

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
        var role = _roleRepository.GetRoleWithUsers(roleId);
        if (role == null)
            throw new NotFoundException($"Role with ID {roleId} not found.");
        
        
        // Business rule: cannot delete role assigned to users
        if (role.Users.Any()) // use the navigation collection, not UserRoleAssignments
        {
            Console.WriteLine("Role is assigned to users, cannot delete.");
            throw new BusinessRuleException("Cannot delete role because it is assigned to users.");
        }
        _roleRepository.DeleteRole(role);
    }
    // === Add permission to role ===
    public void AddPermissionToRole(int roleId, int permissionId)
    {
        var role = _roleRepository.GetRoleByIdWithPermissions(roleId);
        if (role == null)
            throw new NotFoundException($"Role with ID {roleId} not found.");

        var permission = _permissionRepository.GetPermissionById(permissionId);
        if (permission == null)
            throw new NotFoundException($"Permission with ID {permissionId} not found.");

        // Check if permission is already assigned
        if (role.Permissions.Any(p => p.PermissionID == permissionId))
            throw new BusinessRuleException("Permission already assigned to role.");

        
        // Add permission
        role.Permissions.Add(permission);
        _roleRepository.SaveChanges(); // Add this method in RoleRepository to call _context.SaveChanges()
    }

    // === Remove permission from role ===
    public void RemovePermissionFromRole(int roleId, int permissionId)
    {
        var role = _roleRepository.GetRoleByIdWithPermissions(roleId);
        if (role == null)
            throw new NotFoundException($"Role with ID {roleId} not found.");

        var permission = role.Permissions.FirstOrDefault(p => p.PermissionID == permissionId);
        if (permission == null)
            throw new BusinessRuleException("Permission not assigned to this role.");

        // Remove permission
        role.Permissions.Remove(permission);
        _roleRepository.SaveChanges();
    }
}