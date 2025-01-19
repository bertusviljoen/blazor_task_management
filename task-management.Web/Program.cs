using task_management.Web;
using task_management.Web.Components;
using task_management.Web.Services;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using task_management.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

// Configure JSON serialization to use camelCase
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddOutputCache();

//if we are building with aspire 
var apiserviceBaseUrl = "https+http://apiservice";
if (true)
{
    apiserviceBaseUrl = "https+http://localhost:7556";
}

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        // client.BaseAddress = new("https+http://apiservice");
        client.BaseAddress = new(apiserviceBaseUrl);
    });

builder.Services.AddHttpClient<ITaskBoardService, ClientTaskBoardService>(client =>
    {
        client.BaseAddress = new(apiserviceBaseUrl);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.UseStaticFiles();

app.MapRazorComponents<task_management.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
