namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.EventGridTrigger;

/// <summary>
///     Internal constants.
/// </summary>
internal sealed class Constants
{
    /// <summary>
    ///     The default URL to check.
    /// </summary>
    internal const string DefaultUrl = "https://www.plywoodviolin.solutions/";

    /// <summary>
    ///     The default Event Grid event type.
    /// </summary>
    internal const string EventGridEventType = "TheWatcher.DoUrlCheckEvent";

    /// <summary>
    ///     The default Event Grid event subject.
    /// </summary>
    internal const string EventGridEventSubject = "Prototype3";

    /// <summary>
    ///     The key for the URL configuration setting.
    /// </summary>
    internal const string UrlKey = "Url";

    /// <summary>
    ///     The key for the Event Grid Topic endpoint configuration setting.
    /// </summary>
    internal const string EventGridEndpointKey = "EventGrid:Endpoint";

    /// <summary>
    ///     The key for the Event Grid Topic access key environment variable.
    /// </summary>
    internal const string CosmosDbAccessKeyKey = "THEWATCHERS_EVENTGRID_ACCESSKEY";
}