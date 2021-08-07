using System;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TheWatchers.Prototypes.OfflineEvent
{
    public static class TriggerUrlCheckFunction
    {
        [FunctionName("TriggerUrlCheckFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ExecutionContext context, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json")
                .AddUserSecrets<Configuration>()
                .Build();

            var client = new EventGridPublisherClient(
                new Uri(config["TriggerUrlCheck:UrlCheckUrl"]),
                new AzureKeyCredential(config["TriggerUrlCheck:UrlCheckKey"]));

            client.SendEvent(new EventGridEvent(
                "ExampleEventSubject",
                "Example.EventType",
                "1.0",
                new EventData { Stuff = "This is the event data" }));
        }
    }

    public class EventData
    {
      public string Stuff { get; set; }
    }
}
