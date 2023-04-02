using EFO.WebUi.Data;
using ReactiveUI;

namespace EFO.WebUi.Components.ProductList;

public class ProductListViewModel : ReactiveObject
{
    private IEnumerable<ProductRowViewModel>? _products;

    public ProductListViewModel(ProductDto[] products, IProductRowViewModelFactory productRowViewModelFactory)
    {
        Products = products.Select(productRowViewModelFactory.Create).ToArray();
    }

    public IEnumerable<ProductRowViewModel> Products
    {
        get => _products!;
        set => this.RaiseAndSetIfChanged(ref _products, value);
    }
}