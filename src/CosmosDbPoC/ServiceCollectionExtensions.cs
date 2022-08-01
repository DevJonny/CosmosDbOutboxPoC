using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

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
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                }
            }).Result;

        await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbOptions.Database);
        await cosmosClient
            .GetDatabase(cosmosDbOptions.Database)
            .CreateContainerIfNotExistsAsync(cosmosDbOptions.Container, "/id");
        
        var container = cosmosClient.GetContainer(cosmosDbOptions.Database, cosmosDbOptions.Database);
        
        serviceCollection.AddSingleton(container);
    }
}