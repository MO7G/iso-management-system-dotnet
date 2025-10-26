using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iso_management_system.Repositories.Implementations;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Permission> GetAllPermissions()
    {
        return _context.Permissions
            .Include(p => p.Roles)
            .AsNoTracking()
            .ToList();
    }

    public Permission GetPermissionById(int permissionId)
    {
        return _context.Permissions
            .Include(p => p.Roles)
            //.AsNoTracking()  <-- REMOVE this
            .FirstOrDefault(p => p.PermissionID == permissionId);
    }


    public bool PermissionNameExists(string permissionName)
    {
        return _context.Permissions.Any(p => p.PermissionName.ToLower() == permissionName.ToLower());
    }

    public void AddPermission(Permission permission)
    {
        _context.Permissions.Add(permission);
        _context.SaveChanges();
    }

    public Permission GetPermissionWithRoles(int permissionId)
    {
        return _context.Permissions
            .Include(p => p.Roles)
            .FirstOrDefault(p => p.PermissionID == permissionId);
    }

    public void DeletePermission(Permission permission)
    {
        _context.Permissions.Remove(permission);
        _context.SaveChanges();
    }
}