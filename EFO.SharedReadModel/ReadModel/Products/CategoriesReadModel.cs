namespace EFO.SharedReadModel.ReadModel.Products;

internal sealed class CategoriesReadModel : ICategoriesReadModel
{
    private readonly IDictionary<Guid, ProductCategoryDto> _entries = new Dictionary<Guid, ProductCategoryDto>();

    public void AddCategory(Guid categoryId)
    {
        var category = new ProductCategoryDto(categoryId);
        _entries.Add(categoryId, category);
    }

    public ProductCategoryDto GetCategory(Guid categoryId)
    {
        return _entries[categoryId];
    }

    public ProductCategoryDto[] GetCategories(Guid? parentCategoryId)
    {
        return _entries.Values.Where(c => c.ParentId == parentCategoryId).ToArray();
    }
}
