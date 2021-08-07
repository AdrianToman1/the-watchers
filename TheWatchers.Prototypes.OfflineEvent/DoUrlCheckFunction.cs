using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TheWatchers.Prototypes.OfflineEvent
{
    public static class DoUrlCheckFunction
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://localhost:8081";

        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The name of the database and container we will create
        private static string databaseId = "FamilyDatabase";
        private static string containerId = "ResultContainer";

        private static readonly HttpClient Client = new HttpClient();

        // ngrok http -host-header=localhost 7071
        // https://ae40a9377ed8.ngrok.io/runtime/webhooks/EventGrid?functionName=DoUrlCheck

        [FunctionName("DoUrlCheck")]
        public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            var executedAt = DateTimeOffset.Now;

            var url = ((JObject) eventGridEvent.Data)["Url"].Value<string>();

            var requestSentAt = DateTimeOffset.Now;

            var response = await Client.GetAsync(url);

            var responseReceivedAt = DateTimeOffset.Now;

            log.LogInformation($"{DateTime.Now} {url} {response.StatusCode}");

            var urlCheckResult = new UrlCheckResult
            {
                Id = Guid.NewGuid().ToString(),
                ExecutedAt = executedAt,
                ScheduledFor = requestSentAt.DateTime,//myTimer.ScheduleStatus.Next,
                RequestSentAt = requestSentAt,
                ResponseSentAt = response.Headers.Date,
                ResponseReceivedAt = responseReceivedAt,
                StatusCode = new HttpStatusCode
                {
                    StatusCode = (int)response.StatusCode,
                    ReasonPhrase = response.StatusCode.ToString()
                },
                Headers = response.Headers.ToList().ConvertAll(i => new Tuple<string, IEnumerable<string>>(i.Key, i.Value)),
                Url = url
            };

            urlCheckResult.Headers.AddRange(response.Content.Headers.ToList().ConvertAll(i => new Tuple<string, IEnumerable<string>>(i.Key, i.Value)));

            var s = await response.Content.ReadAsStringAsync();

            if (s.Length > 1000)
            {
                s = s.Substring(0, 1000);
            }

            urlCheckResult.Body = s;

            try
            {
                var container = await GetDatabaseContainer();

                var andersenFamilyResponse =
                    await container.CreateItemAsync(urlCheckResult, new PartitionKey(urlCheckResult.Url));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", andersenFamilyResponse.Resource.Id, andersenFamilyResponse.RequestCharge);
            }
            catch (CosmosException de)
            {
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }

        public static async Task<Container> GetDatabaseContainer()
        {
            var cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            var database = (await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId)).Database;
            return (await database.CreateContainerIfNotExistsAsync(containerId, "/Url")).Container;
        }
    }

    public class UrlCheckResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public DateTimeOffset ExecutedAt { get; set; }
        public DateTime ScheduledFor { get; set; }
        public DateTimeOffset RequestSentAt { get; set; }
        // May not be include in response
        public DateTimeOffset? ResponseSentAt { get; set; }
        public DateTimeOffset ResponseReceivedAt { get; set; }
        public string Url { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        // Don't assume that headers are a dictionary.
        public List<Tuple<string, IEnumerable<string>>> Headers { get; set; }
        public string Body { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class HttpStatusCode
    {
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
    }
}
