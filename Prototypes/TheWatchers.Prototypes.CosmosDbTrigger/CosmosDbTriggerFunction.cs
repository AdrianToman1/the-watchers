using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
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
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                foreach (var document in input)
                {
                    var urlCheckResult = JsonConvert.DeserializeObject<UrlCheckResult>(document.ToString());

                    log.LogInformation($"{urlCheckResult.ExecutedAt} {urlCheckResult.Url} {urlCheckResult.StatusCode}");
                }
            }
        }
    }
}
