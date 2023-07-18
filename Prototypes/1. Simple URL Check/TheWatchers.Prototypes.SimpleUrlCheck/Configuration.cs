namespace TheWatchers.Prototypes.SimpleUrlCheck;

/// <summary>
///     Configuration settings.
/// </summary>
public class Configuration
{
    /// <summary>
    ///     The URL to check.
    /// </summary>
    /// <remarks>
    ///     The URL may a trailing forward slash or not.
    ///     If not defined the URL will default to <see cref="Constants.DefaultUrl" />.
    /// </remarks>
    public string Url { get; set; } = Constants.DefaultUrl;
}