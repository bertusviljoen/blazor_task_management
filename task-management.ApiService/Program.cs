
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using task_management.ApiService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureCosmosClient("cosmos-db");

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Configure JSON serialization to use camelCase
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Configure the CosmosDataContextOptions
builder.Services.Configure<CosmosDataContextOptions>(builder.Configuration.GetSection("CosmosDb"));

//Specify default options for the CosmosDataContextOptions
builder.Services.PostConfigure<CosmosDataContextOptions>(options =>
{
    if (string.IsNullOrEmpty(options.DatabaseName))
        options.DatabaseName = builder.Configuration["CosmosDb:DatabaseName"] ?? "task-management-db";
});

builder.Services.AddScoped<IDataContext, CosmosDataContext>();

builder.Services.AddHostedService<DatabaseInitializer>();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    var applicationUrl = app.Configuration["ApplicationUrl"];

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Web.Api");
        options.WithTheme(ScalarTheme.BluePlanet);
        options.WithSidebar(true);
        options.AddServer(new ScalarServer(applicationUrl!, "Web.Api"));
    });
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

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
.WithName("GetWeatherForecast");

app.MapDefaultEndpoints();

app.MapTaskBoardEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
