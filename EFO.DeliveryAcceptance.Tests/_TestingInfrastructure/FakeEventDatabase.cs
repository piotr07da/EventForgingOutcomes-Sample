using EventForging;

namespace EFO.DeliveryAcceptance.Tests._TestingInfrastructure;

public class FakeEventDatabase : IEventDatabase
{
    private static readonly AsyncLocal<Dictionary<string, IEnumerable<object>>> _alreadySavedEvents = new();
    private static readonly AsyncLocal<Dictionary<string, IEnumerable<object>>> _newlySavedEvents = new();

    public Dictionary<string, IEnumerable<object>> AlreadySavedEvents => _alreadySavedEvents.Value ??= new Dictionary<string, IEnumerable<object>>();

    public Dictionary<string, IEnumerable<object>> NewlySavedEvents => _newlySavedEvents.Value ??= new Dictionary<string, IEnumerable<object>>();

    public Task<IEnumerable<object>> ReadAsync<TAggregate>(string aggregateId, CancellationToken cancellationToken = new())
    {
        return Task.FromResult<IEnumerable<object>>(AlreadySavedEvents.TryGetValue(aggregateId, out var asEvents) ? asEvents.ToArray() : Array.Empty<object>());
    }

    public Task WriteAsync<TAggregate>(string aggregateId, IReadOnlyList<object> events, AggregateVersion lastReadAggregateVersion, ExpectedVersion expectedVersion, Guid conversationId, Guid initiatorId, IDictionary<string, string> customProperties, CancellationToken cancellationToken = new())
    {
        long currentVersion;

        if (AlreadySavedEvents.TryGetValue(aggregateId, out var asEvents))
        {
            currentVersion = asEvents.Count() - 1;
        }
        else
        {
            currentVersion = -1;
        }

        if ((expectedVersion == ExpectedVersion.None && currentVersion != -1) || (expectedVersion != ExpectedVersion.Any && expectedVersion != currentVersion))
            throw new EventForgingUnexpectedVersionException(expectedVersion, lastReadAggregateVersion, currentVersion);

        NewlySavedEvents[aggregateId] = events.ToArray(); // makes copy o events
        return Task.CompletedTask;
    }

    public void Reset()
    {
        AlreadySavedEvents.Clear();
        NewlySavedEvents.Clear();
    }

    public void StubAlreadySavedEvents(IDictionary<string, IEnumerable<object>> events)
    {
        foreach (var (streamId, streamEvents) in events)
        {
            AlreadySavedEvents.Add(streamId, streamEvents);
        }
    }
}
