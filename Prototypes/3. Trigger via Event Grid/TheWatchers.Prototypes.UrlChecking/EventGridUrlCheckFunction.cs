// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheWatchers.Prototypes.TriggerViaEventGrid.CosmosService;

namespace TheWatchers.Prototypes.TriggerViaEventGrid;

public class EventGridUrlCheckFunction
{
    private readonly HttpClient _client;
    private readonly Configuration _configuration;
    private readonly ICosmosDbRepositoryService _cosmosDbRepositoryService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventGridUrlCheckFunction" /> class.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClient factory.</param>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="cosmosDbRepositoryService">The Cosmos DB repository service.</param>
    /// <exception cref="ArgumentNullException">
    ///     One of the parameters (<c>httpClientFactory</c>, <c>configuration</c>, or <c>cosmosDbRepositoryService</c>) is
    ///     null.
    /// </exception>
    public EventGridUrlCheckFunction(IHttpClientFactory httpClientFactory, IOptions<Configuration> configuration,
        ICosmosDbRepositoryService cosmosDbRepositoryService)
    {
        _client = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        _cosmosDbRepositoryService = cosmosDbRepositoryService ??
                                     throw new ArgumentNullException(nameof(cosmosDbRepositoryService));
    }

    [FunctionName("Function")]
    public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log,
        CancellationToken cancellationToken = default)
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
            ScheduledFor = executedAt.DateTime, // TODO: Resolve
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
        urlCheckResult.Body = s.Length > Constants.MaxNumberOfHttpResponseBodyCharactersToPersist
            ? s[..Constants.MaxNumberOfHttpResponseBodyCharactersToPersist]
            : s;

        await _cosmosDbRepositoryService.PersistUrlCheckResultAsync(urlCheckResult, cancellationToken);
    }
}