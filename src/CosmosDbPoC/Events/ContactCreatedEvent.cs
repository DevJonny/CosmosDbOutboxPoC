using CosmosDbPoC.Model;

namespace CosmosDbPoC.Events;

public record ContactCreatedEvent(Contact Contact) : IEvent, IAmPersisted
{
    public Guid Id { get; } = Guid.NewGuid();
}