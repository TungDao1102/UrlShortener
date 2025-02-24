using UrlShortener.ApplicationCore.Commons;
using UrlShortener.ApplicationCore.DTOs.Reponse;
using UrlShortener.ApplicationCore.DTOs.Request;

namespace UrlShortener.ApplicationCore.Interfaces.Services
{
    public interface IUrlService
    {
        Task<Result<UrlResponse>> CreateShortenUrlAsync(CreateUrlRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<UrlResponse>> GetListShortenUrlAsync(GetListUrlRequest request, CancellationToken cancellationToken);
    }
}
