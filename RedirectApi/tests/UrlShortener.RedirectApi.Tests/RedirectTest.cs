using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.RedirectApi.Dtos;
using UrlShortener.RedirectApi.Tests.InMemory;

namespace UrlShortener.RedirectApi.Tests
{
    [Collection("Api collection")]
    public class RedirectTest
    {
        private readonly HttpClient _client;
        private readonly InMemoryShortenedUrlReader _storage;

        public RedirectTest(ApiFixture fixture)
        {
            _client = fixture.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });
            _storage = fixture.ShortenedUrlReader;
        }

        [Fact]
        public async Task ShouldReturn301RedirectWithUrlWhenShortUrlExits()
        {
            const string shortUrl = "abc123";
            _storage.Add(shortUrl, new ReadLongUrlResponse(true, "https://qtmviet.com"));

            var response = await _client.GetAsync($"/r/{shortUrl}");

            // HaveStatusCode not available in higher package version
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.MovedPermanently);
            response.Headers.Location.Should().Be("https://qtmviet.com");
        }

        [Fact]
        public async Task ShouldReturn404WhenShortUrlDoesNotExist()
        {
            const string shortUrl = "non-existing";

            var response = await _client.GetAsync($"/r/{shortUrl}");

            response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
        }
    }
}
