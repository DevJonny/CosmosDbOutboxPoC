using CosmosDbPoC.Events;
using CosmosDbPoC.Model;
using Microsoft.Azure.Cosmos;

namespace CosmosDbPoC.DataAccess;

public class UnitOfWork
{
    private readonly Container _container;
    private readonly List<IAmPersisted> _entitiesToPersist = new();

    public UnitOfWork(Container container)
    {
        _container = container;
    }
    
    public void Add(IAmPersisted entity)
    {
        _entitiesToPersist.Add(entity);
    }

    public async Task SaveChanges()
    {
        var entitiesGroupedByPartitionKey = 
            _entitiesToPersist.GroupBy(e => e is IEvent anEvent ? anEvent.PersistedEntity.Id : e.Id);

        foreach (var entityGroup in entitiesGroupedByPartitionKey)
        {
            var partitionKey = new PartitionKey(entityGroup.Key.ToString());
            var tx = _container.CreateTransactionalBatch(partitionKey);
            var entities = entityGroup.AsEnumerable();

            foreach (var entity in entities)
            {
                tx.CreateItem(entity);
            }

            var response = await tx.ExecuteAsync();
        }

        _entitiesToPersist.Clear();
    }
}