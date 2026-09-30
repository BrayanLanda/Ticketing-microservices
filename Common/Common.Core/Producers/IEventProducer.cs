using Common.Core.Events;

namespace Common.Core.Producers;

public interface IEventProducer
{
    Task ProduceAsync(string topic, BaseEvent @event);
}
