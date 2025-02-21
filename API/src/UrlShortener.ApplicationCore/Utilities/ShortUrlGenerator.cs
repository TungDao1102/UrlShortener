using UrlShortener.API.Core.Tests.Extensions;
using UrlShortener.ApplicationCore.Interfaces.Commons;

namespace UrlShortener.ApplicationCore.Utilities
{
    public class ShortUrlGenerator(TokenProvider tokenProvider) : IShortUrlGenerator
    {
        public string GenerateUniqueUrl()
        {
            return tokenProvider.GetToken().EncodeToBase62();
        }
    }
}
