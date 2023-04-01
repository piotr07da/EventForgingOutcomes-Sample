using EFO.WebUi.Data;

namespace EFO.WebUi.Components.ProductList;

public interface IProductListViewModelFactory
{
    ProductListViewModel Create(ProductDto[] products);
}
