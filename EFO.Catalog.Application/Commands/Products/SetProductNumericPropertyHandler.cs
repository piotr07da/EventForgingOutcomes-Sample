using EFO.Catalog.Domain.ProductProperties;
using EFO.Catalog.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Products;

public sealed class SetProductNumericPropertyHandler : IConsumer<SetProductNumericProperty>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<NumericProperty> _propertyRepository;

    public SetProductNumericPropertyHandler(IRepository<Product> productRepository, IRepository<NumericProperty> propertyRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
    }

    public async Task Consume(ConsumeContext<SetProductNumericProperty> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);
        var property = await _propertyRepository.GetAsync(command.PropertyId, context);

        product.SetProperty(property, command.PropertyValue);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
