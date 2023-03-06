using EFO.Catalog.Domain.Categories;
using EFO.Sales.Application.EventHandling;
using EFO.Sales.Domain.Orders;
using EventForging;
using EventForging.InMemory;
using EventForging.Serialization;
using MassTransit;

namespace EFO.WebApi.ServiceCollectionExtensions;

internal static class InMemoryServiceCollectionExtensions
{
    public static void AddInMemoryEventForging(this IServiceCollection services)
    {
        services.AddEventForging(r =>
        {
            r.ConfigureEventForging(c =>
            {
                c.Serialization.SetEventTypeNameMappers(
                    new DefaultEventTypeNameMapper(typeof(OrderStarted).Assembly), // Sales
                    new DefaultEventTypeNameMapper(typeof(CategoryCreated).Assembly) // Catalog
                );
            });
            r.UseInMemory(c =>
            {
                c.SerializationEnabled = true;
                c.AddEventSubscription("MainPipeline");
            });
            r.AddEventHandlers(typeof(SalesEventHandlers).Assembly);
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
