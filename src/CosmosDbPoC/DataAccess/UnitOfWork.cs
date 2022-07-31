using CosmosDbPoC.Model;
using Serilog;

namespace CosmosDbPoC.DataAccess;

public class UnitOfWork
{
    private readonly List<IAmPersisted> _entitiesToPersist = new();

    public void Add(IAmPersisted entity)
    {
        _entitiesToPersist.Add(entity);
    }

    public async Task SaveChanges()
    {
        foreach (var entity in _entitiesToPersist)
        {
            Log.Information("Persisting {EntityId}", entity.Id);
        }
        
        _entitiesToPersist.Clear();
    }
}