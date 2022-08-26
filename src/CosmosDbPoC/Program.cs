using System.CommandLine;
using CosmosDbPoC;
using CosmosDbPoC.CommandLine.Commands;
using CosmosDbPoC.DataAccess;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var cosmosDbOptions = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build()
    .GetRequiredSection(CosmosDbOptions.CosmosDb)
    .Get<CosmosDbOptions>();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(async services =>
    {
        services.AddSingleton(cosmosDbOptions);
        services.AddSingleton(new Outbox());
        services.AddCosmosDb(cosmosDbOptions).Wait();
    })
    .UseSerilog()
    .Build();

await new AppRootCommand(
        host.Services.GetService<CosmosClient>(),
        host.Services.GetService<CosmosDbOptions>(),
        host.Services.GetService<Outbox>())
    .Setup()
    .InvokeAsync(args);