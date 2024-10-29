using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes._6.OfflineEvent.Models;

namespace TheWatchers.Prototypes._6.OfflineEvent.RespondToUrlCheck;

/// <summary>
///     Initializes a new instance of the <see cref="UrlCheckedFunction" /> class.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
/// <exception cref="ArgumentNullException"><c>loggerFactory</c> is null.</exception>
public sealed class UrlCheckedFunction(ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<UrlCheckedFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    [Function("UrlCheckedFunction")]
    public void Run([CosmosDBTrigger(
            "TheWatchers",
            "Prototype6",
            Connection = "THEWATCHERS_COSMOSDB_CONNECTIONSTRING",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]
        IReadOnlyList<UrlCheckResultModel> input)
    {
        if (input != null && input.Count > 0)
        {
            foreach (var urlCheckResult in input)
            {
                _logger.LogInformation($"{urlCheckResult.ExecutedAt} {urlCheckResult.Url} {urlCheckResult.StatusCode}");

                if (urlCheckResult.StatusCode.StatusCode != 200)
                {
                    _logger.LogWarning($"Url {urlCheckResult.Url} is offline");
                }
            }
        }
    }
}