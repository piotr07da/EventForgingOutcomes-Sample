using EFO.Catalog.Domain.ProductProperties;
using EFO.Catalog.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Products;

public sealed class SetProductTextPropertyHandler : IConsumer<SetProductTextProperty>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<TextProperty> _propertyRepository;

    public SetProductTextPropertyHandler(IRepository<Product> productRepository, IRepository<TextProperty> propertyRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
    }

    public async Task Consume(ConsumeContext<SetProductTextProperty> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);
        var property = await _propertyRepository.GetAsync(command.PropertyId, context);

        product.SetProperty(property, command.PropertyText);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
