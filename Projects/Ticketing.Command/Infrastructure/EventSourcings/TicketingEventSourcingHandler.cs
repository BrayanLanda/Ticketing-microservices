using Ticketing.Command.Application.Aggregates;
using Ticketing.Command.Domain.Abstracts;

namespace Ticketing.Command.Infrastructure.EventSourcings;

public class TicketingEventSourcingHandler : IEventSourcingHandler<TicketAggregate>
{
    public Task<TicketAggregate> GetByIdAsync(string aggregateId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}