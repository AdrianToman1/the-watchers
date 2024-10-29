using Microsoft.Azure.Cosmos;
using TheWatchers.Prototypes._6.OfflineEvent.Models;

namespace TheWatchers.Prototypes._6.OfflineEvent.UrlCheck.CosmosService;

/// <inheritdoc />
/// <summary>
///     Initializes a new instance of the <see cref="CosmosDbRepositoryService" /> class.
/// </summary>
/// <param name="container">The Cosmos DB container.</param>
/// <exception cref="ArgumentNullException"><c>container</c> is null.</exception>
public sealed class CosmosDbRepositoryService(Container container) : ICosmosDbRepositoryService
{
    private readonly Container _container = container ?? throw new ArgumentNullException(nameof(container));

    /// <inheritdoc />
    public void Dispose()
    {
        _container.Database.Client.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><c>urlCheckResult</c> is null.</exception>
    public Task PersistUrlCheckResultAsync(UrlCheckResultModel urlCheckResult,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(urlCheckResult);

        return _container.CreateItemAsync(urlCheckResult, new PartitionKey(urlCheckResult.Url), null, cancellationToken);
    }
}