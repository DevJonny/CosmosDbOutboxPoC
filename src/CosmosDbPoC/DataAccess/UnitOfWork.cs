using CosmosDbPoC.Model;
using Microsoft.Azure.Cosmos;

namespace CosmosDbPoC.DataAccess;

public class UnitOfWork
{
    private readonly Container _container;
    private readonly List<PersistedEntity> _entitiesToPersist = new();

    public UnitOfWork(Container container)
    {
        _container = container;
    }
    
    public void Add(PersistedEntity entity)
    {
        _entitiesToPersist.Add(entity);
    }

    public async Task SaveChanges()
    {
        var entitiesGroupedByPartitionKey = 
            _entitiesToPersist.GroupBy(e => e.PartitionKey);

        foreach (var entityGroup in entitiesGroupedByPartitionKey)
        {
            var batch = _container.CreateTransactionalBatch(new PartitionKey(entityGroup.Key.ToString()));
            var entities = entityGroup.AsEnumerable();

            foreach (var entity in entities)
            {
                batch.CreateItem(entity);
            }

            await batch.ExecuteAsync();
        }

        _entitiesToPersist.Clear();
    }
}