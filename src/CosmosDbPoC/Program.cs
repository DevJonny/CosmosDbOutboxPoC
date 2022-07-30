using System.CommandLine;
using CosmosDbPoC.Adapters.CommandLine;
using CosmosDbPoC.Ports.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Serilog;
using Spectre.Console;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddBrighter().AutoFromAssemblies();
    })
    .Build();

var commandProcessor = host.Services.GetRequiredService<IAmACommandProcessor>();

var rootCommand = new RootCommand("Proof of concept app for using Cosmos as a Transactional Outbox");
var endpoint = OptionFactory.AddOption<string>('e', "endpoint", helpText: "The Cosmos DB endpoint to connect to");
var primaryKey = OptionFactory.AddOption<string>('p', "primaryKey", helpText: "The Cosmos DB Primary Key to authenticate with");

rootCommand.AddOption(endpoint);
rootCommand.AddOption(primaryKey);

rootCommand.SetHandler((endpointValue, primaryKeyValue) =>
{
    if (endpointValue is not null)
        Environment.SetEnvironmentVariable("CosmosDb__Outbox__Endpoint", endpointValue, EnvironmentVariableTarget.User);
    
    if (primaryKeyValue is not null)
        Environment.SetEnvironmentVariable("CosmosDb__Outbox__PrimaryKey", primaryKeyValue, EnvironmentVariableTarget.User);
    
    var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
    var email = AnsiConsole.Ask<string>("What's your [green]email[/]?");

    commandProcessor.SendAsync(new CreateContactCommand()
    {
        Name = name,
        Email = email
    });
    
}, endpoint, primaryKey);

await rootCommand.InvokeAsync(args);