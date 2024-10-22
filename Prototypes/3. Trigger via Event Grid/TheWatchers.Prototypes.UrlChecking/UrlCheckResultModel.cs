using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheWatchers.Prototypes.TriggerViaEventGrid;

public class UrlCheckResultModel
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

    public HttpStatusCodeModel StatusCode { get; set; }

    // Don't assume that headers are a dictionary.
    public List<Tuple<string, IEnumerable<string>>> Headers { get; set; }
    public string Body { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class HttpStatusCodeModel
{
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; }
}