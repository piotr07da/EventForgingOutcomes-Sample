using EFO.WebUi.Data;

namespace EFO.WebUi.Components.ProductList;

public sealed class ProductListViewModelFactory : IProductListViewModelFactory
{
    private readonly IProductRowViewModelFactory _productRowViewModelFactory;

    public ProductListViewModelFactory(IProductRowViewModelFactory productRowViewModelFactory)
    {
        _productRowViewModelFactory = productRowViewModelFactory;
    }

    public ProductListViewModel Create(ProductDto[] products)
    {
        return new ProductListViewModel(products, _productRowViewModelFactory);
    }
}
