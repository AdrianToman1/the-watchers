using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes._2.StoreResultsUrlCheck.CosmosService;
using TheWatchers.Prototypes._2.StoreResultsUrlCheck.Models;

namespace TheWatchers.Prototypes._2.StoreResultsUrlCheck;

/// <summary>
///     Performs an HTTP request to the URL and records the response to CosmosDB.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="httpClientFactory">The HttpClient factory.</param>
/// <param name="configuration">The configuration settings.</param>
/// <param name="cosmosDbRepositoryService">The Cosmos DB repository service.</param>
/// <exception cref="ArgumentNullException">
///     <c>loggerFactory</c>, <c>httpClientFactory</c>, <c>configuration</c> or
///     <c>cosmosDbRepositoryService</c> is null.
/// </exception>
public sealed class StoreResultsUrlCheckFunction(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ICosmosDbRepositoryService cosmosDbRepositoryService
)
{
    private readonly HttpClient _client =
        httpClientFactory.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
    
    private readonly IConfiguration _configuration =
        configuration ?? throw new ArgumentNullException(nameof(configuration));

    private readonly ICosmosDbRepositoryService _cosmosDbRepositoryService =
        cosmosDbRepositoryService ?? throw new ArgumentNullException(nameof(cosmosDbRepositoryService));

    private readonly ILogger _logger = loggerFactory.CreateLogger<StoreResultsUrlCheckFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    /// <summary>
    ///     Performs an HTTP request to the URL and records the response to CosmosDB, in response to a Timer trigger.
    /// </summary>
    /// <param name="myTimer">The timer schedule information.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Function("StoreResultsUrlCheckFunction")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        CancellationToken cancellationToken = default)
    {
        var executedAt = DateTimeOffset.Now;

        var url = _configuration[Constants.UrlKey] ?? Constants.DefaultUrl;

        var requestSentAt = DateTimeOffset.Now;
        var httpResponseMessage = await _client.GetAsync(url, cancellationToken);
        var responseReceivedAt = DateTimeOffset.Now;

        _logger.LogInformation($"{url} {httpResponseMessage.StatusCode}");

        var urlCheckResult = new UrlCheckResultModel
        {
            Id = Guid.NewGuid().ToString(),
            ExecutedAt = executedAt,
            ScheduledFor = myTimer.ScheduleStatus?.Next,
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
            Url = url
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