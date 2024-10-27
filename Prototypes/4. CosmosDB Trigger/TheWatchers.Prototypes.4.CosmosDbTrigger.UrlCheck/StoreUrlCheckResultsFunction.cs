using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes._4.CosmosDbTrigger.Models;
using TheWatchers.Prototypes._4.CosmosDbTrigger.UrlCheck.CosmosService;

namespace TheWatchers.Prototypes._4.CosmosDbTrigger.UrlCheck;

/// <summary>
///     Performs an HTTP request to the URL and records the response to CosmosDB.
/// </summary>
/// <param name="loggerFactory">The logg</param>
/// <param name="httpClientFactory">The HttpClient factory.</param>
/// <param name="cosmosDbRepositoryService">The Cosmos DB repository service.</param>
/// <exception cref="ArgumentNullException">
///     <c>loggerFactory</c>, <c>httpClientFactory</c>, <c>configuration</c> or
///     <c>cosmosDbRepositoryService</c> is null.
/// </exception>
public sealed class StoreUrlCheckResultsFunction(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory,
    ICosmosDbRepositoryService cosmosDbRepositoryService
)
{
    private readonly HttpClient _client =
        httpClientFactory.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));

    private readonly ICosmosDbRepositoryService _cosmosDbRepositoryService =
        cosmosDbRepositoryService ?? throw new ArgumentNullException(nameof(cosmosDbRepositoryService));

    private readonly ILogger _logger = loggerFactory.CreateLogger<StoreUrlCheckResultsFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    /// <summary>
    ///     Performs an HTTP request to the URL and records the response to CosmosDB, in response to an Event Grid trigger.
    /// </summary>
    /// <param name="cloudEvent">The incoming Event Grid information.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    /// <exception cref="ArgumentNullException"><c>cloudEvent</c> or <c>cloudEvent.Date</c> is null.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Function("StoreUrlCheckResultsFunction")]
    public async Task Run([EventGridTrigger] CloudEvent cloudEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(cloudEvent);
        ArgumentNullException.ThrowIfNull(cloudEvent.Data);

        var data = cloudEvent.Data.ToObjectFromJson<DoUrlCheckEventData>();

        var executedAt = DateTimeOffset.Now;

        var requestSentAt = DateTimeOffset.Now;
        var httpResponseMessage = await _client.GetAsync(data.Url, cancellationToken);
        var responseReceivedAt = DateTimeOffset.Now;

        _logger.LogInformation($"{data.Url} {httpResponseMessage.StatusCode}");

        var urlCheckResult = new UrlCheckResultModel
        {
            Id = Guid.NewGuid().ToString(),
            ExecutedAt = executedAt,
            ScheduledFor = data.ScheduledFor,
            RequestSentAt = requestSentAt,
            ResponseSentAt = httpResponseMessage.Headers.Date,
            ResponseReceivedAt = responseReceivedAt,
            StatusCode = new HttpStatusCodeModel
            {
                StatusCode = (int)httpResponseMessage.StatusCode,
                ReasonPhrase = httpResponseMessage.StatusCode.ToString()
            },
            Headers = httpResponseMessage.Headers.ToList()
                .ConvertAll(i => new Tuple<string, IEnumerable<string>>(i.Key, i.Value)),
            Url = data.Url
        };

        urlCheckResult.Headers.AddRange(httpResponseMessage.Content.Headers.ToList()
            .ConvertAll(i => new Tuple<string, IEnumerable<string>>(i.Key, i.Value)));

        var s = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        urlCheckResult.Body = s.Length > Constants.MaxNumberOfHttpResponseBodyCharactersToPersist
            ? s[..Constants.MaxNumberOfHttpResponseBodyCharactersToPersist]
            : s;

        await _cosmosDbRepositoryService.PersistUrlCheckResultAsync(urlCheckResult, cancellationToken);
    }
}