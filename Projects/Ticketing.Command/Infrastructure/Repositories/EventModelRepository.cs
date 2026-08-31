using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ticketing.Command.Application.Models;
using Ticketing.Command.Domain.EventModels;

namespace Ticketing.Command.Infrastructure.Repositories
{
    public class EventModelRepository : MongoRepository<EventModel>, IEventModelRepository
    {
        public EventModelRepository(IMongoClient mongoClient, IOptions<MongoSettings> options) : base(mongoClient, options)
        {
        }
    }
}