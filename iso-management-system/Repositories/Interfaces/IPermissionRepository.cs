using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces;

public interface IPermissionRepository
{
    IEnumerable<Permission> GetAllPermissions();
    Permission GetPermissionById(int permissionId);
    bool PermissionNameExists(string permissionName);
    void AddPermission(Permission permission);
    Permission GetPermissionWithRoles(int permissionId);
    void DeletePermission(Permission permission);
}