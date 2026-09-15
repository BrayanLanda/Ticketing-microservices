using Common.Core.Events;
using DnsClient.Protocol;
using Ticketing.Command.Application.Aggregates;
using Ticketing.Command.Domain.Abstracts;

namespace Ticketing.Command.Infrastructure.EventSourcings;

public class TicketingEventSourcingHandler : IEventSourcingHandler<TicketAggregate>
{
    private readonly IEventStore _eventStore;

    public TicketingEventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<TicketAggregate> GetByIdAsync(string aggregateId, CancellationToken cancellationToken)
    {
        var aggregate = new TicketAggregate();
        var events = await _eventStore.GetEventsAsync(aggregateId, cancellationToken);
        if (events is null || !events.Any()) return aggregate;
        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(x => x.Version).Max();
        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
    {
        await _eventStore.SaveEventsAsync(
            aggregate.Id, aggregate.GetUncommittedChanges(),
            aggregate.Version, cancellationToken
        );

        aggregate.MarkChangesAsCommitted();
    }
}