using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.API;
using UrlShortener.ApplicationCore.Interfaces.Commons;
using UrlShorterner.API.Core.Tests.InMemory;
using UrlShortener.Libraries.Testing.Extensions;
using UrlShortener.API.Commons;

namespace UrlShorterner.API.Core.Tests.Commons
{
    public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
    {
        public ApiFixture()
        {
            Environment.SetEnvironmentVariable("RedirectService__Endpoint", "https://urlshortener.tests/r/");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(
                services =>
                {
                    var inMemoryStore = new InMemoryUrlDataStore();
                    services.Remove<IUrlDataStore>();
                    services
                        .AddSingleton<IUrlDataStore>(
                            inMemoryStore);

                    services.Remove<IUserUrlsReader>();
                    services
                        .AddSingleton<IUserUrlsReader>(
                            inMemoryStore);

                    services.Remove<ITokenRangeApiClient>();
                    //   services.AddSingleton<ITokenRangeApiClient, FakeTokenRangeApiClient>();

                    //   services.AddAuthentication(defaultScheme: "TestScheme")
                    //      .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    //           "TestScheme", options => { });

                    services.AddAuthorization(options =>
                    {
                        options.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        options.FallbackPolicy = null;
                    });

                }
            );

            base.ConfigureWebHost(builder);
        }
    }
}
