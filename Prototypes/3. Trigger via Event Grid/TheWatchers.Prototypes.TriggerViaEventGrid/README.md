# Prototype 3 - Store Results of URL Check

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














curl --location 'http://localhost:7078/runtime/webhooks/EventGrid?functionName=EventGridUrlCheckFunction' \
--header 'Content-Type: application/json' \
--header 'aeg-event-type: Notification' \
--data '{
    "id": "Incoming_20200918002745d29ebbea-3341-4466-9690-0a03af35228e",
    "topic": "/subscriptions/50ad1522-5c2c-4d9a-a6c8-67c11ecb75b8/resourcegroups/acse2e/providers/microsoft.communication/communicationservices/{communication-services-resource-name}",
    "subject": "/phonenumber/15555555555",
    "data": {
        "Url": "https://www.plywoodviolin.solutions",
        "ScheduledFor": "2020-09-18T00:27:45.32Z"
    },
    "eventType": "Microsoft.Communication.SMSReceived",
    "dataVersion": "1.0",
    "metadataVersion": "1",
    "eventTime": "2020-09-18T00:27:47Z"
}'




docker run `
        pmcilreavy/azureeventgridsimulator:latest `
        --publish 60101:60101 `
        -v C:\Repos\the-watchers\aegs:/aegs `
        -e ASPNETCORE_ENVIRONMENT=Development `
        -e ASPNETCORE_Kestrel__Certificates__Default__Path=/aegs/certificate.pfx `
        -e ASPNETCORE_Kestrel__Certificates__Default__Password=$CREDENTIAL_PLACEHOLDER$ `
        -e AEGS_Topics__0__name=ExampleTopic `
        -e AEGS_Topics__0__port=60101 `
        -e AEGS_Topics__0__key=TheLocal+DevelopmentKey `
        -e AEGS_Topics__0__subscribers__0__name=Example.EventType `
        -e AEGS_Topics__0__subscribers__0__endpoint=http://localhost:7078/runtime/webhooks/EventGrid?functionName=EventGridUrlCheckFunction `
        -e AEGS_Topics__0__subscribers__0__disableValidation=true `
        -e AEGS_Serilog__MinimumLevel__Default=Verbose





C:\Repos\the-watchers

docker run `
        --detach `
        --publish 60101:60101 `
        -v C:\Repos:/aegs `
        -e ASPNETCORE_ENVIRONMENT=Development `
        -e ASPNETCORE_Kestrel__Certificates__Default__Path=/aegs/certificate.pfx `
        -e ASPNETCORE_Kestrel__Certificates__Default__Password=$CREDENTIAL_PLACEHOLDER$ `
        pmcilreavy/azureeventgridsimulator:latest
        -e TZ=Australia/Brisbane `
        -e AEGS_Topics__0__name=ExampleTopic `
        -e AEGS_Topics__0__port=60101 `
        -e AEGS_Topics__0__key=TheLocal+DevelopmentKey= `
        -e AEGS_Topics__0__subscribers__0__name=RequestCatcherSubscription `
        -e AEGS_Topics__0__subscribers__0__endpoint=https://azureeventgridsimulator.requestcatcher.com/ `
        -e AEGS_Topics__0__subscribers__0__disableValidation=true `
        -e AEGS_Serilog__MinimumLevel__Default=Verbose `


