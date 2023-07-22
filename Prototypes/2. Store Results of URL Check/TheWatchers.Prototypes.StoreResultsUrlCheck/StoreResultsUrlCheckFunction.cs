using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheWatchers.Prototypes.StoreResultsUrlCheck.CosmosService;

namespace TheWatchers.Prototypes.StoreResultsUrlCheck;

/// <summary>
///     Records the HTTP response status code returned by an URL to a Cosmos DB.
/// </summary>
public class StoreResultsUrlCheckFunction
{
    private readonly HttpClient _client;
    private readonly Configuration _configuration;
    private readonly ICosmosDbRepositoryService _cosmosDbRepositoryService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StoreResultsUrlCheckFunction" /> class.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClient factory.</param>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="cosmosDbRepositoryService">The Cosmos DB repository service.</param>
    /// <exception cref="ArgumentNullException">
    ///     One of the parameters (<c>httpClientFactory</c>, <c>configuration</c>, or <c>cosmosDbRepositoryService</c>) is null.
    /// </exception>
    public StoreResultsUrlCheckFunction(IHttpClientFactory httpClientFactory, IOptions<Configuration> configuration, ICosmosDbRepositoryService cosmosDbRepositoryService)
    {
        _client = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        _cosmosDbRepositoryService = cosmosDbRepositoryService ?? throw new ArgumentNullException(nameof(cosmosDbRepositoryService));
    }

    /// <summary>
    ///     Performs a HTTP request to the URL and logs the response status code.
    /// </summary>
    /// <param name="myTimer">The timer schedule information.</param>
    /// <param name="log">The logger.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> representing request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [FunctionName("StoreResultsUrlCheckFunction")]
    public async Task DoSimpleUrlCheck([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log, CancellationToken cancellationToken = default)
    {
        var executedAt = DateTimeOffset.Now;

        var requestSentAt = DateTimeOffset.Now;
        var httpResponseMessage = await _client.GetAsync(_configuration.Url, cancellationToken);
        var responseReceivedAt = DateTimeOffset.Now;

        log.LogInformation($"{responseReceivedAt} {_configuration.Url} {httpResponseMessage.StatusCode}");

        var urlCheckResult = new UrlCheckResultModel
        {
            Id = Guid.NewGuid().ToString(),
            ExecutedAt = executedAt,
            ScheduledFor = myTimer.ScheduleStatus.Next,
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
            Url = _configuration.Url
        };

        urlCheckResult.Headers.AddRange(httpResponseMessage.Content.Headers.ToList()
            .ConvertAll(i => new Tuple<string, IEnumerable<string>>(i.Key, i.Value)));

        var s = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        urlCheckResult.Body = s.Length > Constants.MaxNumberOfHttpResponseBodyCharactersToPersist ? s[..Constants.MaxNumberOfHttpResponseBodyCharactersToPersist] : s;

        await _cosmosDbRepositoryService.PersistUrlCheckResultAsync(urlCheckResult, cancellationToken);
    }
}