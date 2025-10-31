using iso_management_system.Dto.General;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace iso_management_system.ModelBinders.Providers;


// the IModelBinderProvider which is an ASP.NET Core interface used to register custom binders.
public class SortingModelBinderProvider : IModelBinderProvider
{
    
    
    //The MVC framework calls this method for every parameter or model that needs to be bound
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        
        //Checks whether the model type being bound is SortingParameters.
        // If it is then the provider says Use my custom SortingModelBinder to bind this type.
        if (context.Metadata.ModelType == typeof(SortingParameters))
            return new BinderTypeModelBinder(typeof(SortingModelBinder));

        return null;
    }
}