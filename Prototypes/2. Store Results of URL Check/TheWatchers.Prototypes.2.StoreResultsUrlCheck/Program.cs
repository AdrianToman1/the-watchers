using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWatchers.Prototypes._2.StoreResultsUrlCheck;
using TheWatchers.Prototypes._2.StoreResultsUrlCheck.CosmosService;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddCosmosDb(Environment.GetEnvironmentVariable(Constants.CosmosDbAccountKeyKey)!);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();