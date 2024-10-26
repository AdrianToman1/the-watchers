namespace TheWatchers.Prototypes._3.TriggerViaEventGrid.UrlCheck;

/// <summary>
///     Internal constants.
/// </summary>
internal sealed class Constants
{
    /// <summary>
    ///     The key for the Cosmos DB account key environment variable.
    /// </summary>
    internal const string CosmosDbAccountKeyKey = "THEWATCHERS_COSMOSDB_ACCOUNTKEY";

    /// <summary>
    ///     The key for the Cosmos DB account endpoint configuration setting.
    /// </summary>
    internal const string CosmosDbAccountEndpointKey = "CosmosDb:AccountEndpoint";

    /// <summary>
    ///     The key for the Cosmos DB database id configuration setting.
    /// </summary>
    internal const string CosmosDbDatabaseIdKey = "CosmosDb:DatabaseId";

    /// <summary>
    ///     The key for the Cosmos DB container id configuration setting.
    /// </summary>
    internal const string CosmosDbContainerIdKey = "CosmosDb:ContainerId";

    /// <summary>
    ///     The default Cosmos DB account endpoint.
    /// </summary>
    /// <remarks>
    ///     This is the default value for the Azure Cosmos DB emulator.
    /// </remarks>
    internal const string DefaultCosmosDbAccountEndpoint = "https://localhost:8081";

    /// <summary>
    ///     The default Cosmos DB database id.
    /// </summary>
    internal const string DefaultCosmosDatabaseId = "TheWatchers";

    /// <summary>
    ///     The default Cosmos DB container id.
    /// </summary>
    internal const string DefaultCosmosContainerId = "Prototype3";

    /// <summary>
    ///     The maximum number of characters from the HTTP response body to persist.
    /// </summary>
    internal const int MaxNumberOfHttpResponseBodyCharactersToPersist = 1000;
}