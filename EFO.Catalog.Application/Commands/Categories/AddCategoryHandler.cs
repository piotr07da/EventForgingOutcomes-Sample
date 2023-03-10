using EFO.Catalog.Domain.Categories;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Categories;

public sealed class AddCategoryHandler : IConsumer<AddCategory>
{
    private readonly IRepository<Category> _categoryRepository;

    public AddCategoryHandler(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task Consume(ConsumeContext<AddCategory> context)
    {
        var command = context.Message;

        var category = Category.Add(command.CategoryId, command.CategoryName);

        await _categoryRepository.SaveAsync(command.CategoryId, category, context);
    }
}
