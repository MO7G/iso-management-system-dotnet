using System.Collections.Generic;
using iso_management_system.Dto.General;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAllCustomers();
    Customer GetCustomerById(int customerId);
    bool EmailExists(string? email);
    
    IEnumerable<Customer> GetAllCustomers(int pageNumber, int pageSize, out int totalRecords);
    IEnumerable<Customer> SearchCustomers(string? query, int pageNumber, int pageSize, SortingParameters sorting, out int totalRecords);

    void AddCustomer(Customer customer);
    Customer GetCustomerWithProjects(int customerId);
    void DeleteCustomer(Customer customer);
    void UpdateCustomer(Customer customer);

    bool CustomerExists(int customerId);

    
}