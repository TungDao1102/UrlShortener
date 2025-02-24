using Polly.Retry;
using Polly.Extensions.Http;
using UrlShortener.ApplicationCore.Commons;
using Polly;

namespace UrlShortener.API.Commons
{
    public class TokenRangeApiClient : ITokenRangeApiClient
    {
        private readonly HttpClient _httpClient;
        public TokenRangeApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TokenRangeService");
        }
        public async Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken)
        {
            var response = await RetryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync("assign",
                    new { Key = machineKey }, cancellationToken));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to assign new token range");
            }

            var range = await response.Content
                .ReadFromJsonAsync<TokenRange>(cancellationToken: cancellationToken);

            return range;
        }

        private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =
            HttpPolicyExtensions.HandleTransientHttpError()
                                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
