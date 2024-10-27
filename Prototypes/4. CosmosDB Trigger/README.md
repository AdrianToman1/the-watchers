# Prototype 4 - VosmosDB Trigger

Two Azure Functions that use event-based architecture to periodically (every 5 minutes) makes an HTTP request to an URL (https://www.plywoodviolin.solution by default) logging the resulting HTTP response to a Cosmos DB.

## Setup & Configration

Each project for Prototype 4 needs to be individually configured and a Cosmos DB and Azure Event Grid Topic need to be provisioned.

Optionally, to test locally, ngrok needs to be installed.

### Azure Event Grid Topic

Prototype 4 requires accces to an Azure Event Grid Topic. There are no specific requirements for the Azure Event Grid Topic.

### Cosmos DB

Prototype 4 requires accces to an Cosmos DB install, either the Azure Cosmos Emulator or an actual Azure Cosmos DB instance. The required Database and Container will be created automatically if they don't already exist.

### TheWatchers.Prototypes.4.CosmosDbTrigger.TriggerUrlCheck

TheWatchers.Prototypes.4.CosmosDbTrigger.TriggerUrlCheck requires access to an Azure Event Grid Topic. The endpoint for Azure Event Grid Topic is the value `EventGrid:Endpoint` in local.settings.json.

In line with security best practices, the access key for the Azure Event Grid Topic is configured via the environment variable `THEWATCHERS_EVENTGRID_ACCESSKEY`. This enviroment variable must be defined. 

The location.settings.json file can be updated to provide a URL to check, otherwise it will check https://www.plywoodviolin.solution by default.

```json
{
	"IsEncrypted": false,
	"Values": {
		"AzureWebJobsStorage": "UseDevelopmentStorage=true",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "EventGrid:Endpoint": "https://the-watchers-prototype-3-check-url.australiasoutheast-1.eventgrid.azure.net/api/events"

        // Optional settings
        "Url": "https://google.com"
	}
}
```

### TheWatchers.Prototypes.4.CosmosDbTrigger.UrlCheck

TheWatchers.Prototypes.4.CosmosDbTrigger.UrlCheck requires access to a Cosmos DB instance. TheWatchers.Prototypes.4.CosmosDbTrigger.UrlCheck is configured to use the (Azure Cosmos Emulator)[https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21] by default. Follow the Azure Cosmos Emulator installation instructions. Override the CosmosDb:AccountEndpoint settings in local.settings.json as required.

In line with security best practices, the access key for the Cosmos DB is configured via the environment variable `THEWATCHERS_COSMOSDB_ACCOUNTKEY`. This enviroment variable must be defined. 

TheWatchers.Prototypes.4.CosmosDbTrigger.UrlCheck can target an actual Azure Cosmos DB by overriding the CosmosDb:AccountEndpoint in local.settings.json and the `THEWATCHERS_COSMOSDB_ACCOUNTKEY` enviroment variable as required.

Azure will automatically create a Azure Cosmos DB Date the Database Id of `TheWatchers` containing a container with the Container Id of `Prototype2` if either doesn't already exist. The default Database and Container Id can by overridden via the `CosmosDb:DatabaseId` and `CosmosDb:ContainerId` settings in local.settings.json.

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"

        // Optional settings
        "CosmosDb:AccountEndpoint": "https://localhost:8081",
        "CosmosDb:DatabaseId": "TheWatchers",
        "CosmosDb:ContainerId": "Prototype2"
    }
}
```

### TheWatchers.Prototypes.4.CosmosDbTrigger.RespondToUrlCheck

TheWatchers.Prototypes.4.CosmosDbTrigger.RespondToUrlCheck requires access to a Cosmos DB instance. The required Database and Containers will be created automatically if they don't already exist.

Due to the nature of `CosmosDBTrigger` the Azure Cosmos DB databse and container ids are hardocded to `TheWatchers` and `Prototype4` respectively. Currently, if these needs to be change the a code to TheWatchers.Prototypes.4.CosmosDbTrigger.RespondToUrlCheck will be required. Accord to this (StackOverflow)[https://stackoverflow.com/a/57085889] answer, it might be possible to bind these values to settings.

In line with security best practices, the connection string to the Azure Cosmos DB instance is configured via the environment variable `THEWATCHERS_COSMOSDB_CONNECTIONSTRING`. This enviroment variable must be defined.

## ngrok

(ngrok)[https://ngrok.com/] is a tool that allows Azure Event Grid web hooks to tunnel to your Azure Functions running locally.

Creat an ngrok account, and follow the (instructions)[https://ngrok.com/docs/getting-started/} to install and connect. Creating a static domain for ngrok will make it easier get things up and running in the future.

### Testing locally using ngrok

1. Start ngrok using TheWatchers.Prototypes.4.CosmosDbTrigger.UrlCheck port. ie `ngrok http 7164`
    
2. Start both Prototype 3 Azure Function projects.
      
3. Create a new Azure Event Grid Subscription. The Azure Event Grid Subscription must have an Event Schema of `Cloud Event Schema v1.0` and a Endpoint Type of `Web Hook`. The Endpoint Url should take the form of `http://<ngrokurl>/runtime/webhooks/EventGrid?functionName=StoreUrlCheckResultsFunction` where `<ngrokurl>` is the URL provided by ngrok.