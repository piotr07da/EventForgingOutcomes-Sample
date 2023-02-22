using EFO.Sales.Application.EventHandling;
using EFO.Sales.Domain;
using EventForging;
using EventForging.InMemory;
using EventForging.Serialization;
using MassTransit;

namespace EFO.Sales.WebApi.ServiceCollectionExtensions;

internal static class InMemoryServiceCollectionExtensions
{
    public static void AddInMemoryEventForging(this IServiceCollection services)
    {
        services.AddEventForging(r =>
        {
            r.ConfigureEventForging(c =>
            {
                c.Serialization.SetEventTypeNameMappers(new DefaultEventTypeNameMapper(typeof(OrderStarted).Assembly));
            });
            r.UseInMemory(c =>
            {
                c.SerializationEnabled = true;
                c.AddEventSubscription("MainPipeline");
            });
            r.AddEventHandlers(typeof(EventHandlers).Assembly);
        });
    }

    public static void AddInMemoryMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(r =>
        {
            r.UsingInMemory((rc, c) =>
            {
            });
        });
    }
}
