using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TheWatchers.Prototypes.SimpleUrlCheck
{
    public static class SimpleUrlCheckFunction
    {
        private static readonly HttpClient Client = new HttpClient();

        [FunctionName("SimpleUrlCheckFunction")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            const string url = "https://www.plywoodviolin.solutions/";

            var response = await Client.GetAsync(url);

            log.LogInformation($"{DateTime.Now} {url} {response.StatusCode}");
        }
    }
}