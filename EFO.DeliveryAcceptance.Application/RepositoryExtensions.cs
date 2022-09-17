using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public static class RepositoryExtensions
{
    public static async Task SaveAsync<TAggregate>(this IRepository<TAggregate> repository, Guid aggregateId, TAggregate aggregate, ExpectedVersion expectedVersion, ConsumeContext consumeContext)
    {
        await repository.SaveAsync(aggregateId, aggregate, expectedVersion, consumeContext.ConversationId ?? Guid.Empty, consumeContext.InitiatorId ?? Guid.Empty, null);
    }
}
