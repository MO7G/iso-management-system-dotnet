using System;
using System.Collections.Generic;
using System.Text.Json;
using iso_management_system.Models;

namespace iso_management_system.Helpers
{
    public static class DebugHelper
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
        
        
       public static async Task PrintRequestData(HttpRequest request)
        {
            Console.WriteLine("====== Debugging Request ======");
            Console.WriteLine($"Method: {request.Method}");
            Console.WriteLine($"Path: {request.Path}");
            Console.WriteLine($"Content-Type: {request.ContentType}");
            Console.WriteLine();

            // Query parameters
            if (request.Query.Any())
            {
                Console.WriteLine(">>> Query Parameters:");
                foreach (var kvp in request.Query)
                    Console.WriteLine($"   - {kvp.Key}: {kvp.Value}");
                Console.WriteLine();
            }

            // Route values
            if (request.RouteValues.Any())
            {
                Console.WriteLine(">>> Route Values:");
                foreach (var kvp in request.RouteValues)
                    Console.WriteLine($"   - {kvp.Key}: {kvp.Value}");
                Console.WriteLine();
            }

            // Headers
            if (request.Headers.Any())
            {
                Console.WriteLine(">>> Headers:");
                foreach (var kvp in request.Headers)
                    Console.WriteLine($"   - {kvp.Key}: {kvp.Value}");
                Console.WriteLine();
            }

            // Body (formatted JSON if possible)
            if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                try
                {
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(body);
                    var formattedJson = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                    Console.WriteLine(">>> Body (JSON):");
                    Console.WriteLine(formattedJson);
                }
                catch
                {
                    // Not valid JSON
                    Console.WriteLine(">>> Body (Raw):");
                    Console.WriteLine(body);
                }

                Console.WriteLine();
            }

            Console.WriteLine("====== End of Request ======\n");
        }
        
       
    }
}