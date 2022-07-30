using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace CosmosDbPoC.Ports.Commands.Handlers;

public class CreateContactCommandHandler : RequestHandlerAsync<CreateContactCommand>
{
    private readonly ILogger<CreateContactCommandHandler> _logger;

    public CreateContactCommandHandler(ILogger<CreateContactCommandHandler> logger)
    {
        _logger = logger;
    }

    public override Task<CreateContactCommand> HandleAsync(CreateContactCommand command, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Creating Contact with {ContactName} and {ContactEmail}", command.Name, command.Email);
        
        return base.HandleAsync(command, cancellationToken);
    }
}