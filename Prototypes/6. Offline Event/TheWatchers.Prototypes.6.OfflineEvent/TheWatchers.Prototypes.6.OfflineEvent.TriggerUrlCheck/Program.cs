using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWatchers.Prototypes._6.OfflineEvent.TriggerUrlCheck;
using TheWatchers.Prototypes._6.OfflineEvent.TriggerUrlCheck.EventGrid;

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