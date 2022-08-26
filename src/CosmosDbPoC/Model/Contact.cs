using Newtonsoft.Json;

namespace CosmosDbPoC.Model;

public record Contact(string Name, string Email) : PersistedEntity
{
    public override string ToString()
        => JsonConvert.SerializeObject(this);
}