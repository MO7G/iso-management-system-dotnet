using System.ComponentModel.DataAnnotations;
using iso_management_system.Attributes;

namespace iso_management_system.Dto.Customer;

[AtLeastOneFieldRequired]
public class CustomerUpdateDTO
{
    
    
    public string? Name { get; set; }
    public bool NameHasValue { get; set; }

    public string? Email { get; set; }
    public bool EmailHasValue { get; set; }
}
