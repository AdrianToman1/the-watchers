namespace TheWatchers.Prototypes.StoreResultsUrlCheck;

/// <summary>
///     Cosmos DB configuration settings.
/// </summary>
public class CosmosDbSettings
{
    /// <summary>
    ///     The Cosmos DB account endpoint.
    /// </summary>
    /// <remarks>
    ///     If not defined it will default to <see cref="Constants.DefaultCosmosDbAccountEndpoint" />, which is the default
    ///     value for the Azure Cosmos DB emulator.
    /// </remarks>
    public string AccountEndpoint { get; set; } = Constants.DefaultCosmosDbAccountEndpoint;

    /// <summary>
    ///     The Cosmos DB account key.
    /// </summary>
    /// <remarks>
    ///     If not defined it will default to <see cref="Constants.DefaultCosmosDbAccountKey" />, which is the default value
    ///     for the Azure Cosmos DB emulator.
    /// </remarks>
    public string AccountKey { get; set; } = Constants.DefaultCosmosDbAccountKey;

    /// <summary>
    ///     The Cosmos DB database Id.
    /// </summary>
    /// <remarks>
    ///     If not defined it will default to <see cref="Constants.DefaultCosmosDatabaseId" />.
    /// </remarks>
    public string DatabaseId { get; set; } = Constants.DefaultCosmosDatabaseId;

    /// <summary>
    ///     The Cosmos DB container Id.
    /// </summary>
    /// <remarks>
    ///     If not defined it will default to <see cref="Constants.DefaultCosmosContainerId" />.
    /// </remarks>
    public string ContainerId { get; set; } = Constants.DefaultCosmosContainerId;
}