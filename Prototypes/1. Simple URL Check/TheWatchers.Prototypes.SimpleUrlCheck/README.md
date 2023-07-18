# Prototype 1 - Simple URL Check

An Azure Function that periodically (every 5 minutes) makes an HTTP request to an URL (https://www.plywoodviolin.solution by default) log's the HTTP response status code.

## Settings

The location.settings.json file can be updated to provide a URL to check, otherwise it will check https://www.plywoodviolin.solution by default.

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "Url": "https://google.com"
    }
}
```
