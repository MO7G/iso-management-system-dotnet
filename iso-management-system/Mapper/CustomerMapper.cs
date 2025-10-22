using iso_management_system.DTOs;
using iso_management_system.Models;
using System.Linq;
using iso_management_system.Dto.Customer;

namespace iso_management_system.Mappers;

public static class CustomerMapper
{
    public static CustomerResponseDTO ToResponseDTO(Customer customer)
    {
        return new CustomerResponseDTO
        {
            CustomerID = customer.CustomerID,
            Name = customer.Name,
            Email = customer.Email,
            CreatedAt = customer.CreatedAt,
            ModifiedAt = customer.ModifiedAt,
        };
    }

    
    public static Customer ToEntity(CustomerRequestDTO dto)
    {
        return new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }
}