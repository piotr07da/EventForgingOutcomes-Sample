using EFO.SharedReadModel.ReadModel.Products;
using MassTransit;

namespace EFO.SharedReadModel.Queries;

public class GetCategoriesHandler : IConsumer<GetCategories>
{
    private readonly ICategoriesReadModel _categoriesReadModel;

    public GetCategoriesHandler(ICategoriesReadModel categoriesReadModel)
    {
        _categoriesReadModel = categoriesReadModel ?? throw new ArgumentNullException(nameof(categoriesReadModel));
    }

    public async Task Consume(ConsumeContext<GetCategories> context)
    {
        var categories = _categoriesReadModel.GetCategories(context.Message.ParentCategoryId);
        await context.RespondAsync(new ProductCategoriesDto(categories));
    }
}


