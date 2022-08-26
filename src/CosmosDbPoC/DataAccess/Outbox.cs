using CosmosDbPoC.Events;

namespace CosmosDbPoC.DataAccess;

public class Outbox
{
    private readonly List<IEvent> _persistedEvents = new();

    public void Add(IEvent persistedEvent)
    {
        _persistedEvents.Add(persistedEvent);
    }

    public List<IEvent> GetAll()
    {
        return _persistedEvents;
    }
}