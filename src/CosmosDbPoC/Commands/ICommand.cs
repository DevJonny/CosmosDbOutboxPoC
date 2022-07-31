namespace CosmosDbPoC.Commands;

public interface ICommand
{
    Guid Id { get; }

    Task Execute();
}