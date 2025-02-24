using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using UrlShortener.TokenRangeService.Commons;
using UrlShortener.TokenRangeService.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration["Postgres:ConnectionString"]!);

builder.Services.AddSingleton(
    new TokenRangeManager(builder.Configuration["Postgres:ConnectionString"]!));

var telemetryConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (telemetryConnectionString is not null)
    builder.Services
        .AddOpenTelemetry()
        .UseAzureMonitor();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/healthz");

app.MapGet("/", () => "TokenRanges Service");
app.MapPost("/assign",
    async (AssignTokenRangeRequest request, TokenRangeManager manager) =>
    {
        var range = await manager.AssignRangeAsync(request.Key);

        return range;
    });

app.Run();
