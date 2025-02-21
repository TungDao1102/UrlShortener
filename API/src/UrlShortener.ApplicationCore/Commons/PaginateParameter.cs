namespace UrlShortener.ApplicationCore.Commons
{
    public class PaginateParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? ContinuationToken { get; set; } = default;
    }
}
