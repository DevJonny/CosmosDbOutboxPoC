namespace CosmosDbPoC.Model;

public record Contact(string Name, string Email) : IAmPersisted
{
    public Guid Id { get; } = Guid.NewGuid();
}