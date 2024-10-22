using System.Threading;
using System.Threading.Tasks;

namespace TheWatchers.Prototypes.TriggerViaEventGrid.EventGrid;

/// <summary>
///     Event Grid Publisher Client service.
/// </summary>
public interface IEventGridPublisherClientService
{
    /// <summary>
    ///     Persist a <see cref="UrlCheckResultModel" /> to the Cosmos DB.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken" /> representing request cancellation.</param>
    public Task PersistUrlCheckResultAsync(CancellationToken cancellationToken = default);
}