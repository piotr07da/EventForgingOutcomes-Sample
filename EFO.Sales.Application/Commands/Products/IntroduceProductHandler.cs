using EFO.Sales.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application.Commands.Products;

public sealed class IntroduceProductHandler : IConsumer<IntroduceProduct>
{
    private readonly IRepository<Product> _productRepository;

    public IntroduceProductHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<IntroduceProduct> context)
    {
        var command = context.Message;

        var product = Product.Introduce(command.ProductId, command.ProductName);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
