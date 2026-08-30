using Ticketing.Command.Domain.Abstracts;

namespace Ticketing.Command.Domain.EventModels
{
    public interface IEventModelRepository : IMongoRepository<EventModel>
    {
        
    }
}