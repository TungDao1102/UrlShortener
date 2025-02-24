using UrlShortener.API.Commons;
using UrlShortener.ApplicationCore.Commons;

namespace UrlShorterner.API.Core.Tests.InMemory
{
    public class FakeTokenRangeApiClient : ITokenRangeApiClient
    {
        public Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken)
        {
            return Task.FromResult<TokenRange?>(new TokenRange(1, 10));
        }
    }
}
