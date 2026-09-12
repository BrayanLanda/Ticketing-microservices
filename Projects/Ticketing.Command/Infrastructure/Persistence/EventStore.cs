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
    public Task SaveEventsAsync(string aggregateId, IEnumerable<BaseEvent> events, int expectedVesion, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BaseEvent>> GetEventsAsync(string aggregateId, CancellationToken cancellationToken)
    {
        var eventStream = await _eventModelRepository.FilterByAsync(doc => doc.AggegateIdentifier == aggregateId, cancellationToken);
        if(eventStream is null || !eventStream.Any())
            throw new Exception("The aggregate has no events");
        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList()!;
    }
}
