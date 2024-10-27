using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWatchers.Prototypes._4.CosmosDbTrigger.Models;

namespace TheWatchers.Prototypes._4.CosmosDbTrigger.RespondToUrlCheck;

/// <summary>
///     Initializes a new instance of the <see cref="CosmosDbTriggerFunction" /> class.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
/// <exception cref="ArgumentNullException"><c>loggerFactory</c> is null.</exception>
public sealed class CosmosDbTriggerFunction(ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<CosmosDbTriggerFunction>() ??
                                       throw new ArgumentNullException(nameof(loggerFactory));

    [Function("CosmosDbTriggerFunction")]
    public void Run([CosmosDBTrigger(
            "TheWatchers",
            "Prototype4",
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
            }
        }
    }
}