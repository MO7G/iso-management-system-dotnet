using System.Linq;
using iso_management_system.Exceptions;

namespace iso_management_system.Attributes;

using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray()  // wrap each error in a string[]
                );

            throw new CustomValidationException("Validation failed", errors); 
        }

    }
}