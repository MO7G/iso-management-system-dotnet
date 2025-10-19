using System.Collections.Generic;
using iso_management_system.Dto.Permission;

namespace iso_management_system.DTOs
{
    public class RoleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }

        // List of permissions associated with the role
        public List<PermissionResponseDTO> Permissions { get; set; } = new List<PermissionResponseDTO>();
    }

    
}