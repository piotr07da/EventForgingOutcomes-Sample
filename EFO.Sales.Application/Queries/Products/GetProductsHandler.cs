using EFO.Sales.Application.ReadModel.Products;
using MassTransit;

namespace EFO.Sales.Application.Queries.Products;

public sealed class GetProductsHandler : IConsumer<GetProducts>
{
    private readonly IProductsReadModel _readModel;

    public GetProductsHandler(IProductsReadModel readModel)
    {
        _readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
    }

    public async Task Consume(ConsumeContext<GetProducts> context)
    {
        var query = context.Message;
        var products = _readModel.GetProducts();
        await context.RespondAsync(products);
    }
}
