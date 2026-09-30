using Common.Core.Events;
using Common.Core.Producers;
using Ticketing.Command.Application.Models;

namespace Ticketing.Command.Infrastructure.Persistence;

public class TicketEventProducer : IEventProducer
{
    private readonly KafkaSettings _kafkaSettings;

    public TicketEventProducer(KafkaSettings kafkaSettings)
    {
        _kafkaSettings = kafkaSettings;
    }

    public Task ProduceAsync(string topic, BaseEvent @event)
    {
        throw new NotImplementedException();
    }
}
