using TheWatchers.Prototypes._3.TriggerViaEventGrid.Models;

namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.UrlCheck.CosmosService;

/// <summary>
///     Cosmos DB repository service.
/// </summary>
public interface ICosmosDbRepositoryService : IDisposable
{
    /// <summary>
    ///     Persist a <see cref="UrlCheckResultModel" /> to the Cosmos DB.
    /// </summary>
    /// <param name="ueCheckResultModel">The <see cref="UrlCheckResultModel" /> to persist.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    public Task PersistUrlCheckResultAsync(UrlCheckResultModel ueCheckResultModel,
        CancellationToken cancellationToken = default);
}