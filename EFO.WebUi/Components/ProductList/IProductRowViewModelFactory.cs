using EFO.WebUi.Data;

namespace EFO.WebUi.Components.ProductList;

public interface IProductRowViewModelFactory
{
    ProductRowViewModel Create(ProductDto product);
}
