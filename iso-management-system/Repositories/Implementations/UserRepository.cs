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
        switch (sorting.SortBy?.ToLower())
        {
            case "firstname":
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(u => u.FirstName)
                    : baseQuery.OrderBy(u => u.FirstName);
                break;
            case "lastname":
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(u => u.LastName)
                    : baseQuery.OrderBy(u => u.LastName);
                break;
            case "email":
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(u => u.Email)
                    : baseQuery.OrderBy(u => u.Email);
                break;
            default:
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(u => u.UserID)
                    : baseQuery.OrderBy(u => u.UserID);
                break;
        }

        totalRecords = baseQuery.Count();

        return baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToList();
    }

    public User? GetUserByIdNotTracked(int userId)
    {
        return _context.Users
            .Include(u => u.Roles)
            .AsNoTracking()
            .FirstOrDefault(u => u.UserID == userId);
    }

    
    
    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }


    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var tracked = _context.Users.Local.FirstOrDefault(u => u.UserID == user.UserID);
        if (tracked == null)
        {
            _context.Users.Attach(user);
        }

        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync(cancellationToken);
    }

    
    
   
    
    
    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users.AsNoTracking()
            .AnyAsync(u => u.UserID == userId);
    }

    public async Task<bool> HasRolesAsync(int userId)
    {
        return await _context.Users.AsNoTracking()
            .Where(u => u.UserID == userId)
            .SelectMany(u => u.Roles)
            .AnyAsync();
    }

    public async Task<bool> HasProjectAssignmentsAsync(int userId)
    {
        return await _context.Users.AsNoTracking()
            .Where(u => u.UserID == userId)
            .SelectMany(u => u.ProjectAssignments)
            .AnyAsync();
    }

    public async Task DeleteUserByIdAsync(int userId)
    {
        var user = new User { UserID = userId };
        _context.Users.Attach(user);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }



}