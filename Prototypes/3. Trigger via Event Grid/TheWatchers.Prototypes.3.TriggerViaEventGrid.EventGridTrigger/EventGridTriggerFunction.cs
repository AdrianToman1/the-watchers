using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes._3.TriggerViaEventGrid.EventGridTrigger.EventGrid;

namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.EventGridTrigger;

/// <summary>
///     Initializes a new instance of the <see cref="EventGridTriggerFunction" /> class.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="configuration">The configuration settings.</param>
/// <param name="eventGridPublisherClientService">The event grid publisher client service.</param>
/// <param name="configuration">The configuration settings.</param>
/// <exception cref="ArgumentNullException"><c>configuration</c> is null.</exception>
public sealed class EventGridTriggerFunction(
    ILoggerFactory loggerFactory,
    IEventGridPublisherClientService eventGridPublisherClientService,
    IConfiguration configuration)
{
    private readonly IConfiguration _configuration =
        configuration ?? throw new ArgumentNullException(nameof(configuration));

    private readonly IEventGridPublisherClientService _eventGridPublisherClientService =
        eventGridPublisherClientService ??
        throw new ArgumentNullException(nameof(eventGridPublisherClientService));

    private readonly ILogger _logger = loggerFactory.CreateLogger<EventGridTriggerFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    /// <summary>
    ///     Sends a request to perform an HTTP check for the configured URL to event grid.
    /// </summary>
    /// <param name="myTimer">The timer schedule information.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Function("EventGridTriggerFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        CancellationToken cancellationToken = default)
    {
        var url = _configuration[Constants.UrlKey] ?? Constants.DefaultUrl;

        _eventGridPublisherClientService.PersistUrlCheckResultAsync(url, myTimer.ScheduleStatus?.Next,
            cancellationToken);

        _logger.LogInformation($"Request to check {url} successful sent to Event Grid");
    }
}