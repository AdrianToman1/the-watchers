using System;
using System.Collections.Generic;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TheWatchers.Prototypes.OfflineEvent
{
    public static class UrlCheckedFunction
    {
        [FunctionName("UrlCheckedFunction")]
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
                            new Uri(config["UrlCheckedConfiguration:UrlCheckUrl"]),
                            new AzureKeyCredential(config["UrlCheckedConfiguration:UrlCheckKey"]));

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
