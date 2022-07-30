using Paramore.Brighter;
using Serilog;

namespace CosmosDbPoC.Ports.Commands.Handlers;

public class CreateContactCommandHandler : RequestHandlerAsync<CreateContactCommand>
{
    public override Task<CreateContactCommand> HandleAsync(CreateContactCommand command, CancellationToken cancellationToken = new CancellationToken())
    {
        Log.Information("Creating Contact with {ContactName} and {ContactEmail}", command.Name, command.Email);
        
        return base.HandleAsync(command, cancellationToken);
    }
}