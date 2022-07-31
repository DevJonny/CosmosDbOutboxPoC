using System.CommandLine;
using CosmosDbPoC;
using CosmosDbPoC.CommandLine.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var cosmosDbOptions = config.GetRequiredSection(CosmosDbOptions.CosmosDb).Get<CosmosDbOptions>();

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(cosmosDbOptions);
    })
    .UseSerilog()
    .Build();

await new AppRootCommand(cosmosDbOptions)
    .Setup()
    .InvokeAsync(args);