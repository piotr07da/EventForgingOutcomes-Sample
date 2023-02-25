using EventForging;

namespace EFO.Catalog.Application.Commands.Products;

public sealed class SetProductValuePropertyHandler : IConsumer<SetProductValueProperty>
{
    private readonly IRepository<Product> _productRepository;

    public SetProductValuePropertyHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<SetProductValueProperty> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);

        product.SetValueProperty(command.PropertyName, command.PropertyValue, command.PropertyUnit);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
