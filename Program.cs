using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Prevent cycle errors during JSON serialization of related entities
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configure PostgreSQL Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure CORS for frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure OpenAPI (Native .NET 10 document generator)
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();

// 1. Classic Swagger UI (SmartBear Swagger)
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MDMS API v1");
    options.RoutePrefix = "swagger"; // Serves Swagger UI directly at /swagger
});

// 2. Also keep Scalar UI available at /scalar/v1 as an additional modern option
app.MapScalarApiReference();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Initialize and Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // If database currently has the old small starter seed (< 40 users), upgrade to the full Phase 1 dataset
        if (context.Users.Count() < 40)
        {
            Console.WriteLine("Populating database with full Phase 1 dataset (48 customers, 576+ bills)...");
            DbInitializer.ResetAndSeed(context);
        }
        else
        {
            DbInitializer.Initialize(context);
        }
        Console.WriteLine("Database initialization and seed check completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing or seeding the database.");
    }
}

app.Run();
