using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheWatchers.Prototypes.StoreResultsUrlCheck;
using TheWatchers.Prototypes.StoreResultsUrlCheck.CosmosService;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TheWatchers.Prototypes.StoreResultsUrlCheck;

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
        builder.Services.AddOptions<Configuration>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("Configuration").Bind(settings);
            });
        builder.Services.AddOptions<CosmosDbSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("CosmosDb").Bind(settings);
            });
        builder.Services.AddCosmosDbService();
    }

    /// <inheritdoc />
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", true)
            .AddEnvironmentVariables()
            .Build();
    }
}