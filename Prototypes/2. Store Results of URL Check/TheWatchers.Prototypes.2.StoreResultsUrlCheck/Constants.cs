namespace TheWatchers.Prototypes._2.StoreResultsUrlCheck;

/// <summary>
///     Internal constants.
/// </summary>
internal sealed class Constants
{
    /// <summary>
    ///     The default URL to check.
    /// </summary>
    internal const string DefaultUrl = "https://plywood-violin.azurewebsites.net/";

    /// <summary>
    ///     The key for the URL configuration setting.
    /// </summary>
    internal const string UrlKey = "Url";

    internal const string CosmosDbAccountKeyKey = "THEWATCHERS_COSMOSDB_ACCOUNTKEY";

    internal const string CosmosDbAccountEndpointKey = "CosmosDb:AccountEndpoint";

    internal const string CosmosDbDatabaseIdKey = "CosmosDb:DatabaseId";

    internal const string CosmosDbContainerIdKey = "CosmosDb:ContainerId";
    
    /// <summary>
    ///     The default Cosmos DB account endpoint.
    /// </summary>
    /// <remarks>
    ///     This is the default value for the Azure Cosmos DB emulator.
    /// </remarks>
    internal const string DefaultCosmosDbAccountEndpoint = "https://localhost:8081";

    /// <summary>
    ///     The default Cosmos DB database Id.
    /// </summary>
    internal const string DefaultCosmosDatabaseId = "TheWatchers";

    /// <summary>
    ///     The default Cosmos DB container Id.
    /// </summary>
    internal const string DefaultCosmosContainerId = "Prototype2";

    /// <summary>
    ///     The maximum number of characters from the HTTP response body to persist.
    /// </summary>
    internal const int MaxNumberOfHttpResponseBodyCharactersToPersist = 1000;

}