using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TheWatchers.Prototypes.StoreResultsUrlCheck.CosmosService;

/// <summary>
///     Extension methods for adding Cosmos DB services to the <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionCosmosDbRepositoryServiceExtensions
{
    /// <summary>
    ///     Adds the <see cref="ICosmosDbRepositoryService" /> to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add <see cref="ICosmosDbRepositoryService" /> to.</param>
    /// <exception cref="ArgumentNullException"><c>services</c> is null.</exception>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddCosmosDbService(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<ICosmosDbRepositoryService>(sp =>
        {
            var configuration = sp.GetService<IOptions<CosmosDbSettings>>();

            return new CosmosDbRepositoryService(GetContainerAsync(
                configuration.Value.AccountEndpoint,
                configuration.Value.AccountKey,
                configuration.Value.DatabaseId,
                configuration.Value.ContainerId
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
        var cosmosClient = new CosmosClient(accountEndpoint, accountKey);
        var database = (await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId)).Database;
        var container = (await database.CreateContainerIfNotExistsAsync(containerId, "/Url"))
            .Container;

        return container;
    }
}