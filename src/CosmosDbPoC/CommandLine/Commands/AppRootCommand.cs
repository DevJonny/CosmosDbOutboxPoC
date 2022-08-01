using System.CommandLine;
using CosmosDbPoC.Commands;
using CosmosDbPoC.DataAccess;
using Microsoft.Azure.Cosmos;
using Spectre.Console;

namespace CosmosDbPoC.CommandLine.Commands;

public class AppRootCommand : RootCommand
{
    private readonly Container _container;
    
    public AppRootCommand(Container container) : base("Proof of concept app for using Cosmos as a Transactional Outbox")
    {
        _container = container;
    }

    public AppRootCommand Setup()
    {
        this.SetHandler(() =>
        {
            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
            var email = AnsiConsole.Ask<string>("What's your [green]email[/]?");

            new CreateContactCommand(new UnitOfWork(_container))
            {
                Name = name,
                Email = email
            }.Execute().Wait();
        });

        return this;
    }
}