using iso_management_system.Dto.Permission;
using iso_management_system.DTOs;
using iso_management_system.Models;

namespace iso_management_system.Mappers
{
    public static class PermissionMapper
    {
        public static PermissionResponseDTO ToResponseDTO(Permission permission)
        {
            if (permission == null) return null;

            return new PermissionResponseDTO
            {
                Id = permission.PermissionID,
                Name = permission.PermissionName
            };
        }
    }
}