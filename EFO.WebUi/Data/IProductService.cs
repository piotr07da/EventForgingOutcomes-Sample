using Refit;

namespace EFO.WebUi.Data;

public interface IProductService
{
    [Post("/products/search")]
    Task<ProductsDto> SearchProductsAsync(SearchProductsDto search);
}
