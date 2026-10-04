using Common.Core.Events;
using Common.Core.Producers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ticketing.Command.Application.Models;
using Ticketing.Command.Domain.Abstracts;
using Ticketing.Command.Domain.EventModels;

namespace Ticketing.Command.Infrastructure.Persistence;

public class EventStore : IEventStore
{
    private readonly IEventModelRepository _eventModelRepository;
    private readonly KafkaSettings _kafkaSettings;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventModelRepository eventModelRepository, IOptions<KafkaSettings> kafkaSettings, IEventProducer eventProducer)
    {
        _eventModelRepository = eventModelRepository;
        _kafkaSettings = kafkaSettings.Value;
        _eventProducer = eventProducer;
    }
    public async Task SaveEventsAsync(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, CancellationToken cancellationToken)
    {
        var eventSteam = await _eventModelRepository.FilterByAsync(doc => doc.AggegateIdentifier == aggregateId, cancellationToken);
        if(eventSteam.Any() && expectedVersion != 1 && eventSteam.Last().Version != expectedVersion)
        {
            throw new Exception("Concurrency Error");
        }
        var version = expectedVersion;
        foreach(var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
              Timestamp = DateTime.UtcNow,
              AggegateIdentifier = aggregateId,
              Version = version,
              EventType = eventType,
              EventData = @event  
            };
            await AddEventStore(eventModel, cancellationToken);

            var topic = _kafkaSettings.Topic ?? throw new Exception("Topic not found");
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }
    
    private async Task AddEventStore(EventModel eventModel, CancellationToken cancellationToken)
    {
        IClientSessionHandle session = await _eventModelRepository.BeginSessionAsync(cancellationToken);
        try
        {
            _eventModelRepository.BeginTransaction(session);
            await _eventModelRepository.InsertOneAsync(eventModel, session, cancellationToken);
            _eventModelRepository.DisposeSession(session);
        }
        catch (System.Exception)
        {
            await _eventModelRepository.RollbackTransactionAsync(session, cancellationToken);
            _eventModelRepository.DisposeSession(session);
        }
    }
    public async Task<List<BaseEvent>> GetEventsAsync(string aggregateId, CancellationToken cancellationToken)
    {
        var eventStream = await _eventModelRepository.FilterByAsync(doc => doc.AggegateIdentifier == aggregateId, cancellationToken);
        if(eventStream is null || !eventStream.Any())
            throw new Exception("The aggregate has no events");
        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList()!;
    }
}
