using iso_management_system.ModelBinders.Providers;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Extensions;

public static class ModelBinderExtensions
{
    public static void AddCustomModelBinders(this MvcOptions options)
    {
        // Add all custom model binders here
        options.ModelBinderProviders.Insert(0, new SortingModelBinderProvider());
            
        // 👇 Example: If you add more later
        // options.ModelBinderProviders.Insert(2, new FilterModelBinderProvider());
    }
}