using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace iso_management_system.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AtLeastOneFieldRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("Update payload cannot be null.");

        var type = value.GetType();

        // Check the HasValue flags
        var hasValueProperties = type
            .GetProperties()
            .Where(p => p.PropertyType == typeof(bool) && (bool)p.GetValue(value)!)
            .ToList();

        if (!hasValueProperties.Any())
            return new ValidationResult("At least one field must be provided for update.");

        // Optional: also check that the corresponding value is not null
        foreach (var flagProp in hasValueProperties)
        {
            var valuePropName = flagProp.Name.Replace("HasValue", "");
            var valueProp = type.GetProperty(valuePropName);
            if (valueProp != null && valueProp.GetValue(value) != null)
            {
                return ValidationResult.Success; // at least one non-null value
            }
        }

        // If all fields are null even though HasValue is true
        return new ValidationResult("At least one field must have a value to update.");
    }

}