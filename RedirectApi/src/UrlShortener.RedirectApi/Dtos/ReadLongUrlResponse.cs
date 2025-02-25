namespace UrlShortener.RedirectApi.Dtos
{
    public record ReadLongUrlResponse(bool Found, string? LongUrl);
}
