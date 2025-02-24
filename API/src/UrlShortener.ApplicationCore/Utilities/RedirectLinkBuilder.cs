namespace UrlShortener.ApplicationCore.Utilities
{
    public class RedirectLinkBuilder(Uri redirectServiceEndpoint)
    {
        public Uri LinkTo(string shortUrl)
        {
            return new Uri(redirectServiceEndpoint, shortUrl);
        }
    }
}
