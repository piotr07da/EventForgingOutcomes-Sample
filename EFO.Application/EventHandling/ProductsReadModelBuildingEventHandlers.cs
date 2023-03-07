using EventForging.EventsHandling;
using CatalogProductIntroduced = EFO.Catalog.Domain.Products.ProductIntroduced;
using SalesProductIntroduced = EFO.Sales.Domain.Products.ProductIntroduced;

namespace EFO.Application.EventHandling;

public sealed class ProductsReadModelBuildingEventHandlers :
    IEventHandler<CatalogProductIntroduced>,
    IEventHandler<SalesProductIntroduced>
{
    public string SubscriptionName => "ReadModelBuilder";

    public async Task HandleAsync(CatalogProductIntroduced e, EventInfo ei, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task HandleAsync(SalesProductIntroduced e, EventInfo ei, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
