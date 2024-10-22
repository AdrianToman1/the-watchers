using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes.TriggerViaEventGrid.EventGrid;

namespace TheWatchers.Prototypes.TriggerViaEventGrid;

public class EventGridTriggerFunction
{
    private readonly IEventGridPublisherClientService _eventGridPublisherClientService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventGridTriggerFunction" /> class.
    /// </summary>
    /// <param name="eventGridPublisherClientService">The Event Grid Publisher Client service.</param>
    /// <exception cref="ArgumentNullException"><c>configuration</c> is null.</exception>
    public EventGridTriggerFunction(IEventGridPublisherClientService eventGridPublisherClientService)
    {
        _eventGridPublisherClientService = eventGridPublisherClientService ??
                                           throw new ArgumentNullException(nameof(eventGridPublisherClientService));
    }

    [FunctionName("EventGridTriggerFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log,
        CancellationToken cancellationToken = default)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        _eventGridPublisherClientService.PersistUrlCheckResultAsync(cancellationToken);
    }
}