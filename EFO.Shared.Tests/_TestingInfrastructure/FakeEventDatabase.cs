using System.Runtime.CompilerServices;
using EventForging;

namespace EFO.Shared.Tests._TestingInfrastructure;

public class FakeEventDatabase : IEventDatabase
{
    private static readonly AsyncLocal<Dictionary<string, IEnumerable<object>>> _alreadySavedEvents = new();
    private static readonly AsyncLocal<Dictionary<string, IEnumerable<object>>> _newlySavedEvents = new();

    public Dictionary<string, IEnumerable<object>> AlreadySavedEvents => _alreadySavedEvents.Value ?? throw new NullReferenceException("Not initialized.");

    public Dictionary<string, IEnumerable<object>> NewlySavedEvents => _newlySavedEvents.Value ?? throw new NullReferenceException("Not initialized.");

    public async IAsyncEnumerable<object> ReadAsync<TAggregate>(string aggregateId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var events = AlreadySavedEvents.TryGetValue(aggregateId, out var asEvents) ? asEvents.ToArray() : Array.Empty<object>();
        foreach (var e in events)
        {
            yield return e;
        }

        await Task.CompletedTask;
    }

    public Task WriteAsync<TAggregate>(string aggregateId, IReadOnlyList<object> events, AggregateVersion lastReadAggregateVersion, ExpectedVersion expectedVersion, Guid conversationId, Guid initiatorId, IDictionary<string, string> customProperties, CancellationToken cancellationToken = default)
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
            throw new EventForgingUnexpectedVersionException(aggregateId, null, expectedVersion, lastReadAggregateVersion, currentVersion);

        NewlySavedEvents[aggregateId] = events.ToArray(); // makes copy o events
        return Task.CompletedTask;
    }

    public static void Initialize()
    {
        _alreadySavedEvents.Value = new Dictionary<string, IEnumerable<object>>();
        _newlySavedEvents.Value = new Dictionary<string, IEnumerable<object>>();
    }

    public void StubAlreadySavedEvents(IDictionary<string, IEnumerable<object>> events)
    {
        foreach (var (streamId, streamEvents) in events)
        {
            AlreadySavedEvents.Add(streamId, streamEvents);
        }
    }
}
