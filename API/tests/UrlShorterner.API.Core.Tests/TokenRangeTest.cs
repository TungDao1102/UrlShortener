using FluentAssertions;
using UrlShortener.ApplicationCore.Commons;

namespace UrlShorterner.API.Core.Tests
{
    public class TokenRangeTest
    {
        [Fact]
        public void StartTokenGreaterEndToken()
        {
            var act = () => new TokenRange(10, 5);

            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("End must be greater than or equal to start");
        }
    }
}
