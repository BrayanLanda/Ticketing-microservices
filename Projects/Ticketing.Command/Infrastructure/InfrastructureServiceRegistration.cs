using Common.Core.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Ticketing.Command.Domain.Abstracts;
using Ticketing.Command.Domain.EventModels;
using Ticketing.Command.Infrastructure.Repositories;

namespace Ticketing.Command.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            BsonClassMap.RegisterClassMap<BaseEvent>();
            BsonClassMap.RegisterClassMap<TicketCreatedEvent>();

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddTransient<IEventModelRepository, EventModelRepository>();
            services.AddSingleton<IMongoClient, MongoClient>(sp => 
                new MongoClient(configuration.GetConnectionString("MongoDb"))
            );

            return services;
        }
    }
}