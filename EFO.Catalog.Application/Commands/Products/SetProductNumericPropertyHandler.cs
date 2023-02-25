using EFO.Catalog.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Products;

public sealed class SetProductNumericPropertyHandler : IConsumer<SetProductNumericProperty>
{
    private readonly IRepository<Product> _productRepository;

    public SetProductNumericPropertyHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<SetProductNumericProperty> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);

        product.SetProperty(command.PropertyId, command.PropertyValue);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
