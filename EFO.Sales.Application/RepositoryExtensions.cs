using EventForging;
using MassTransit;

namespace EFO.Sales.Application;

public static class RepositoryExtensions
{
    public static async Task<TAggregate> GetAsync<TAggregate, TMessage>(this IRepository<TAggregate> repository, Guid aggregateId, ConsumeContext<TMessage> context)
        where TMessage : class
    {
        return await repository.GetAsync(aggregateId, context.CancellationToken);
    }

    public static async Task SaveAsync<TAggregate, TMessage>(this IRepository<TAggregate> repository, Guid aggregateId, TAggregate aggregate, ConsumeContext<TMessage> context)
        where TMessage : class
    {
        ExpectedVersion expectedVersion;

        if (context.Headers.TryGetHeader("Expected-Version", out var ev) && ev != null)
        {
            var evStr = ev.ToString();
            expectedVersion = evStr!.ToLower() switch
            {
                "any" => ExpectedVersion.Any,
                "none" => ExpectedVersion.None,
                _ => int.Parse(evStr),
            };
        }
        else
        {
            expectedVersion = ExpectedVersion.Any;
        }

        var conversationId = context.ConversationId ?? Guid.Empty;
        var initiatorId = context.CorrelationId ?? context.MessageId ?? Guid.Empty;

        var customProperties = new Dictionary<string, string>();
        foreach (var header in context.Headers)
        {
            if (header.Key.StartsWith("Custom-Property-"))
            {
                var key = header.Key["Custom-Property-".Length..];
                customProperties.Add(key, header.Value.ToString() ?? string.Empty);
            }
        }

        await repository.SaveAsync(aggregateId, aggregate, expectedVersion, conversationId, initiatorId, customProperties, context.CancellationToken);
    }
}
