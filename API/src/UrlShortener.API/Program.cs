using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using UrlShortener.API.Commons;
using UrlShortener.API.Extensions;
using UrlShortener.API.Jobs;
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

builder.Services.AddSingleton<ITokenRangeApiClient, TokenRangeApiClient>();
builder.Services.AddHostedService<TokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
        options => { builder.Configuration.Bind("AzureAd", options); });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthZPolicy", policyBuilder =>
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement()
        {
            RequiredScopesConfigurationKey = "AzureAd:Scopes"
        }));

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy =
        new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    // By default, all incoming requests will be authorized according to 
    // the default policy    
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        if (builder.Configuration["WebAppEndpoints"] is null)
            return;

        var origins = builder.Configuration["WebAppEndpoints"]!.Split(",");

        policy
            .WithOrigins([.. origins])
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowWebApp");

app.MapHealthChecks("/healthz")
    .AllowAnonymous();

app.MapGet("/", () => "API").AllowAnonymous();

app.Run();
