namespace TheWatchers.Prototypes.StoreResultsUrlCheck;

/// <summary>
///     Internal constants.
/// </summary>
internal class Constants
{
    /// <summary>
    ///     The default URL to check.
    /// </summary>
    internal const string DefaultUrl = "https://www.plywoodviolin.solutions";

    /// <summary>
    ///     The default Cosmos DB account endpoint.
    /// </summary>
    /// <remarks>
    ///     This is the default value for the Azure Cosmos DB emulator.
    /// </remarks>
    internal const string DefaultCosmosDbAccountEndpoint = "https://localhost:8081";

    /// <summary>
    ///     The default Cosmos DB account key.
    /// </summary>
    /// <remarks>
    ///     This is the default value for the Azure Cosmos DB emulator.
    /// </remarks>
    internal const string DefaultCosmosDbAccountKey =
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

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