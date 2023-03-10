using EFO.Catalog.Application.EventHandling;
using EFO.Catalog.Domain.Categories;
using EFO.Sales.Application.EventHandling;
using EFO.Sales.Domain.Orders;
using EFO.SharedReadModel.EventHandling;
using EventForging;
using EventForging.InMemory;
using EventForging.Serialization;

namespace EFO.WebApi.ServiceCollectionExtensions;

internal static class EventForgingServiceCollectionExtensions
{
    public static void AddAndConfigureEventForging(this IServiceCollection services)
    {
        services.AddEventForging(r =>
        {
            r.ConfigureEventForging(c =>
            {
                c.Serialization.SetEventTypeNameMappers(
                    new DefaultEventTypeNameMapper(typeof(CategoryAdded).Assembly), // Catalog
                    new DefaultEventTypeNameMapper(typeof(OrderStarted).Assembly) // Sales
                );
            });
            r.UseInMemory(c =>
            {
                c.SerializationEnabled = true;
                c.AddEventSubscription("MainPipeline");
                c.SetStreamNameFactory((at, aid) => $"{at.FullName}-{aid}"); // A full name is used because there is an aggregate named Product in both - Catalog and Sales projects.
            });

            r.AddEventHandlers(typeof(CatalogEventHandlers).Assembly);
            r.AddEventHandlers(typeof(SalesEventHandlers).Assembly);
            r.AddEventHandlers(typeof(ProductsReadModelBuildingEventHandlers).Assembly);
        });
    }
}
