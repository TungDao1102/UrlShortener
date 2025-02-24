using Azure.Identity;
using UrlShortener.API.Extensions;
using UrlShortener.ApplicationCore.Utilities;
using UrlShortener.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton(
                new RedirectLinkBuilder(
                    new Uri(builder.Configuration["RedirectService:Endpoint"]!)));

builder.Services.AddHealthChecks()
    .AddCosmosHealthCheck(builder.Configuration)
    .AddUrlGroup(
        new Uri(
            new Uri(builder.Configuration["TokenRangeService:Endpoint"]!),
            "healthz"),
        name: "token-range-service");

builder.Services.AddHttpClient("TokenRangeService",
    client =>
    {
        client.BaseAddress =
            new Uri(builder.Configuration["TokenRangeService:Endpoint"]!);
    });

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

app.UseAuthorization();

app.MapControllers();

app.Run();
