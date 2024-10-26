using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TheWatchers.Prototypes._1.SimpleUrlCheck;

/// <summary>
///     Logs the HTTP response status code returned by a URL.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="httpClientFactory">The HttpClient factory.</param>
/// <param name="configuration">The configuration settings.</param>
/// <exception cref="ArgumentNullException"><c>loggerFactory</c>, <c>httpClientFactory</c> or <c>configuration</c> is null.</exception>
public sealed class SimpleUrlCheckFunction(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
{
    private readonly HttpClient _client =
        httpClientFactory.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));

    private readonly IConfiguration _configuration =
        configuration ?? throw new ArgumentNullException(nameof(configuration));

    private readonly ILogger _logger = loggerFactory.CreateLogger<SimpleUrlCheckFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    /// <summary>
    ///     Performs an HTTP request to the URL and logs the response status code, triggered by a timer.
    /// </summary>
    /// <param name="myTimer">The timer schedule information.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Function("SimpleUrlCheckFunction")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        CancellationToken cancellationToken = default)
    {
        var url = _configuration[Constants.UrlKey] ?? Constants.DefaultUrl;

        var response = await _client.GetAsync(url, cancellationToken);

        _logger.LogInformation($"{url} {response.StatusCode}");
    }
}