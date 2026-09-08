using AutoMapper;
using Common.Core.Events;
using static Ticketing.Command.Features.Tickets.TicketCreate;

namespace Ticketing.Command.Application.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TicketCreateRequest, TicketCreateCommand>();
             CreateMap<TicketCreateRequest, TicketCreatedEvent>();
        }
    }
}