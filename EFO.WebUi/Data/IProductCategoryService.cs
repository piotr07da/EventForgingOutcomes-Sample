using Refit;

namespace EFO.WebUi.Data;

public interface IProductCategoryService
{
    [Get("/product-categories?parentCategoryId={parentCategoryId}")]
    Task<ProductCategoriesDto> GetCategoriesAsync(Guid? parentCategoryId);

    [Get("/product-categories/{categoryId}")]
    Task<ProductCategoryDto> GetCategoryAsync(Guid categoryId);
}
