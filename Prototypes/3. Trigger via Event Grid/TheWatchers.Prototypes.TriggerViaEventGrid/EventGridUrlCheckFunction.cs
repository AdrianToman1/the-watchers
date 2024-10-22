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
using TheWatchers.Prototypes.TriggerViaEventGrid.CosmosService;
using TheWatchers.Prototypes.TriggerViaEventGrid.EventGrid;

namespace TheWatchers.Prototypes.TriggerViaEventGrid;

/// <summary>
///     Records the HTTP response status code returned by an URL to a Cosmos DB in response to an Event Grid event.
/// </summary>
public class EventGridUrlCheckFunction
{
    private readonly HttpClient _client;
    private readonly ICosmosDbRepositoryService _cosmosDbRepositoryService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventGridUrlCheckFunction" /> class.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClient factory.</param>
    /// <param name="cosmosDbRepositoryService">The Cosmos DB repository service.</param>
    /// <exception cref="ArgumentNullException">
    ///     One of the parameters (<c>httpClientFactory</c>, or <c>cosmosDbRepositoryService</c>) is
    ///     null.
    /// </exception>
    public EventGridUrlCheckFunction(IHttpClientFactory httpClientFactory,
        ICosmosDbRepositoryService cosmosDbRepositoryService)
    {
        _client = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _cosmosDbRepositoryService = cosmosDbRepositoryService ??
                                     throw new ArgumentNullException(nameof(cosmosDbRepositoryService));
    }

    /// <summary>
    ///     Performs a HTTP request to the URL and logs the response status code.
    /// </summary>
    /// <param name="eventGridEvent">The Event Grid event properties.</param>
    /// <param name="log">The logger.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> representing request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [FunctionName("EventGridUrlCheckFunction")]
    public async Task DoUrlCheck([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log, CancellationToken cancellationToken = default)
    {
        var data = eventGridEvent.Data.ToObjectFromJson<DoUrlCheckEventData>();


        var executedAt = DateTimeOffset.Now;

        var requestSentAt = DateTimeOffset.Now;
        var httpResponseMessage = await _client.GetAsync(data.Url, cancellationToken);
        var responseReceivedAt = DateTimeOffset.Now;

        log.LogInformation($"{responseReceivedAt} {data.Url} {httpResponseMessage.StatusCode}");

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