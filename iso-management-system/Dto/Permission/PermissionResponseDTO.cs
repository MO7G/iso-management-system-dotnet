using System;
using System.Collections.Generic;

namespace iso_management_system.Dto.Permission;

public class PermissionResponseDTO
{
    public int PermissionID { get; set; }
    public string PermissionName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public List<string>? RoleNames { get; set; }
}

