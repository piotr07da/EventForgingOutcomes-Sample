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
        var conversationId = context.ConversationId ?? Guid.Empty;
        var initiatorId = context.CorrelationId ?? context.MessageId ?? Guid.Empty;
        await repository.SaveAsync(aggregateId, aggregate, ExpectedVersion.Any, conversationId, initiatorId, null, context.CancellationToken);
    }
}
