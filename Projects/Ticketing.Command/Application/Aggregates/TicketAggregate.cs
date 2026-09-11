using Common.Core.Events;
using Ticketing.Command.Domain.Abstracts;
using static Ticketing.Command.Features.Tickets.TicketCreate;

namespace Ticketing.Command.Application.Aggregates;

public class TicketAggregate : AggregateRoot
{
    public bool Active { get; set; }
    public TicketAggregate(TicketCreateCommand command)
    {
        var ticketCreateEvent = new TicketCreatedEvent
        {
            Id = command.Id,
            Username = command.ticketCreateRequest.Username,
            TypeError = command.ticketCreateRequest.TypeError,
            DetailError = command.ticketCreateRequest.DetailError
        };
        RaiseEvent(ticketCreateEvent);
    }

    public void Apply(TicketCreatedEvent @event)
    {
        _id = @event.Id;
        Active = true;
    }
}
