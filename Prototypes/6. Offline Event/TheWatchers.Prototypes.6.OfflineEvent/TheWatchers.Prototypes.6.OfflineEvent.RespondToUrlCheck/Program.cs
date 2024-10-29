using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWatchers.Prototypes._6.OfflineEvent.RespondToUrlCheck;
using TheWatchers.Prototypes._6.OfflineEvent.RespondToUrlCheck.EventGrid;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddEventGrid(Environment.GetEnvironmentVariable(Constants.CosmosDbAccessKeyKey)!);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();