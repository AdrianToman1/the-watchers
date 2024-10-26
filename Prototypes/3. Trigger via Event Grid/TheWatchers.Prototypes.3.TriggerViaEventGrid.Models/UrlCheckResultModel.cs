using Newtonsoft.Json;

namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.Models;

/// <summary>
///     Model representing the result of a URL check.
/// </summary>
public sealed class UrlCheckResultModel
{
    /// <summary>
    ///     Unique identifier for the result.
    /// </summary>
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    ///     Date and time the check was executed.
    /// </summary>
    public DateTimeOffset ExecutedAt { get; set; }

    /// <summary>
    ///     Date and time the check was scheduled for.
    /// </summary>
    public DateTime? ScheduledFor { get; set; }

    /// <summary>
    ///     Date and time the HTTP request to the URL was sent.
    /// </summary>
    public DateTimeOffset RequestSentAt { get; set; }

    /// <summary>
    ///     Date and time the remote server HTTP sent the response.
    /// </summary>
    /// <remarks>
    ///     This may be null if the remote server did not include the data header as part of the response.
    /// </remarks>
    public DateTimeOffset? ResponseSentAt { get; set; }

    /// <summary>
    ///     Date and time the response was received.
    /// </summary>
    public DateTimeOffset ResponseReceivedAt { get; set; }

    /// <summary>
    ///     The URL that is the subject of the check.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    ///     The HTTP status code returned in the response.
    /// </summary>
    public HttpStatusCodeModel StatusCode { get; set; }

    /// <summary>
    ///     The HTTP headers returned in the response.
    /// </summary>
    /// <remarks>
    ///     It is possible for the response to have multiple headers with the same name, so the headers can not be represented
    ///     as a dictionary.
    /// </remarks>
    public List<Tuple<string, IEnumerable<string>>> Headers { get; set; }

    /// <summary>
    ///     The body of the HTTP response.
    /// </summary>
    public string Body { get; set; }
}