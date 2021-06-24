using System;
using System.Collections.Generic;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TheWatchers.Prototypes.UrlChecking;

namespace TheWatchers.Prototypes.CosmosDbTrigger
{
    public static class CosmosDbTriggerFunction
    {
        [FunctionName("CosmosDbTriggerFunction")]
        public static void Run([CosmosDBTrigger(
            databaseName: "FamilyDatabase",
            collectionName: "ResultContainer",
            ConnectionStringSetting = "ConnectionString",
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ExecutionContext context, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                foreach (var document in input)
                {
                    var urlCheckResult = JsonConvert.DeserializeObject<UrlCheckResult>(document.ToString());

                    log.LogInformation($"{urlCheckResult.ExecutedAt} {urlCheckResult.Url} {urlCheckResult.StatusCode}");


                    if (urlCheckResult.StatusCode.StatusCode == 200)
                    {
                        var config = new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json")
                            .AddUserSecrets<Configuration>()
                            .Build();

                        var client = new EventGridPublisherClient(
                            new Uri("https://urlcheckresult.australiasoutheast-1.eventgrid.azure.net/api/events"),
                            new AzureKeyCredential("qiCfR/T8RO0rTpcUAFSR2/22T4uZuLsnGryc+oAwyt0="));

                        //var client = new EventGridPublisherClient(
                        //    new Uri(config["EventGridUrl"]),
                        //    new AzureKeyCredential(config["EventGridKey"]));

                        // Add EventGridEvents to a list to publish to the topic
                        EventGridEvent egEvent =
                            new EventGridEvent(
                                "ExampleEventSubject",
                                "Example.EventType",
                                "1.0",
                                "This is the event data");

                        // Send the event
                        client.SendEvent(egEvent);
                    }
                }
            }
        }
    }
}
