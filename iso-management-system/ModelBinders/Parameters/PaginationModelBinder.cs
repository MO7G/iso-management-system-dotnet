using iso_management_system.Constants;
using iso_management_system.Dto.General;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace iso_management_system.ModelBinders;


public class PaginationModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var pageNumberValue = bindingContext.ValueProvider.GetValue("pageNumber").FirstValue;
        var pageSizeValue = bindingContext.ValueProvider.GetValue("pageSize").FirstValue;

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

        bindingContext.Result = ModelBindingResult.Success(pagination);
        return Task.CompletedTask;
    }
}