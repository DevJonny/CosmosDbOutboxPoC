using CosmosDbPoC.DataAccess;
using CosmosDbPoC.Events;
using CosmosDbPoC.Model;
using Serilog;

namespace CosmosDbPoC.Commands;

public class CreateContactCommand : ICommand
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; init; }
    public string Email { get; init; }

    private readonly UnitOfWork _unitOfWork;

    public CreateContactCommand(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        Log.Information("Creating Contact with {ContactName} and {ContactEmail}", Name, Email);

        var contact = new Contact(Name, Email);
        
        _unitOfWork.Add(contact);
        _unitOfWork.Add(new ContactCreatedEvent(contact));

        await _unitOfWork.SaveChanges();
    }
}