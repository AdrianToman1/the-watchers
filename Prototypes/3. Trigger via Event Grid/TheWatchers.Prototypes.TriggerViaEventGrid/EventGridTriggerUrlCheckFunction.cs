using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheWatchers.Prototypes.TriggerViaEventGrid.EventGrid;

namespace TheWatchers.Prototypes.TriggerViaEventGrid;

/// <summary>
///     Publishes an event to an Event Grid topic to check the provided URL in response to a timer trigger.
/// </summary>
public class EventGridTriggerUrlCheckFunction
{
    private readonly IEventGridPublisherClientService _eventGridPublisherClientService;
    private readonly Configuration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventGridTriggerUrlCheckFunction" /> class.
    /// </summary>
    /// <param name="eventGridPublisherClientService">The Event Grid Publisher Client service.</param>
    /// <param name="configuration">The configuration settings.</param>
    /// <exception cref="ArgumentNullException">
    ///     One of the parameters (<c>eventGridPublisherClientService</c>, or <c>configuration</c> is null.
    /// </exception>
    public EventGridTriggerUrlCheckFunction(IEventGridPublisherClientService eventGridPublisherClientService, IOptions<Configuration> configuration)
    {
        _eventGridPublisherClientService = eventGridPublisherClientService ??
                                           throw new ArgumentNullException(nameof(eventGridPublisherClientService));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
    }

    [FunctionName("EventGridTriggerUrlCheckFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log,
        CancellationToken cancellationToken = default)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        _eventGridPublisherClientService.PersistUrlCheckResultAsync(_configuration.Url, myTimer.ScheduleStatus.Next, cancellationToken);
    }
}