using static EFO.WebUi.Data.ProductService;

namespace EFO.WebUi.Data;

public interface IProductService
{
    Task<ProductDto[]> GetProductsAsync(Guid categoryId, IDictionary<Guid, NumericPropertyFilter> numericPropertiesFilters, IDictionary<Guid, TextPropertyFilter> textPropertiesFilters);
}
