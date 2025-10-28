using System.Reflection;

namespace iso_management_system.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();

        // 1️⃣ Register all classes ending with "Repository" as their implemented interfaces
        foreach (var type in assembly.GetTypes())
            if (type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository"))
            {
                var interfaces = type.GetInterfaces();
                foreach (var iface in interfaces) services.AddScoped(iface, type);
            }

        // 2️⃣ Register all classes ending with "Service" as self (you can change to interface if you have one)
        foreach (var type in assembly.GetTypes())
            if (type.IsClass && !type.IsAbstract && type.Name.EndsWith("Service"))
                services.AddScoped(type); // registers as itself


        // 3️⃣ Register UnitOfWork
        foreach (var type in assembly.GetTypes())
            if (type.IsClass && !type.IsAbstract && type.Name == "UnitOfWork")
            {
                var iface = type.GetInterface("IUnitOfWork");
                if (iface != null)
                    services.AddScoped(iface, type); // register with interface
                else
                    services.AddScoped(type); // register as self if no interface
            }
    }
}