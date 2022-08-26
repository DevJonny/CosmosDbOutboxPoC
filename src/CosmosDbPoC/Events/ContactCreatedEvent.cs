using CosmosDbPoC.Model;
using Newtonsoft.Json;

namespace CosmosDbPoC.Events;

public record ContactCreatedEvent : IEvent
{
    public ContactCreatedEvent(Contact contact)
    {
        PersistedEntity = contact;
    }

    public override Guid PartitionKey => PersistedEntity.Id;
    public string Type => nameof(ContactCreatedEvent);
    public PersistedEntity PersistedEntity { get; init; }
    public bool Dispatched { get; set; }

    public override string ToString()
        => JsonConvert.SerializeObject(this);
}