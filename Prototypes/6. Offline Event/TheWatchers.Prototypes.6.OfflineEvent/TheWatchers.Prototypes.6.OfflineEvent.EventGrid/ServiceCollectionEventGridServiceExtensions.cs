using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheWatchers.Prototypes._6.OfflineEvent.EventGrid;

/// <summary>
///     Extension methods for adding Event Grid services to the <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionEventGridServiceExtensions
{
    /// <summary>
    ///     Adds <see cref="IEventGridPublisherClientService" /> to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add <see cref="IEventGridPublisherClientService" /> to.</param>
    /// <param name="eventGridKey">The event grid key to use to create the client.</param>
    /// <exception cref="ArgumentNullException"><c>services</c> or <c>eventGridKey</c> is null.</exception>
    /// <exception cref="ArgumentException"><c>eventGridKey</c> is empty or whitespace.</exception>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddEventGrid(this IServiceCollection services, string eventGridKey)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(eventGridKey);
        ArgumentException.ThrowIfNullOrEmpty(eventGridKey);

        services.AddSingleton<IEventGridPublisherClientService>(sp =>
        {
            var configuration = sp.GetService<IConfiguration>();

            var endpoint = configuration![Constants.EventGridEndpointKey]!;

            return new EventGridPublisherClientService(new EventGridPublisherClient(
                new Uri(endpoint),
                new AzureKeyCredential(eventGridKey)));
        });

        return services;
    }
}