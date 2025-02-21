using System.Text;
using Microsoft.Azure.Cosmos;
using UrlShortener.ApplicationCore.DTOs.Request;
using UrlShortener.ApplicationCore.Entities;
using UrlShortener.ApplicationCore.Interfaces.Commons;

namespace UrlShortener.Infrastructure;

public class CosmosUserUrlsReader(Container container) : IUserUrlsReader
{
    public async Task<IEnumerable<ShortenedUrl>> GetAllAsync(GetListUrlRequest request, CancellationToken cancellationToken)
    {
        var query =
            new QueryDefinition("SELECT * FROM c  WHERE c.PartitionKey = @partitionKey")
            .WithParameter("@partitionKey", request.Author);

        var queryContinuationToken = request.ContinuationToken is null
            ? null
            : Encoding.UTF8.GetString(Convert.FromBase64String(request.ContinuationToken));

        var iterator = container.GetItemQueryIterator<ShortenedUrl>(query,
            continuationToken: queryContinuationToken,
            requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(request.Author),
                MaxItemCount = request.PageSize
            });

        var results = new List<ShortenedUrl>();
        string? resultContinuationToken = null;
        var readItemsCount = 0;

        while (readItemsCount < request.PageSize && iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            results.AddRange(response);
            readItemsCount += response.Count;
            resultContinuationToken = response.ContinuationToken;
        }

        var responseContinuationToken =
            resultContinuationToken is null
                ? null
                : Convert.ToBase64String(Encoding.UTF8.GetBytes(resultContinuationToken));

        return results;
    }
}








