using System;
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Dto.Customer;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services;



public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<CustomerResponseDTO> GetAllCustomers()
    {
        var customers = _customerRepository.GetAllCustomers();
        return customers.Select(CustomerMapper.ToResponseDTO);
    }

    public CustomerResponseDTO GetCustomerById(int customerId)
    {
        var customer = _customerRepository.GetCustomerById(customerId);
        if (customer == null)
            throw new NotFoundException($"Customer with ID {customerId} not found.");

        return CustomerMapper.ToResponseDTO(customer);
    }
    
    
 

    

    public CustomerResponseDTO CreateCustomer(CustomerRequestDTO dto)
    {
        if (_customerRepository.EmailExists(dto.Email))
            throw new BusinessRuleException("A customer with this email already exists.");

        var customer = CustomerMapper.ToEntity(dto);
        _customerRepository.AddCustomer(customer);
        return CustomerMapper.ToResponseDTO(customer);
    }
    
    
    public CustomerResponseDTO UpdateCustomer(int customerId, CustomerUpdateDTO dto)
    {
        // Fetch entity
        var customer = _customerRepository.GetCustomerById(customerId);
        if (customer == null)
            throw new NotFoundException($"Customer with ID {customerId} not found.");

        // Apply updates only if the field was sent
        if (dto.NameHasValue) customer.Name = dto.Name;
        if (dto.EmailHasValue) customer.Email = dto.Email;

        // Update modified timestamp
        customer.ModifiedAt = DateTime.Now;

        // Persist changes via repository
        _customerRepository.UpdateCustomer(customer);
        var updatedDto = CustomerMapper.ToResponseDTO(customer);
        return updatedDto;
    }

    

    public void DeleteCustomer(int customerId)
    {
        var customer = _customerRepository.GetCustomerWithProjects(customerId);
        if (customer == null)
            throw new NotFoundException($"Customer with ID {customerId} not found.");

        if (customer.Projects.Any())
            throw new BusinessRuleException("Cannot delete a customer who has active projects.");

        _customerRepository.DeleteCustomer(customer);
    }
}