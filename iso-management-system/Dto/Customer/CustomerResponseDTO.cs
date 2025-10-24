using System;

namespace iso_management_system.Dto.Customer;

public class CustomerResponseDTO
{
    public int CustomerID { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}