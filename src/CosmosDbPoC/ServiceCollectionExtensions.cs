using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CosmosDbPoC;

public static class ServiceCollectionExtensions
{
    public static async Task AddCosmosDb(this IServiceCollection serviceCollection, CosmosDbOptions cosmosDbOptions)
    {
        var cosmosClient = CosmosClient.CreateAndInitializeAsync(
            cosmosDbOptions.Endpoint,
            cosmosDbOptions.PrimaryKey,
            Array.Empty<(string databaseId, string containerId)>(),
            new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                }
            }).Result;

        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbOptions.Database);
        Container container = await database.CreateContainerIfNotExistsAsync(cosmosDbOptions.Container, cosmosDbOptions.PartitionKey);
        Container leaseContainer = await database.CreateContainerIfNotExistsAsync(cosmosDbOptions.LeaseContainer, "/partitionKey");

        var throughPut = await leaseContainer.ReadThroughputAsync();
        
        Log.Information("Current Container through put {Throughput}", throughPut);
        
        if (throughPut < 400)
        {
            await leaseContainer.ReplaceThroughputAsync(throughPut.Value + 100);
        }

        serviceCollection.AddSingleton(cosmosClient);
    }
}