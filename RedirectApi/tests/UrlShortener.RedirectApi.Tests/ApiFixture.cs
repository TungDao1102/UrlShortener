using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Testcontainers.Redis;
using UrlShortener.RedirectApi.Tests.InMemory;
using UrlShortener.Libraries.Testing.Extensions;
using UrlShortener.RedirectApi.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace UrlShortener.RedirectApi.Tests
{
    public class ApiFixture : WebApplicationFactory<IRedirectApiAssemblyMarker>, IAsyncLifetime
    {
        private readonly RedisContainer _redisContainer = new RedisBuilder().Build();
        public string RedisConnectionString => _redisContainer.GetConnectionString();

        public InMemoryShortenedUrlReader ShortenedUrlReader { get; } = [];
        public async Task InitializeAsync()
        {
            await _redisContainer.StartAsync();
            Environment.SetEnvironmentVariable("Redis__ConnectionString", RedisConnectionString);
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _redisContainer.StopAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(
                services =>
                {
                    services.Remove<IShortenedUrlReader>();
                    services.AddSingleton<IShortenedUrlReader>(
                        s =>
                            new RedisUrlReader(ShortenedUrlReader,
                                ConnectionMultiplexer.Connect(RedisConnectionString),
                                s.GetRequiredService<ILogger<RedisUrlReader>>())
                    );
                });
            base.ConfigureWebHost(builder);
        }
    }
}
