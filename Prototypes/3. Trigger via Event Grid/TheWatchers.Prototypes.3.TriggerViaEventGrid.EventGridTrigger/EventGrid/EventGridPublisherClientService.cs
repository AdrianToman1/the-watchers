using Azure.Messaging.EventGrid;
using TheWatchers.Prototypes._3.TriggerViaEventGrid.Models;

namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.EventGridTrigger.EventGrid;

/// <inheritdoc />
public sealed class EventGridPublisherClientService : IEventGridPublisherClientService
{
    private const string DataVersion = "1.0";

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
    /// <exception cref="ArgumentNullException"><c>url</c> is null.</exception>
    /// <exception cref="ArgumentException"><c>url</c> is empty or whitespace.</exception>
    public Task PersistUrlCheckResultAsync(string url, DateTime? scheduledFor,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        var eventGridEvent =
            new EventGridEvent(
                Constants.EventGridEventSubject,
                Constants.EventGridEventType,
                DataVersion,
                new DoUrlCheckEventData
                {
                    Url = url,
                    ScheduledFor = scheduledFor
                }
            );

        return _eventGridPublisherClient.SendEventAsync(eventGridEvent, cancellationToken);
    }
}