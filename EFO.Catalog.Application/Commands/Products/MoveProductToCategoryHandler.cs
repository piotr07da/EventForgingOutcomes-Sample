using EFO.Catalog.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Products;

public sealed class MoveProductToCategoryHandler : IConsumer<MoveProductToCategory>
{
    private readonly IRepository<Product> _productRepository;

    public MoveProductToCategoryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<MoveProductToCategory> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);

        product.MoveToCategory(command.CategoryId);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
