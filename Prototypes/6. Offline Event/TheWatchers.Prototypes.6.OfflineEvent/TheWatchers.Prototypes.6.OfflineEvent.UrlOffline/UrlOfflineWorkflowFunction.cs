using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TheWatchers.Prototypes._6.OfflineEvent.UrlOffline
{
    public class UrlOfflineWorkflowFunction
    {
        private readonly ILogger<UrlOfflineWorkflowFunction> _logger;

        public UrlOfflineWorkflowFunction(ILogger<UrlOfflineWorkflowFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(UrlOfflineWorkflowFunction))]
        public void Run([EventGridTrigger] CloudEvent cloudEvent)
        {
            _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
        }
    }
}
