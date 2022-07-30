using Paramore.Brighter;

namespace CosmosDbPoC.Ports.Commands;

public class CreateContactCommand : Command
{
    public string Name { get; init; }
    public string Email { get; init; }
    
    public CreateContactCommand() : base(Guid.NewGuid()) { }
}