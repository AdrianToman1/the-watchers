using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace TheWatchers.Prototypes.StoreResultsUrlCheck.CosmosService;

/// <inheritdoc />
public class CosmosDbRepositoryService : ICosmosDbRepositoryService
{
    private readonly Container _container;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CosmosDbRepositoryService" /> class.
    /// </summary>
    /// <param name="container">The Cosmos DB container.</param>
    /// <exception cref="ArgumentNullException"><c>container</c> is null.</exception>
    public CosmosDbRepositoryService(Container container)
    {
        if (container == null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        _container = container;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _container?.Database.Client.Dispose();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><c>urlCheckResult</c> is null.</exception>
    public Task PersistUrlCheckResultAsync(UrlCheckResultModel urlCheckResult,
        CancellationToken cancellationToken = default)
    {
        if (urlCheckResult == null)
        {
            throw new ArgumentNullException(nameof(urlCheckResult));
        }

        return _container.CreateItemAsync(urlCheckResult, new PartitionKey(urlCheckResult.Url));
    }
}