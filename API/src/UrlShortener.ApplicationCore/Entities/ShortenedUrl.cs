using Newtonsoft.Json;

namespace UrlShortener.ApplicationCore.Entities
{
    public class ShortenedUrl(Uri longUrl, string shortUrl, string createdBy, DateTimeOffset createdOn)
    {
        public string LongUrl { get; } = longUrl.ToString();

        [JsonProperty(PropertyName = "id")] // Cosmos DB Unique Identifier
        public string ShortUrl { get; } = shortUrl;
        public string CreatedBy { get; set; } = createdBy;
        public DateTimeOffset CreatedOn { get; } = createdOn;
    }
}
