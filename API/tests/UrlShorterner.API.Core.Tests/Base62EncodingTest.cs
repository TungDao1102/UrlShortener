using FluentAssertions;
using UrlShortener.API.Core.Tests.Extensions;

namespace UrlShorterner.API.Core.Tests
{
    public class Base62EncodingTest
    {
        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(20, "K")]
        [InlineData(1000, "G8")]
        [InlineData(61, "z")]
        [InlineData(987654321, "14q60P")]
        public void ShouldEncodeNumberToBase62(long number, string expected)
        {
            number.EncodeToBase62().Should().Be(expected);
        }
    }
}
