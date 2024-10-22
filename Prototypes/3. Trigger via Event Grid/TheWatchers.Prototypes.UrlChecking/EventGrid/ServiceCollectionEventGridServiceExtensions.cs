using System;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TheWatchers.Prototypes.TriggerViaEventGrid.EventGrid;

/// <summary>
///     Extension methods for adding Event Grid services to the <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionEventGridServiceExtensions
{
    /// <summary>
    ///     Adds <see cref="IEventGridPublisherClientService" /> to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add <see cref="IEventGridPublisherClientService" /> to.</param>
    /// <exception cref="ArgumentNullException"><c>services</c> is null.</exception>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddEventGrid(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IEventGridPublisherClientService>(sp =>
        {
            var configuration = sp.GetService<IOptions<EventGridSettings>>();

            return new EventGridPublisherClientService(new EventGridPublisherClient(
                new Uri(configuration.Value.Url),
                new AzureKeyCredential(configuration.Value.Key)));
        });

        return services;
    }
}