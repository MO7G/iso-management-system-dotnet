using iso_management_system.Dto.General;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace iso_management_system.ModelBinders;

public class SortingModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var sortBy = context.ValueProvider.GetValue("sortBy").FirstValue ?? "Id";
        var sortDirection = context.ValueProvider.GetValue("sortDirection").FirstValue ?? "asc";

        // Normalize direction
        sortDirection = sortDirection.ToLower() == "desc" ? "desc" : "asc";

        var result = new SortingParameters
        {
            SortBy = sortBy,
            SortDirection = sortDirection
        };

        context.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}