using Newtonsoft.Json;

namespace UrlShortener.CosmosDbTriggerFunction.Models
{
    public class ShortenedUrlEntity
    {
        public string LongUrl { get; }

        [JsonProperty(PropertyName = "id")] // Cosmos DB Unique Identifier
        public string ShortUrl { get; }

        public DateTimeOffset CreatedOn { get; }

        [JsonProperty(PropertyName = "PartitionKey")] // Cosmos DB Partition Key
        public string CreatedBy { get; }

        public ShortenedUrlEntity(string longUrl, string shortUrl,
            DateTimeOffset createdOn, string createdBy)
        {
            LongUrl = longUrl;
            ShortUrl = shortUrl;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }
    }
}
