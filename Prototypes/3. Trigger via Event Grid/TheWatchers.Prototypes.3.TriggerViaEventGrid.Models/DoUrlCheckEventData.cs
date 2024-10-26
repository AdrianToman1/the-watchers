namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.Models;

public sealed class DoUrlCheckEventData
{
    /// <summary>
    ///     The URL to check.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    ///     Date and time the check is scheduled for.
    /// </summary>
    /// <remarks>
    ///     The source of this data is of the type <see cref="DateTime" />.
    /// </remarks>
    public DateTime? ScheduledFor { get; set; }
}