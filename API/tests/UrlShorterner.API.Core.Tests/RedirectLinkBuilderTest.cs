using FluentAssertions;
using UrlShortener.ApplicationCore.Utilities;

namespace UrlShorterner.API.Core.Tests
{
    public class RedirectLinkBuilderTest
    {
        [Fact]
        public void ShouldReturnCompleteLinkWhenShortUrlIsProvided()
        {
            var redirectServiceEndpoint = new Uri("https://redirect-service.com/r/");
            const string shortUrl = "abc123";
            var expectedUri = new Uri("https://redirect-service.com/r/abc123");
            var redirectLinkBuilder = new RedirectLinkBuilder(redirectServiceEndpoint);

            var result = redirectLinkBuilder.LinkTo(shortUrl);

            result.Should().Be(expectedUri);
        }
    }
}
