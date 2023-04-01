using EFO.SharedReadModel.ReadModel.Products;
using MassTransit;

namespace EFO.SharedReadModel.Queries;

public class GetCategoryHandler : IConsumer<GetCategory>
{
    private readonly ICategoriesReadModel _categoriesReadModel;

    public GetCategoryHandler(ICategoriesReadModel categoriesReadModel)
    {
        _categoriesReadModel = categoriesReadModel ?? throw new ArgumentNullException(nameof(categoriesReadModel));
    }

    public async Task Consume(ConsumeContext<GetCategory> context)
    {
        var category = _categoriesReadModel.GetCategory(context.Message.CategoryId);
        await context.RespondAsync(category);
    }
}
