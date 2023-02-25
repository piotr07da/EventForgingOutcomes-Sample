using EFO.Sales.Application.ReadModel.Products;
using EventForging.EventsHandling;

ling;

namespace EFO.Sales.Application.EventHandling;

internal class ProductsReadModelBuildingEventHandlers :
    IEventHandler<ProductIntroduced>,
    IEventHandler<ProductNamed>,
    IEventHandler<ProductPriced>
{
    private readonly IProductsReadModel _productsReadModel;

    public ProductsReadModelBuildingEventHandlers(IProductsReadModel productsReadModel)
    {
        _productsReadModel = productsReadModel ?? throw new ArgumentNullException(nameof(productsReadModel));
    }

    public string SubscriptionName => "ReadModelBuilder";

    public Task Handle(ProductIntroduced e, EventInfo ei, CancellationToken cancellationToken)
    {
        _productsReadModel.GetOrAdd(e.ProductId);
        return Task.CompletedTask;
    }

    public Task Handle(ProductNamed e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetOrAdd(e.ProductId);
        product.Name = e.Name;
        return Task.CompletedTask;
    }

    public Task Handle(ProductPriced e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetOrAdd(e.ProductId);
        var prices = product.Prices.ToList();
        pricesProductDto.PriceDto.Price(e.QuantityThreshold, e.UnitPrice));
        prices = prices.OrderBy(p => p.QuantityThreshold).ToList();
        product.Prices = prices.ToArray();
        return Task.CompletedTas

   }
}
