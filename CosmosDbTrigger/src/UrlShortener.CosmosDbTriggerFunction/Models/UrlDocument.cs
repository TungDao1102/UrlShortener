namespace UrlShortener.CosmosDbTriggerFunction.Models
{
    public class UrlDocument
    {
        public string Id { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string LongUrl { get; set; } = string.Empty;
    }
}
