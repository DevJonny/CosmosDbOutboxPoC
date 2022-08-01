using CosmosDbPoC.Model;

namespace CosmosDbPoC.Events;

public record ContactCreatedEvent : IEvent, IAmPersisted
{
    public ContactCreatedEvent(Contact contact)
    {
        PersistedEntity = contact;
    }
    
    public Guid Id { get; } = Guid.NewGuid();
    public IAmPersisted PersistedEntity { get; init; }
}