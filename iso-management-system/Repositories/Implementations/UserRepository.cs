using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Dto.General;
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
    
    
    
    public IEnumerable<User> GetAllUsers(int pageNumber, int pageSize, out int totalRecords)
    {
        var query = _context.Users
            .Include(u => u.Roles)
            .AsNoTracking();

        totalRecords = query.Count();

        return query
            .OrderBy(u => u.UserID)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
    
    public IEnumerable<User> SearchUsers(
        string? query,
        int pageNumber,
        int pageSize,
        SortingParameters sorting, // add sorting
        out int totalRecords)
    {
        var baseQuery = _context.Users
            .Include(u => u.Roles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            baseQuery = baseQuery.Where(u =>
                u.FirstName.Contains(query) ||
                u.LastName.Contains(query) ||
                u.Email.Contains(query));
        }

        // Apply dynamic sorting
        baseQuery = sorting.SortBy?.ToLower() switch
        {
            "firstname" => sorting.SortDirection.ToLower() == "desc" 
                ? baseQuery.OrderByDescending(u => u.FirstName) 
                : baseQuery.OrderBy(u => u.FirstName),
            "lastname" => sorting.SortDirection.ToLower() == "desc" 
                ? baseQuery.OrderByDescending(u => u.LastName) 
                : baseQuery.OrderBy(u => u.LastName),
            "email" => sorting.SortDirection.ToLower() == "desc" 
                ? baseQuery.OrderByDescending(u => u.Email) 
                : baseQuery.OrderBy(u => u.Email),
            _ => sorting.SortDirection.ToLower() == "desc" 
                ? baseQuery.OrderByDescending(u => u.UserID) 
                : baseQuery.OrderBy(u => u.UserID),
        };

        totalRecords = baseQuery.Count();

        return baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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
    public void UpdateUser(User user)
    {
        var tracked = _context.Users.Local.FirstOrDefault(u => u.UserID == user.UserID);
        if (tracked == null)
        {
            _context.Users.Attach(user);
        }

        _context.Entry(user).State = EntityState.Modified;
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