using FluentValidation;
using MediatR;

namespace Ticketing.Command.Features.Tickets
{
    public class TicketCreate
    {
        public sealed class TicketCreateRequest(
            string username, string typeError, string detailError
        )
        {
            public string Username { get; set; } = username;
            public string TypeError { get; set; } = typeError;
            public string DetailError { get; set; } = detailError;
        }

        public record TicketCreateCommand(TicketCreateRequest ticketCreateRequest) : IRequest<bool>;

        public class TicketCreateCommandValidator : AbstractValidator<TicketCreateCommand>
        {
            public TicketCreateCommandValidator()
            {
                RuleFor(x => x.ticketCreateRequest)
                .SetValidator(new TicketCreateValidator());
            }
        }

        public class TicketCreateValidator : AbstractValidator<TicketCreateRequest>
        {
            public TicketCreateValidator()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("Enter a username");
                RuleFor(x => x.DetailError).NotEmpty().WithMessage("Enter the error details");
            }
        }

        public sealed class TicketCreateCommandHandler : IRequestHandler<TicketCreateCommand, bool>
        {
            public Task<bool> Handle(TicketCreateCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}