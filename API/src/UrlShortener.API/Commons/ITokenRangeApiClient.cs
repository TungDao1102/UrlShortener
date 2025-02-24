using UrlShortener.ApplicationCore.Commons;

namespace UrlShortener.API.Commons
{
    public interface ITokenRangeApiClient
    {
        Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken);
    }
}
