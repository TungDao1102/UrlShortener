namespace UrlShortener.ApplicationCore.DTOs.Request
{
    public record CreateUrlRequest(Uri LongUrl, string CreatedBy);
}
