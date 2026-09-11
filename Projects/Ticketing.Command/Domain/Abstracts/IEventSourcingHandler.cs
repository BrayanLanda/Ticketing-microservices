namespace Ticketing.Command.Domain.Abstracts;

public interface IEventSourcingHandler<T>
{
    Task<T> GetByIdAsync(string aggregateId, CancellationToken cancellationToken);
    Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken);
}