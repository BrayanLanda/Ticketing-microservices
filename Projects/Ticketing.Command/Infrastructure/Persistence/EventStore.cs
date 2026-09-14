using Common.Core.Events;
using Ticketing.Command.Domain.Abstracts;
using Ticketing.Command.Domain.EventModels;

namespace Ticketing.Command.Infrastructure.Persistence;

public class EventStore : IEventStore
{
    private readonly IEventModelRepository _eventModelRepository;

    public EventStore(IEventModelRepository eventModelRepository)
    {
        _eventModelRepository = eventModelRepository;
    }
    public async Task SaveEventsAsync(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, CancellationToken cancellationToken)
    {
        var eventSteam = await _eventModelRepository.FilterByAsync(doc => doc.AggegateIdentifier == aggregateId, cancellationToken);
        if(eventSteam.Any() && expectedVersion != 1 && eventSteam.Last().Version != expectedVersion)
        {
            throw new Exception("Concurrency Error");
        }
        var version = expectedVersion;
        foreach(var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
              Timestamp = DateTime.UtcNow,
              AggegateIdentifier = aggregateId,
              Version = version,
              EventType = eventType,
              EventData = @event  
            };
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(string aggregateId, CancellationToken cancellationToken)
    {
        var eventStream = await _eventModelRepository.FilterByAsync(doc => doc.AggegateIdentifier == aggregateId, cancellationToken);
        if(eventStream is null || !eventStream.Any())
            throw new Exception("The aggregate has no events");
        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList()!;
    }
}
