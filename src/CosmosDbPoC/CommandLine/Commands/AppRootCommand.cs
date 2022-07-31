using System.CommandLine;
using CosmosDbPoC.Commands;
using CosmosDbPoC.DataAccess;
using Spectre.Console;

namespace CosmosDbPoC.CommandLine.Commands;

public class AppRootCommand : RootCommand
{
    private readonly CosmosDbOptions _cosmosDbOptions;
    
    public AppRootCommand(CosmosDbOptions cosmosDbOptions) : base("Proof of concept app for using Cosmos as a Transactional Outbox")
    {
        _cosmosDbOptions = cosmosDbOptions;
    }

    public AppRootCommand Setup()
    {
        this.SetHandler(() =>
        {
            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
            var email = AnsiConsole.Ask<string>("What's your [green]email[/]?");

            new CreateContactCommand(new UnitOfWork())
            {
                Name = name,
                Email = email
            }.Execute().Wait();
        });

        return this;
    }
}