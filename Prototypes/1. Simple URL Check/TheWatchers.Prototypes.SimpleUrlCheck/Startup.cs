using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheWatchers.Prototypes.SimpleUrlCheck;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TheWatchers.Prototypes.SimpleUrlCheck;

/// <inheritdoc />
/// <remarks>
///     Startup class required to configure dependency injection.
/// </remarks>
public class Startup : FunctionsStartup
{
    /// <inheritdoc />
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddOptions<Configuration>()
            .Configure<IConfiguration>((settings, configuration) => { configuration.Bind(settings); });
    }
}