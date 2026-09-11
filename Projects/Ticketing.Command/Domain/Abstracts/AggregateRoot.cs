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
}
