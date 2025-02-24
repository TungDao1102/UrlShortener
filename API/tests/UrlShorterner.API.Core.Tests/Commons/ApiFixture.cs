using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.API;
using UrlShortener.API.Commons;
using UrlShortener.ApplicationCore.Interfaces.Commons;
using UrlShortener.Libraries.Testing.Extensions;
using UrlShorterner.API.Core.Tests.Auth;
using UrlShorterner.API.Core.Tests.InMemory;

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
                    services.AddSingleton<ITokenRangeApiClient, FakeTokenRangeApiClient>();

                    services.AddAuthentication(defaultScheme: "TestScheme")
                       .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                            "TestScheme", options => { });

                    services.AddAuthorizationBuilder()
                        .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build())
                        .SetFallbackPolicy(null);

                }
            );

            base.ConfigureWebHost(builder);
        }
    }
}
