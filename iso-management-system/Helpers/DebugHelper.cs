using System;
using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Helpers
{
    public static class DebugPrinter
    {
        public static void PrintRole(Role role)
        {
            if (role == null)
            {
                Console.WriteLine("Role is null");
                return;
            }

            Console.WriteLine($"RoleID: {role.RoleID}, Name: {role.RoleName}, Description: {role.Description}");

            // Assigned Users
            if (role.Users != null && role.Users.Count > 0)
            {
                Console.WriteLine("  Assigned Users:");
                foreach (var user in role.Users)
                {
                    Console.WriteLine($"    UserID: {user.UserID}, Name: {user.FirstName} {user.LastName}, Email: {user.Email}");
                }
            }
            else
            {
                Console.WriteLine("  No users assigned");
            }

            // Permissions
            if (role.Permissions != null && role.Permissions.Count > 0)
            {
                Console.WriteLine("  Permissions:");
                foreach (var perm in role.Permissions)
                {
                    Console.WriteLine($"    PermissionID: {perm.PermissionID}, Name: {perm.PermissionName}");
                }
            }
            else
            {
                Console.WriteLine("  No permissions assigned");
            }

            Console.WriteLine("------------");
        }

        public static void PrintRoles(IEnumerable<Role> roles)
        {
            if (roles == null) 
            {
                Console.WriteLine("Roles list is null");
                return;
            }

            foreach (var role in roles)
            {
                PrintRole(role);
            }
        }
    }
}