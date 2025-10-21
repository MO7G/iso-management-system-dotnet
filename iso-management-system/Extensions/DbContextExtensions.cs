// Extension method

using System;
using iso_management_system.Configurations.Db;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static void EnsureConnection(this AppDbContext db)
    {
        try
        {
            db.Database.OpenConnection();
            db.Database.CloseConnection();
            Console.WriteLine("-------------Database connection successful-------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"----------Database connection failed: {ex.Message}");
           throw;
        }
    }
}

// Usage in Program.cs
