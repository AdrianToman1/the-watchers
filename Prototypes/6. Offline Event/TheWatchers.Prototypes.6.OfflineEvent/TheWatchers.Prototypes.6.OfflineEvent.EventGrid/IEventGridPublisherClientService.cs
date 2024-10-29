namespace TheWatchers.Prototypes._6.OfflineEvent.EventGrid;

/// <summary>
///     Event Grid Publisher Client service.
/// </summary>
public interface IEventGridPublisherClientService
{
    /// <summary>
    ///     Publishes an event to an Event Grid topic to check the provided URL.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <param name="url">The URL to check.</param>
    /// <param name="scheduledFor">Date and time the check is scheduled for.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    public Task PersistUrlCheckResultAsync(string url, DateTime? scheduledFor,
        CancellationToken cancellationToken = default);
}