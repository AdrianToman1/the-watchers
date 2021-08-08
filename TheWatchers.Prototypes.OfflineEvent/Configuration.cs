namespace TheWatchers.Prototypes.OfflineEvent
{
    public class Configuration
    {
        public TriggerUrlCheckConfiguration TriggerUrlCheck { get; set; }

        public UrlCheckedFunctionConfiguration UrlCheckedFunction { get; set; }
    }

    public class TriggerUrlCheckConfiguration
    {
        public string UrlCheckUrl { get; set; }

        public string UrlCheckKey { get; set; }
    }

    public class UrlCheckedFunctionConfiguration
    {
        public string UrlCheckedUrl { get; set; }

        public string UrlCheckedKey { get; set; }
    }
}
