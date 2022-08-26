using CosmosDbPoC.DataAccess;
using CosmosDbPoC.Events;
using CosmosDbPoC.Model;
using Microsoft.Azure.Cosmos;
using Serilog;

namespace CosmosDbPoC.FeedProcessor;

public class FeedProcessor
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly Outbox _outbox;

    public FeedProcessor(CosmosClient cosmosClient, CosmosDbOptions cosmosDbOptions, Outbox outbox)
    {
        _cosmosClient = cosmosClient;
        _cosmosDbOptions = cosmosDbOptions;
        _outbox = outbox;
    }

    public async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync()
    {
        var leaseContainer = _cosmosClient.GetContainer(_cosmosDbOptions.Database, _cosmosDbOptions.LeaseContainer);
        
        var changeFeedProcessor = _cosmosClient.GetContainer(_cosmosDbOptions.Database, _cosmosDbOptions.Container)
            .GetChangeFeedProcessorBuilder<IEvent>(processorName: "changeFeedSample", onChangesDelegate: HandleChangesAsync)
            .WithInstanceName("consoleHost")
            .WithLeaseContainer(leaseContainer)
            .Build();

        Log.Debug("Starting Change Feed Processor...");
        await changeFeedProcessor.StartAsync();
        Log.Debug("Change Feed Processor started");
        
        return changeFeedProcessor;
    }
    
    async Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<IEvent> changes, CancellationToken cancellationToken)
    {
        Log.Debug("Change Feed request consumed {RU} RU", context.Headers.RequestCharge);

        foreach (var entity in changes)
        {
            if (entity is not IEvent @event)
                continue;

            Log.Debug("Detected operation for item with id {ItemId}", entity.PersistedEntity.Id);
            
            _outbox.Add(@event);
            
            Log.Information("Added event with Id {Id} for Entity {EntityId}", entity.PersistedEntity.Id, @event.PersistedEntity.Id);
        }

        Log.Verbose("Finished handling changes");
    }
}