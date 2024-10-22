using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;

namespace TheWatchers.Prototypes.TriggerViaEventGrid.EventGrid;

/// <inheritdoc />
public class EventGridPublisherClientService : IEventGridPublisherClientService
{
    private readonly EventGridPublisherClient _eventGridPublisherClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventGridPublisherClientService" /> class.
    /// </summary>
    /// <param name="eventGridPublisherClient">The Event Grid Publisher Client.</param>
    /// <exception cref="ArgumentNullException"><c>eventGridPublisherClient</c> is null.</exception>
    public EventGridPublisherClientService(EventGridPublisherClient eventGridPublisherClient)
    {
        _eventGridPublisherClient = eventGridPublisherClient ??
                                    throw new ArgumentNullException(nameof(eventGridPublisherClient));
    }

    /// <inheritdoc />
    public Task PersistUrlCheckResultAsync(string url, DateTime scheduledFor, CancellationToken cancellationToken = default)
    {
        var egEvent =
            new EventGridEvent(
                Constants.EventGridEventSubject,
                Constants.EventGridEventType,
                "1.0",
                new DoUrlCheckEventData
                {
                    Url = url,
                    ScheduledFor = scheduledFor
                }
                );

        return _eventGridPublisherClient.SendEventAsync(egEvent, cancellationToken);
    }
}

public class DoUrlCheckEventData
{
    public string Url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     The source of this data is of the type <see cref="DateTime"/>.
    /// </remarks>
    public DateTime ScheduledFor { get; set; }
}