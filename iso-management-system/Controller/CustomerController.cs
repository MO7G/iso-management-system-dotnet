using iso_management_system.Attributes;
using iso_management_system.Dto.Customer;
using iso_management_system.Dto.General;
using iso_management_system.Helpers;
using iso_management_system.ModelBinders;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// or System.Text.Json

namespace iso_management_system.Controllers;

[ApiController]
[ValidateModel]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }
    
    
    /// <summary>
    /// Retrieves a paginated list of all customers.
    /// </summary>
    [HttpGet("customers")]
    public ActionResult<ApiResponseWrapper<PagedResponse<CustomerResponseDTO>>> GetCustomers(
        [ModelBinder(BinderType = typeof(PaginationModelBinder))] PaginationParameters pagination)
    {
        var customers = _customerService.GetAllCustomers(pagination.PageNumber, pagination.PageSize);
        return Ok(ApiResponse.Ok(customers, "Customers fetched successfully"));
    }

    /// <summary>
    /// Searches customers by query (name or email), with pagination and sorting.
    /// </summary>
    [HttpGet("search")]
    public ActionResult<ApiResponseWrapper<PagedResponse<CustomerResponseDTO>>> SearchCustomers(
        string? query,
        [ModelBinder(BinderType = typeof(PaginationModelBinder))] PaginationParameters pagination,
        [FromQuery] SortingParameters sorting)
    {
        var result = _customerService.SearchCustomers(query, pagination.PageNumber, pagination.PageSize, sorting);
        return Ok(ApiResponse.Ok(result, "Customers fetched successfully"));
    }
    
    
    // Get customer by ID
    [HttpGet("{customerId}")]
    public ActionResult<ApiResponseWrapper<CustomerResponseDTO>> GetCustomerById(int customerId)
    {
        var customer = _customerService.GetCustomerById(customerId);
        return Ok(ApiResponse.Ok(customer, "Customer fetched successfully"));
    }

    [HttpPatch("update/{customerId}")]
    public ActionResult<ApiResponseWrapper<CustomerResponseDTO>> UpdateCustomer(
        int customerId,
        [FromBody] CustomerUpdateDTO dto)
    {
        // Delegate everything to the service
        var updatedDto = _customerService.UpdateCustomer(customerId, dto);

        return Ok(ApiResponse.Ok(updatedDto, "Customer updated successfully"));
    }


    // Create a new customer
    [HttpPost("create")]
    public ActionResult<ApiResponseWrapper<CustomerResponseDTO>> CreateCustomer([FromBody] CustomerRequestDTO dto)
    {
        Console.WriteLine("this is the dto: " + JsonConvert.SerializeObject(dto));
        var created = _customerService.CreateCustomer(dto);
        return CreatedAtAction(
            nameof(GetCustomerById),
            new { customerId = created.CustomerID },
            ApiResponse.Created(created, "Customer created successfully")
        );
    }
    
    
    // Delete customer
    [HttpDelete("delete/{customerId}")]
    public ActionResult<ApiResponseWrapper<object>> DeleteCustomer(int customerId)
    {
        _customerService.DeleteCustomer(customerId);
        return Ok(ApiResponse.Ok<object>(null, "Customer deleted successfully"));
    }
}