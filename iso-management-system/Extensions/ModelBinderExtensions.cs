using iso_management_system.ModelBinders.Providers;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Extensions;

public static class ModelBinderExtensions
{
    public static void AddCustomModelBinders(this MvcOptions options)
    {
        // Register the custom SortingModelBinderProvider at the highest priority (index 0).
        // The index determines the order in which model binder providers are evaluated.
        // Custom binders should come before the built-in ones so they get the first chance
        // to handle specific parameter types like SortingParameters.
        options.ModelBinderProviders.Insert(0, new SortingModelBinderProvider());

            
        // i can add more here later if i want as well 
        // options.ModelBinderProviders.Insert(2, new FilterModelBinderProvider());
    }
}