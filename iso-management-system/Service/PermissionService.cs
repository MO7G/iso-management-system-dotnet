using System.Collections.Generic;
using System.Linq;
using iso_management_system.Dto.Permission;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services;

public class PermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public IEnumerable<PermissionResponseDTO> GetAllPermissions()
    {
        var permissions = _permissionRepository.GetAllPermissions();
        return permissions.Select(PermissionMapper.ToResponseDTO);
    }

    public PermissionResponseDTO GetPermissionById(int permissionId)
    {
        var permission = _permissionRepository.GetPermissionById(permissionId);
        if (permission == null)
            throw new NotFoundException($"Permission with ID {permissionId} not found.");

        return PermissionMapper.ToResponseDTO(permission);
    }

    public PermissionResponseDTO CreatePermission(PermissionRequestDTO dto)
    {
        if (_permissionRepository.PermissionNameExists(dto.PermissionName))
            throw new BusinessRuleException("A permission with this name already exists.");

        var permission = PermissionMapper.ToEntity(dto);
        _permissionRepository.AddPermission(permission);
        return PermissionMapper.ToResponseDTO(permission);
    }

    public void DeletePermission(int permissionId)
    {
        var permission = _permissionRepository.GetPermissionWithRoles(permissionId);
        if (permission == null)
            throw new NotFoundException($"Permission with ID {permissionId} not found.");

        // Business rule: do not allow deletion if assigned to roles
        if (permission.Roles.Any())
            throw new BusinessRuleException("Cannot delete a permission that is assigned to roles.");

        _permissionRepository.DeletePermission(permission);
    }
}
