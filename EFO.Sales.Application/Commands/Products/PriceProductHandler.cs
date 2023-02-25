using EFO.Sales.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application.Commands.Products;

public sealed class PriceProductHandler : IConsumer<PriceProduct>
{
    private readonly IRepository<Product> _productRepository;

    public PriceProductHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<PriceProduct> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);

        product.Price(command.QuantityThreshold, command.UnitPrice);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
