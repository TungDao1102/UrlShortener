using UrlShortener.ApplicationCore.Commons;

namespace UrlShortener.ApplicationCore.Extensions
{
    public static class ErrorExtensions
    {
        public static Error MissingCreatedBy => new("missing_value", "Created by is required");
    }
}
