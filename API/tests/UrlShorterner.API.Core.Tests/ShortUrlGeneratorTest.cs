using FluentAssertions;
using UrlShortener.ApplicationCore.Utilities;

namespace UrlShorterner.API.Core.Tests
{
    public class ShortUrlGeneratorTest
    {
        [Fact]
        public void ShouldReturnShortUrlFor10001()
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(10001, 20000);
            var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

            var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

            shortUrl.Should().Be("2bJ");
        }

        [Fact]
        public void ShouldReturnShortUrlForZero()
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(0, 10);
            var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

            var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

            shortUrl.Should().Be("0");
        }
    }
}
