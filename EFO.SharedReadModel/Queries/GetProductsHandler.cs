using EFO.SharedReadModel.ReadModel.Products;
using MassTransit;

namespace EFO.SharedReadModel.Queries;

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
        var products = _readModel.GetProducts(
            query.CategoryId,
            query.NumericPropertiesFilters.ToDictionary(e => e.Key, e => (e.Value.MinValue, e.Value.MaxValue)),
            query.TextPropertiesFilters.ToDictionary(e => e.Key, e => e.Value.Values));
        await context.RespondAsync(products);
    }
}
