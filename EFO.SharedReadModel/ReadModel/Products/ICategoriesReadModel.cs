namespace EFO.SharedReadModel.ReadModel.Products;

public interface ICategoriesReadModel
{
    void AddCategory(Guid categoryId);

    ProductCategoryDto GetCategory(Guid categoryId);

    ProductCategoryDto[] GetCategories(Guid? parentCategoryId);
}
