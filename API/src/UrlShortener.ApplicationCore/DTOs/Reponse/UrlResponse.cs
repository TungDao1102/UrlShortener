namespace UrlShortener.ApplicationCore.DTOs.Reponse
{
    public record UrlResponse(string Id, Uri LongUrl, Uri ShortUrl, DateTimeOffset CreatedOn);
}
