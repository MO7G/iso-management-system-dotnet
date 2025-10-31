// Extension method

using System;
using iso_management_system.Configurations.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

public static class DbContextExtensions
{
    
    // An extension method that pretends to be a method from teh AppDbContext but as a helper here 
    public static void EnsureConnection(this AppDbContext db)
    {
        var logger = db.GetService<ILoggerFactory>()?.CreateLogger("DatabaseCheck");

        try
        {
            db.Database.OpenConnection();
            db.Database.CloseConnection();
            logger?.LogInformation(" Database connection successful.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Database connection failed: {Message}", ex.Message);
            throw;
        }
    }
}


