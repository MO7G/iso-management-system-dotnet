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




var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging(false); // prevents logging parameter values
});



builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Throws an error if the incoming JSON contains properties
        // that **do not exist** in the target DTO or model.
        // This helps prevent clients from sending extra/unexpected fields.
        options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;

        // Prevents infinite loops when serializing objects that have
        // circular references (e.g., Parent → Child → Parent).
        // Without this, JSON serialization can throw an exception.
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

        // Ignores properties with null values when serializing objects to JSON.
        // This makes the JSON output cleaner by omitting fields that have no value.
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });







builder.Logging.ClearProviders(); // optional: clear default providers
builder.Logging.AddConsole();     // or keep your console logger

// Filter EF Core SQL commands
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);





// Add application service using assembly scanning
builder.Services.AddApplicationServices();

builder.Services.AddControllers(options =>
{
    options.AddCustomModelBinders(); // 👈 one line for all
});

// Add controllers
builder.Services.AddControllers();

// Disable automatic 400 for model validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add global exception middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


// ✅ Test database connection using extension
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.EnsureConnection();  // <-- your extension method
}

app.Run();