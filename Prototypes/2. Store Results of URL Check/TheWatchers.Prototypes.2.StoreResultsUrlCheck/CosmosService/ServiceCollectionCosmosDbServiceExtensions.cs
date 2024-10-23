using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheWatchers.Prototypes._2.StoreResultsUrlCheck.CosmosService;

/// <summary>
///     Extension methods for adding Cosmos DB services to the <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionCosmosDbServiceExtensions
{
    private const string PartitionKeyPath = "/Url";

    /// <summary>
    ///     Adds <see cref="ICosmosDbRepositoryService" /> to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add <see cref="ICosmosDbRepositoryService" /> to.</param>
    /// <param name="accountKey">The cosmos account key or resource token to use to create the client.</param>
    /// <exception cref="ArgumentNullException"><c>services</c> or <c>accountKey</c> is null.</exception>
    /// <exception cref="ArgumentException"><c>accountKey</c> is empty or whitespace.</exception>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddCosmosDb(this IServiceCollection services, string accountKey)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(accountKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountKey);

        services.AddSingleton<ICosmosDbRepositoryService>(sp =>
        {
            var configuration = sp.GetService<IConfiguration>();

            var accountEndpoint = configuration?[Constants.CosmosDbAccountEndpointKey] ??
                                  Constants.DefaultCosmosDbAccountEndpoint;
            var databaseId = configuration?[Constants.CosmosDbDatabaseIdKey] ?? Constants.DefaultCosmosDatabaseId;
            var containerId = configuration?[Constants.CosmosDbContainerIdKey] ?? Constants.DefaultCosmosContainerId;

            return new CosmosDbRepositoryService(GetContainerAsync(
                accountEndpoint,
                accountKey,
                databaseId,
                containerId
            ).GetAwaiter().GetResult());
        });

        return services;
    }

    /// <summary>
    ///     Ensures that all Cosmos DB resources exists, creating them if they don't. Then returns a reference to the container
    ///     object.
    /// </summary>
    /// <param name="accountEndpoint">The cosmos service endpoint to use.</param>
    /// <param name="accountKey">The cosmos account key or resource token to use to create the client.</param>
    /// <param name="databaseId">The database id to use.</param>
    /// <param name="containerId">The container id to use.</param>
    /// <returns>A <see cref="Task" /> containing a <see cref="Container" />.</returns>
    /// <remarks>
    ///     Cosmos DB management is done exclusively via asynchronous methods. Encapsulate the all the asynchronous Cosmos DB
    ///     management into a single asynchronous method to ease calling it from synchronous code.
    /// </remarks>
    private static async Task<Container> GetContainerAsync(string accountEndpoint, string accountKey, string databaseId,
        string containerId)
    {
        ArgumentNullException.ThrowIfNull(accountEndpoint);
        ArgumentNullException.ThrowIfNull(accountKey);
        ArgumentNullException.ThrowIfNull(databaseId);
        ArgumentNullException.ThrowIfNull(containerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountEndpoint);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseId);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerId);

        var cosmosClient = new CosmosClient(accountEndpoint, accountKey);
        var database = (await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId)).Database;
        var container = (await database.CreateContainerIfNotExistsAsync(containerId, PartitionKeyPath))
            .Container;

        return container;
    }
}