using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
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
            .AsNoTracking()
            .FirstOrDefault(c => c.CustomerID == customerId);
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