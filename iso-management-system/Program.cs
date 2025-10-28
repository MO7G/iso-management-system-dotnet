using iso_management_system.Configurations.Db;
using iso_management_system.Extensions;
using iso_management_system.Middleware;
using iso_management_system.Repositories.Implementations;
using iso_management_system.Repositories.Interfaces;
using iso_management_system.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;



// This here will build the web application this will allow the following
// Initializes the app and sets up dependency injection (DI), logging, configuration, and Kestrel
var builder = WebApplication.CreateBuilder(args);


// adds ef core DBContext to the DI container
// Configures SQL Server connection.
// enableSensitiveDataLogging(false) prevents the logs of annoying sensitive parameters in sql logs on the terminal can be annoying with debugging !!
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging(false); // prevents logging parameter values
});











// Automatically registers application services and repositories:
// // - All classes ending with "Repository" are registered as their interfaces.
// // - All classes ending with "Service" are registered as themselves (or interface if you modify).
// // - The "UnitOfWork" class is registered with IUnitOfWork interface if available.
// // This keeps Program.cs clean and avoids manual DI registrations for every class.
builder.Services.AddApplicationServices();


builder.Services.AddControllers(options =>
    {
        // Adds all custom model binders globally, e.g., SortingModelBinderProvider.
        // This allows controller actions to automatically bind complex query parameters
        // such as sorting, filtering, or pagination without writing manual parsing logic.
        options.AddCustomModelBinders(); 
    })
    .AddNewtonsoftJson(options =>
    {
        // Configure JSON serialization/deserialization using Newtonsoft.Json instead of System.Text.Json.
        // Newtonsoft.Json is more mature and feature-rich, especially for advanced scenarios like:
        // - custom converters
        // - circular reference handling
        // - conditional serialization
        // - strict property validation

        // Throws an error if incoming JSON contains properties that do not exist in the target DTO/model.
        // This prevents clients from sending extra/unexpected fields and helps maintain API integrity.
        options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;

        // Prevents infinite loops when serializing objects with circular references
        // (e.g., Parent → Child → Parent). Without this, JSON serialization can throw exceptions.
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

        // Omits null properties from the serialized JSON output.
        // This keeps responses cleaner and reduces payload size.
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });



// I disable automatic 400 responses for model validation errors.
// Since I am using a centralized exception middleware and custom error handling,
// I want full control over validation responses instead of letting ASP.NET Core
// automatically return a 400 Bad Request. This allows me to return consistent
// error formats, add custom error codes, or include additional context in the response.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});



// I add Swagger/OpenAPI support to my application.
// - AddEndpointsApiExplorer() enables discovery of API endpoints for documentation.
// - AddSwaggerGen() generates the Swagger/OpenAPI specification and UI.
// This allows me to easily test and explore my API endpoints in development.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// I configure middleware for development environment.
// - UseSwagger() enables the Swagger/OpenAPI JSON endpoint.
// - UseSwaggerUI() provides a visual interface to explore and test my API endpoints.
// I only enable these in development to avoid exposing API docs in production.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Add global exception handling middleware
// I am using a centralized exception handler so that all unhandled exceptions 
// in the application are caught and transformed into consistent error responses.
// This replaces the default automatic 400 responses and allows custom error formatting.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Redirect HTTP requests to HTTPS
// Ensures all communication is encrypted using TLS/SSL.
app.UseHttpsRedirection();

// Enable authorization middleware
// This checks for authentication/authorization policies before reaching the controller endpoints.
// For now this is a placeholder it will not block any controller request or something until later using a policy or [Authorize] attribute !!
app.UseAuthorization();

// Map controller routes
// This tells ASP.NET Core to route incoming HTTP requests to the appropriate controller actions.
app.MapControllers();


// ✅ Test database connection at startup using a scoped service
// I create a scope to get an instance of AppDbContext and call EnsureConnection()
// This verifies that the database is reachable and correctly configured before the app starts.
// If there is an issue, the app will fail early, helping with debugging startup issues.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.EnsureConnection();  // <-- your custom extension method
}

// Run the application
// Starts the Kestrel web server and begins listening for incoming HTTP requests.
app.Run();
