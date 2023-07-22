# Prototype 2 - Store Results of URL Check

An Azure Function that periodically (every 5 minutes) makes an HTTP request to an URL (https://www.plywoodviolin.solution by default) logging the resulting HTTP response to a Cosmos DB.

## Installation

Prototype 2 requires access to a Cosmos DB instance. Prototype 2 is configured to use the (Azure Cosmos Emulator)[https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21] by default. Follow the Azure Cosmos Emulator installation instructions. Override the CosmosDb:AccountEndpoint and CosmosDb:AccountKey settings in local.settings.json as required.

Prototype 2 can target an actual Azure Cosmos DB by overriding the CosmosDb:AccountEndpoint and CosmosDb:AccountKey settings in local.settings.json.

Prototype will automatically create a Azure Cosmos DB Date the Database Id of `TheWatchers` containing a container with the Container Id of `Prototype2` if either doesn't already exist. The default Database and Container Id can by overridden via the CosmosDb:DatabaseId and CosmosDb:ContainerId settings in local.settings.json.

## Settings

The location.settings.json file can be updated to provide a URL to check, otherwise it will check https://www.plywoodviolin.solution by default.

Further, the Cosmos DB configuration settings in location.settings.json file can by overriden as required.

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    },
    // Optional
    "Configuration": {
        "Url": "https://google.com"
    },
    // Optional
    "CosmosDb": {
        "AccountEndpoint": "https://localhost:8081",
        "AccountKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        "DatabaseId": "TheWatchers",
        "ContainerId": "Prototype2"
    }
}
```
