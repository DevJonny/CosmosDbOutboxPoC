using CosmosDbPoC.Model;

namespace CosmosDbPoC.Events;

public record IEvent : PersistedEntity
{
    public PersistedEntity PersistedEntity { get; init; }

    public string Type { get; }

    public bool Dispatched { get; set; }
}