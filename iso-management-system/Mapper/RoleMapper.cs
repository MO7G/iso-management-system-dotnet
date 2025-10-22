using System.Linq;
using iso_management_system.Dto.Permission;
using iso_management_system.DTOs;
using iso_management_system.Models;

namespace iso_management_system.Mappers
{
    public static class RoleMapper
    {
        public static RoleResponseDTO ToResponseDTO(Role role)
        {
            if (role == null) return null;

            return new RoleResponseDTO
            {
                Id = role.RoleID,
                Name = role.RoleName,
                Description = role.Description,
                Permissions = role.Permissions
                    .Select(p => new PermissionResponseDTO
                    {
                        PermissionID = p.PermissionID,
                        PermissionName = p.PermissionName
                    })
                    .ToList()
            };
        }
        
        public static Role ToEntity(RoleRequestDTO roleRequest)
        {
            if (roleRequest == null) return null;

            return new Role
            {
                RoleName = roleRequest.RoleName,
                Description = roleRequest.Description // include description
            };
        }
    }
}