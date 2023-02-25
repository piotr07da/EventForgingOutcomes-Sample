using EventForging;
using MassTransit;

namespace EFO.Sales.Application;

public static class RepositoryExtensions
{
    public static async Task<TAggregate> GetAsync<TAggregate, TMessage>(this IRepository<TAggregate> repository, Guid aggregateId, ConsumeContext<TMessage> context)
        where TMessage : class
    {
        return await Shared.Application.RepositoryExtensions.GetAsync(repository, aggregateId, context);
    }

    public static async Task SaveAsync<TAggregate, TMessage>(this IRepository<TAggregate> repository, Guid aggregateId, TAggregate aggregate, ConsumeContext<TMessage> context)
        where TMessage : class
    {
        await Shared.Application.RepositoryExtensions.SaveAsync(repository, aggregateId, aggregate, context);
    }
}
