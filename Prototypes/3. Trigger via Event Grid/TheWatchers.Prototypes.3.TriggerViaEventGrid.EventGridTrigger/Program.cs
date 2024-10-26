using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWatchers.Prototypes._3.TriggerViaEventGrid.EventGridTrigger;
using TheWatchers.Prototypes._3.TriggerViaEventGrid.EventGridTrigger.EventGrid;

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