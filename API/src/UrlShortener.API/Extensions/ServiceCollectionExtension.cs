using UrlShortener.ApplicationCore.Interfaces.Commons;
using UrlShortener.ApplicationCore.Interfaces.Services;
using UrlShortener.ApplicationCore.Services;
using UrlShortener.ApplicationCore.Utilities;

namespace UrlShortener.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUrlService, UrlService>();
            services.AddSingleton<TokenProvider>();
            services.AddScoped<IShortUrlGenerator, ShortUrlGenerator>();
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton(
                new RedirectLinkBuilder(
                    new Uri(configuration["RedirectService:Endpoint"]!)));

            return services;
        }
    }
}
