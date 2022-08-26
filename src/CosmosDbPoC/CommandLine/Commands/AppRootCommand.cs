using System.CommandLine;
using CosmosDbPoC.Commands;
using CosmosDbPoC.DataAccess;
using Microsoft.Azure.Cosmos;
using Spectre.Console;

namespace CosmosDbPoC.CommandLine.Commands;

public class AppRootCommand : RootCommand
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly Outbox _outbox;

    private bool _running = true;
    
    public AppRootCommand(CosmosClient cosmosClient, CosmosDbOptions cosmosDbOptions, Outbox outbox) : base("Proof of concept app for using Cosmos as a Transactional Outbox")
    {
        _cosmosClient = cosmosClient;
        _cosmosDbOptions = cosmosDbOptions;
        _outbox = outbox;
    }

    public AppRootCommand Setup()
    {
        this.SetHandler(async () =>
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.Database, _cosmosDbOptions.Container);
            
            var feedProcessor = new FeedProcessor.FeedProcessor(_cosmosClient, _cosmosDbOptions, _outbox);
            var changeFeedProcessor = await feedProcessor.StartChangeFeedProcessorAsync();

            do
            {
                var command = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What do you want to do?")
                        .AddChoices(AddNewContact, ViewOutbox, Quit));

                Action commandToRun = command switch
                {
                    AddNewContact => async () => await AddNewContactCommand(container),
                    ViewOutbox => ViewOutboxCommand,
                    Quit => () => _running = false,
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                commandToRun.Invoke();
                
            } while (_running);

            await changeFeedProcessor.StopAsync();
        });

        return this;
    }

    private async Task AddNewContactCommand(Container container)
    {
        var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
        var email = AnsiConsole.Ask<string>("What's your [green]email[/]?");

        await new CreateContactCommand(new UnitOfWork(container))
        {
            Name = name,
            Email = email
        }.Execute();
    }

    private void ViewOutboxCommand()
    {
        AnsiConsole.WriteLine("Events in the Outbox:");
        
        foreach (var @event in _outbox.GetAll())
        {
            AnsiConsole.WriteLine($"{@event.Type} for {@event.PersistedEntity.Id}");
        }
    }

    private const string AddNewContact = "Add a new Contact";
    private const string ViewOutbox = "View Outbox";
    private const string Quit = "Quit";
}