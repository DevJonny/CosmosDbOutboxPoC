using System.CommandLine;
using CosmosDbPoC.Ports.Commands;
using Paramore.Brighter;
using Spectre.Console;

namespace CosmosDbPoC.Adapters.CommandLine.Options;

public class AppRootCommand : RootCommand
{
    private readonly IAmACommandProcessor _commandProcessor;
    
    public AppRootCommand(IAmACommandProcessor commandProcessor) : base("Proof of concept app for using Cosmos as a Transactional Outbox")
    {
        _commandProcessor = commandProcessor;
    }

    public AppRootCommand Setup()
    {
        var endpoint = OptionFactory.AddOption<string>('e', "endpoint", helpText: "The Cosmos DB endpoint to connect to");
        var primaryKey = OptionFactory.AddOption<string>('p', "primaryKey", helpText: "The Cosmos DB Primary Key to authenticate with");

        AddOption(endpoint);
        AddOption(primaryKey);
        
        this.SetHandler((endpointValue, primaryKeyValue) =>
        {
            if (endpointValue is not null)
                Environment.SetEnvironmentVariable("CosmosDb__Outbox__Endpoint", endpointValue,
                    EnvironmentVariableTarget.User);

            if (primaryKeyValue is not null)
                Environment.SetEnvironmentVariable("CosmosDb__Outbox__PrimaryKey", primaryKeyValue,
                    EnvironmentVariableTarget.User);

            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
            var email = AnsiConsole.Ask<string>("What's your [green]email[/]?");

            _commandProcessor.SendAsync(new CreateContactCommand
            {
                Name = name,
                Email = email
            });
        }, endpoint, primaryKey);

        return this;
    }
}