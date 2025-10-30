using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Dto.General;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iso_management_system.Repositories.Implementations;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _context.Customers
            .Include(c => c.Projects)
            .AsNoTracking()
            .ToList();
    }

    public Customer GetCustomerById(int customerId)
    {
        return _context.Customers
            .Include(c => c.Projects)
            .AsNoTracking() // read-only
            .FirstOrDefault(c => c.CustomerID == customerId);
    }

    
    public Customer GetTrackedCustomerById(int customerId)
    {
        return _context.Customers
            .Include(c => c.Projects)
            .FirstOrDefault(c => c.CustomerID == customerId); // tracked
    }
    
    public bool EmailExists(string? email)
    {
        if (string.IsNullOrEmpty(email)) return false;
        return _context.Customers.Any(c => c.Email.ToLower() == email.ToLower());
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public Customer GetCustomerWithProjects(int customerId)
    {
        return _context.Customers
            .Include(c => c.Projects)
            .FirstOrDefault(c => c.CustomerID == customerId);
    }
    
    
     
    public IEnumerable<Customer> GetAllCustomers(int pageNumber, int pageSize, out int totalRecords)
    {
        var query = _context.Customers
            .Include(c => c.Projects)
            .AsNoTracking();

        totalRecords = query.Count();

        return query
            .OrderBy(c => c.CustomerID)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public IEnumerable<Customer> SearchCustomers(
        string? query,
        int pageNumber,
        int pageSize,
        SortingParameters sorting,
        out int totalRecords)
    {
        var baseQuery = _context.Customers
            .Include(c => c.Projects)
            .AsQueryable();

        // Filter
        if (!string.IsNullOrWhiteSpace(query))
        {
            baseQuery = baseQuery.Where(c =>
                c.Name.Contains(query) ||
                c.Email.Contains(query));
        }

        // Sort dynamically
        switch (sorting.SortBy?.ToLower())
        {
            case "name":
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(c => c.Name)
                    : baseQuery.OrderBy(c => c.Name);
                break;
            case "email":
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(c => c.Email)
                    : baseQuery.OrderBy(c => c.Email);
                break;
            default:
                baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                    ? baseQuery.OrderByDescending(c => c.CustomerID)
                    : baseQuery.OrderBy(c => c.CustomerID);
                break;
        }

        totalRecords = baseQuery.Count();

        return baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToList();
    }
    
    
    
    public void UpdateCustomer(Customer customer)
    {
        // Attach the entity if it's not already tracked
        var tracked = _context.Customers.Local.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
        if (tracked == null)
        {
            _context.Customers.Attach(customer);
        }

        // Mark entity as modified
        _context.Entry(customer).State = EntityState.Modified;

        // Save changes
        _context.SaveChanges();
    }

    
    public bool CustomerExists(int customerId)
    {
        return _context.Customers.Any(c => c.CustomerID == customerId);
    }

    
    public void DeleteCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }
}