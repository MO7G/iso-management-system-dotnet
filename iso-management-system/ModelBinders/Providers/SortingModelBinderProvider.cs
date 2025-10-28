using iso_management_system.Dto.General;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace iso_management_system.ModelBinders.Providers;

public class SortingModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(SortingParameters))
            return new BinderTypeModelBinder(typeof(SortingModelBinder));

        return null;
    }
}