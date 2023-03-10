namespace EFO.WebUi.Data;

public interface IProductCategoriesService
{
    Task<ProductCategoryDto[]> GetCategoriesAsync(Guid? parentCategoryId);
}
