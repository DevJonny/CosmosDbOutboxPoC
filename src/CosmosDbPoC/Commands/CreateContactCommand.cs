using Serilog;

namespace CosmosDbPoC.Commands;

public class CreateContactCommand : ICommand
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; init; }
    public string Email { get; init; }

    public async Task Execute()
    {
        Log.Information("Creating Contact with {ContactName} and {ContactEmail}", Name, Email);
    }
}