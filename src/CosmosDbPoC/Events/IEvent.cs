using CosmosDbPoC.Model;

namespace CosmosDbPoC.Events;

public interface IEvent
{
    IAmPersisted PersistedEntity { get; init; }
}