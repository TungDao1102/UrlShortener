using UrlShortener.ApplicationCore.Entities;

namespace UrlShortener.ApplicationCore.Interfaces.Commons
{
    public interface IUrlDataStore
    {
        Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken);
    }
}
