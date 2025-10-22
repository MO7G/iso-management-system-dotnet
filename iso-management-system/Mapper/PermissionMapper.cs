using System.Linq;
using iso_management_system.Dto.Permission;
using iso_management_system.DTOs;
using iso_management_system.Models;

namespace iso_management_system.Mappers;

public static class PermissionMapper
{
    public static PermissionResponseDTO ToResponseDTO(Permission permission)
    {
        return new PermissionResponseDTO
        {
            PermissionID = permission.PermissionID,
            PermissionName = permission.PermissionName,
            Description = permission.Description,
            CreatedAt = permission.CreatedAt,
            ModifiedAt = permission.ModifiedAt,
            RoleNames = permission.Roles?.Select(r => r.RoleName).ToList()
        };
    }

    public static Permission ToEntity(PermissionRequestDTO dto)
    {
        return new Permission
        {
            PermissionName = dto.PermissionName,
            Description = dto.Description,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }
}