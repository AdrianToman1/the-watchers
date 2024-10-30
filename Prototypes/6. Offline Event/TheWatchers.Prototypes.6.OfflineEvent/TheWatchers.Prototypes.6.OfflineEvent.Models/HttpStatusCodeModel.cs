﻿namespace TheWatchers.Prototypes._6.OfflineEvent.Models;

/// <summary>
///     Model representing an HTTP response status code.
/// </summary>
public sealed class HttpStatusCodeModel
{
    /// <summary>
    ///     The status code.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    ///     The reason phrase.
    /// </summary>
    public string ReasonPhrase { get; set; }
}