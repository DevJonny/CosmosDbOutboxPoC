using Newtonsoft.Json;

namespace CosmosDbPoC.Model;

public record PersistedEntity
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; } = Guid.NewGuid();

    [JsonProperty(PropertyName = "entityId")]
    public virtual Guid PartitionKey => Id;
}