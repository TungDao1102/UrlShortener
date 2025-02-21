using UrlShortener.ApplicationCore.DTOs.Request;
using UrlShortener.ApplicationCore.Entities;

namespace UrlShortener.ApplicationCore.Interfaces.Commons
{
    public interface IUserUrlsReader
    {
        Task<IEnumerable<ShortenedUrl>> GetAllAsync(GetListUrlRequest request, CancellationToken cancellationToken);
    }
}
