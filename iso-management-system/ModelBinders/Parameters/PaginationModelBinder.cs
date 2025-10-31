using iso_management_system.Constants;
using iso_management_system.Dto.General;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace iso_management_system.ModelBinders;


public class PaginationModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        
        // in .net we use the valueProvider to read the values coming the request !!
        var pageNumberValue = bindingContext.ValueProvider.GetValue("pageNumber").FirstValue;
        var pageSizeValue = bindingContext.ValueProvider.GetValue("pageSize").FirstValue;

        
        // parsing string to int here 
        int.TryParse(pageNumberValue, out var pageNumber);
        int.TryParse(pageSizeValue, out var pageSize);

        if (pageNumber < 1)
            pageNumber = PaginationDefaults.DefaultPageNumber;

        if (pageSize < 1)
            pageSize = PaginationDefaults.DefaultPageSize;

        if (pageSize > PaginationDefaults.MaxPageSize)
            pageSize = PaginationDefaults.MaxPageSize;

        var pagination = new PaginationParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        
        // mark the binding as successful and assign the resulting object.
        bindingContext.Result = ModelBindingResult.Success(pagination);
        
        // Binding is synchronous so return a completed task
        return Task.CompletedTask;
    }
}