using UrlShortener.ApplicationCore.Commons;
using UrlShortener.ApplicationCore.DTOs.Reponse;
using UrlShortener.ApplicationCore.DTOs.Request;
using UrlShortener.ApplicationCore.Entities;
using UrlShortener.ApplicationCore.Extensions;
using UrlShortener.ApplicationCore.Factories;
using UrlShortener.ApplicationCore.Interfaces.Commons;
using UrlShortener.ApplicationCore.Interfaces.Services;
using UrlShortener.ApplicationCore.Utilities;

namespace UrlShortener.ApplicationCore.Services
{
    public class UrlService(IShortUrlGenerator shortUrlGenerator,
                            IUrlDataStore urlDataStore,
                            TimeProvider timeProvider,
                            RedirectLinkBuilder redirectLinkBuilder,
                            IUserUrlsReader userUrlsReader) : IUrlService
    {
        public async Task<Result<UrlResponse>> CreateShortenUrlAsync(CreateUrlRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CreatedBy))
                return ErrorExtensions.MissingCreatedBy;

            DateTimeOffset timeCreate = timeProvider.GetUtcNow();

            var shortened = new ShortenedUrl(request.LongUrl,
                        shortUrlGenerator.GenerateUniqueUrl(),
                        request.CreatedBy,
                        timeCreate);

            await urlDataStore.AddAsync(shortened, cancellationToken);

            return new UrlResponse(shortened.ShortUrl,
                                request.LongUrl,
                                redirectLinkBuilder.LinkTo(shortened.ShortUrl),
                                timeCreate);
        }

        public async Task<IEnumerable<UrlResponse>> GetListShortenUrlAsync(GetListUrlRequest request, CancellationToken cancellationToken)
        {
            var result = await userUrlsReader.GetAllAsync(request, cancellationToken);
            return result.ToResponse(redirectLinkBuilder);
        }
    }
}
