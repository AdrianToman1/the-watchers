namespace TheWatchers.Prototypes._6.OfflineEvent.RespondToUrlCheck;

/// <summary>
///     Internal constants.
/// </summary>
internal sealed class Constants
{
    /// <summary>
    ///     The default Event Grid event type.
    /// </summary>
    internal const string EventGridEventType = "TheWatcher.UrlOffline";

    /// <summary>
    ///     The default Event Grid event subject.
    /// </summary>
    internal const string EventGridEventSubject = "Prototype6";

    /// <summary>
    ///     The key for the Event Grid Topic endpoint configuration setting.
    /// </summary>
    internal const string EventGridEndpointKey = "EventGrid:Endpoint";

    /// <summary>
    ///     The key for the Event Grid Topic access key environment variable.
    /// </summary>
    internal const string CosmosDbAccessKeyKey = "THEWATCHERS_EVENTGRID_ACCESSKEY";
}