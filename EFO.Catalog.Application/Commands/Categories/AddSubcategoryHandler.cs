using EFO.Catalog.Domain.Categories;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Categories;

public sealed class AddSubcategoryHandler : IConsumer<AddSubcategory>
{
    private readonly IRepository<Category> _categoryRepository;

    public AddSubcategoryHandler(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task Consume(ConsumeContext<AddSubcategory> context)
    {
        var command = context.Message;

        var category = await _categoryRepository.GetAsync(command.CategoryId);

        var subcategory = category.AddSubcategory(command.SubcategoryId, command.SubcategoryName);

        await _categoryRepository.SaveAsync(command.SubcategoryId, subcategory, context);
    }
}
