using Common.Core.Events;

namespace Ticketing.Command.Domain.Abstracts;

public interface IEventStore
{
    Task<List<BaseEvent>> GetEventsAsync(string aggregateId, CancellationToken cancellationToken);
    Task SaveEventsAsync(string aggregateId, IEnumerable<BaseEvent> events, int expectedVesion, CancellationToken cancellationToken);
}
