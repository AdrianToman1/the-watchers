using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes._6.OfflineEvent.TriggerUrlCheck.EventGrid;

namespace TheWatchers.Prototypes._6.OfflineEvent.TriggerUrlCheck;

/// <summary>
///     Initializes a new instance of the <see cref="TriggerUrlCheckFunction" /> class.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="configuration">The configuration settings.</param>
/// <param name="eventGridPublisherClientService">The event grid publisher client service.</param>
/// <exception cref="ArgumentNullException"><c>configuration</c>, <c>eventGridPublisherClientService</c> or <c>loggerFactory</c> is null.</exception>
public sealed class TriggerUrlCheckFunction(
    ILoggerFactory loggerFactory,
    IEventGridPublisherClientService eventGridPublisherClientService,
    IConfiguration configuration)
{
    private readonly IConfiguration _configuration =
        configuration ?? throw new ArgumentNullException(nameof(configuration));

    private readonly IEventGridPublisherClientService _eventGridPublisherClientService =
        eventGridPublisherClientService ??
        throw new ArgumentNullException(nameof(eventGridPublisherClientService));

    private readonly ILogger _logger = loggerFactory.CreateLogger<TriggerUrlCheckFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    /// <summary>
    ///     Sends a request to perform an HTTP check for the configured URL to event grid.
    /// </summary>
    /// <param name="myTimer">The timer schedule information.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Function("TriggerUrlCheckFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        CancellationToken cancellationToken = default)
    {
        var url = _configuration[Constants.UrlKey] ?? Constants.DefaultUrl;

        _eventGridPublisherClientService.PersistUrlCheckResultAsync(url, myTimer.ScheduleStatus?.Next,
            cancellationToken);

        _logger.LogInformation($"Request to check {url} successful sent to Event Grid");
    }
}