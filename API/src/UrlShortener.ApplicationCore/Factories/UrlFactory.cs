using UrlShortener.ApplicationCore.DTOs.Reponse;
using UrlShortener.ApplicationCore.Entities;
using UrlShortener.ApplicationCore.Utilities;

namespace UrlShortener.ApplicationCore.Factories
{
    public static class UrlFactory
    {
        public static UrlResponse ToResponse(this ShortenedUrl shortenedUrl, RedirectLinkBuilder redirectLinkBuilder)
        {
            return new UrlResponse(shortenedUrl.ShortUrl,
                                redirectLinkBuilder.LinkTo(shortenedUrl.ShortUrl),
                                new Uri(shortenedUrl.LongUrl),
                                shortenedUrl.CreatedOn);
        }

        public static IEnumerable<UrlResponse> ToResponse(this IEnumerable<ShortenedUrl> shortenedUrls, RedirectLinkBuilder redirectLinkBuilder)
        {
            return shortenedUrls.Select(x => x.ToResponse(redirectLinkBuilder)).ToList();
        }
    }
}
