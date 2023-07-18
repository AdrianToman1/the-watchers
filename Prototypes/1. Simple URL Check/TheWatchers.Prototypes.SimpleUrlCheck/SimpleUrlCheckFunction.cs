using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TheWatchers.Prototypes.SimpleUrlCheck;

/// <summary>
///     Logs the HTTP response status code returned by an URL.
/// </summary>
public class SimpleUrlCheckFunction
{
    private readonly HttpClient _client;
    private readonly Configuration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SimpleUrlCheckFunction" /> class.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClient factory.</param>
    /// <param name="configuration">The configuration settings</param>
    /// <exception cref="ArgumentNullException">
    ///     One of the parameters (<c>httpClientFactory</c>, or <c>configuration</c>) is
    ///     null.
    /// </exception>
    public SimpleUrlCheckFunction(IHttpClientFactory httpClientFactory, IOptions<Configuration> configuration)
    {
        _client = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    ///     Performs a HTTP request to the URL and logs the response status code.
    /// </summary>
    /// <param name="myTimer">The timer schedule information.</param>
    /// <param name="log">The logger.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [FunctionName("SimpleUrlCheckFunction")]
    public async Task DoSimpleUrlCheck([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
    {
        var response = await _client.GetAsync(_configuration.Url);

        log.LogInformation($"{DateTime.Now} {_configuration.Url} {response.StatusCode}");
    }
}