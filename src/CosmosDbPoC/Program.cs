using System.CommandLine;
using CosmosDbPoC.Adapters.CommandLine.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Serilog;

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

await new AppRootCommand(commandProcessor)
    .Setup()
    .InvokeAsync(args);