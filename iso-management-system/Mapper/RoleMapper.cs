using System.Linq;
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
                Permissions = role.RolePermissionMappings
                    .Select(rp => PermissionMapper.ToResponseDTO(rp.Permission))
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