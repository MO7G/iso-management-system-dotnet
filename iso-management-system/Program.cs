

// this will create the builder object and sets the app configuration , logging , and dependency injection container as well 
// 🟩 Similar to:
// Node.js (Express) → const app = express();
// Spring Boot → SpringApplication.run(App.class, args)
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// like Spring Boot → same as @OpenAPIDefinition or enabling springdoc-openapi.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// This takes all the registered services and creates the actual WebApplication instance — which will handle requests.
var app = builder.Build();

// Configure the HTTP request pipeline.
// This builds the HTTP request pipeline, middleware by middleware — each one can modify the request or response.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// middleware that redirects HTTP → HTTPS.
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


// get mapping route here 
app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();


// a record like a dto 
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}