using FluentValidation;
using MediatR;
using Ticketing.Command.Application.Aggregates;
using Ticketing.Command.Domain.Abstracts;
using Ticketing.Command.Features.Apis;

namespace Ticketing.Command.Features.Tickets
{
    public sealed class TicketCreate : IMinimalApi
    {
        public void AddEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("/api/tickets", async (TicketCreateRequest ticketCreateRequest, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var id = Guid.CreateVersion7(DateTimeOffset.UtcNow).ToString();
                var command = new TicketCreateCommand(id, ticketCreateRequest);
                var result = await mediator.Send(command, cancellationToken);
                return result ? Results.Ok() : Results.BadRequest();
            })
            .WithName("CreateTicket")
            .WithOpenApi()
            .Produces(200)
            .Produces(400);
        }
        public sealed class TicketCreateRequest(
            string username, int typeError, string detailError
        )
        {
            public string Username { get; set; } = username;
            public int TypeError { get; set; } = typeError;
            public string DetailError { get; set; } = detailError;
        }

        public record TicketCreateCommand(string Id, TicketCreateRequest ticketCreateRequest) : IRequest<bool>;

        public class TicketCreateCommandValidator : AbstractValidator<TicketCreateCommand>
        {
            public TicketCreateCommandValidator()
            {
                RuleFor(x => x.ticketCreateRequest)
                .SetValidator(new TicketCreateValidator());
                RuleFor(x => x.Id).NotEmpty().WithMessage("Enter event id");
            }
        }

        public class TicketCreateValidator : AbstractValidator<TicketCreateRequest>
        {
            public TicketCreateValidator()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("Enter a username")
                                        .EmailAddress().WithMessage("It must be an email");
                RuleFor(x => x.TypeError).NotEmpty().WithMessage("The error type must exist")
                                        .InclusiveBetween(1, 5).WithMessage("The error range is from 1 to 5");
                RuleFor(x => x.DetailError).NotEmpty().WithMessage("Enter the error details");
            }
        }

        public sealed class TicketCreateCommandHandler(
            IEventSourcingHandler<TicketAggregate> eventSourcingHandler
        ) : IRequestHandler<TicketCreateCommand, bool>
        {
            private readonly IEventSourcingHandler<TicketAggregate> _eventSourcingHandler = eventSourcingHandler;
            public async Task<bool> Handle(TicketCreateCommand request, CancellationToken cancellationToken)
            {
                var aggregate = new TicketAggregate(request);
                await _eventSourcingHandler.SaveAsync(aggregate, cancellationToken);
                return true;
            }
        }
    }
}
