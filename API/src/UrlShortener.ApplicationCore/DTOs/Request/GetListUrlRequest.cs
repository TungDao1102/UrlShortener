using UrlShortener.ApplicationCore.Commons;

namespace UrlShortener.ApplicationCore.DTOs.Request
{
    public class GetListUrlRequest(string? author) : PaginateParameter
    {
        public string? Author { get; set; } = author;
    }
}
