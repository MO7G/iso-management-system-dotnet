using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.models;
using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
namespace iso_management_system.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    
    public IEnumerable<User> GetAllUsers()
    {
        // Eager-load related roles for full data consistency
        return _context.Users
            .Include(u => u.Roles)
            .AsNoTracking()
            .ToList();
    }

    
    
    public User GetUserById(int userId)
    {
        return _context.Users
            .Include(u => u.Roles)
            .AsNoTracking()
            .FirstOrDefault(u => u.UserID == userId);
    }

    
    
    public bool EmailExists(string email)
    {
        return _context.Users.Any(u => u.Email.ToLower() == email.ToLower());
    }

    
    
    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    
    
    public Models.User GetUserWithRoles(int userId)
    {
        return _context.Users
            .Include(u => u.Roles)
            .Include(u => u.ProjectAssignments)
            .FirstOrDefault(u => u.UserID == userId);
    }

    
    
    public void DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}