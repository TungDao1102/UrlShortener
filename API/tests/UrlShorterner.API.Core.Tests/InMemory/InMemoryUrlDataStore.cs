using UrlShortener.ApplicationCore.DTOs.Request;
using UrlShortener.ApplicationCore.Entities;
using UrlShortener.ApplicationCore.Interfaces.Commons;

namespace UrlShorterner.API.Core.Tests.InMemory
{
    public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore, IUserUrlsReader
    {
        public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
        {
            Add(shortened.ShortUrl, shortened);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ShortenedUrl>> GetAllAsync(GetListUrlRequest request, CancellationToken cancellationToken)
        {
            var data = Values.Where(u => u.CreatedBy == request.Author).Take(request.PageSize).ToList();

            return Task.FromResult<IEnumerable<ShortenedUrl>>(data);
        }
    }
}
