using Common.Core.Events;

namespace Ticketing.Command.Domain.Abstracts;

public abstract class AggregateRoot
{
    protected string _id = string.Empty;
    public string Id { get => _id; }
    public int Version { get; set; }
    private readonly List<BaseEvent> _changes = new();
    public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes.AsEnumerable();
    public void MarkChangesAsCommitted() => _changes.Clear();
    public void ApplyChange(BaseEvent @event, bool IsnewEvent)
    {
        var method = GetType().GetMethod("Apply", [@event.GetType()]);
        if(method is null)
        {
            throw new ArgumentException(nameof(method), $"Apply method is not found");
        }
        method.Invoke(this, [@event]);
        if(IsnewEvent) _changes.Add(@event);
    }
    public void RaiseEvent(BaseEvent @event)
    {
        ApplyChange(@event, true);
    }
}
