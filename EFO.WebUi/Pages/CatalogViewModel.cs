using System.Reactive.Linq;
using EFO.WebUi.Components;
using EFO.WebUi.Components.ProductList;
using EFO.WebUi.Data;
using ReactiveUI;

namespace EFO.WebUi.Pages;

public class CatalogViewModel : ReactiveObject
{
    private readonly IProductListViewModelFactory _productListViewModelFactory;
    private readonly IProductService _productService;
    private readonly IProductCategoryService _productCategoryService;
    private Guid _categoryId;
    private string? _categoryName;
    private ProductListViewModel? _productListViewModel;

    public CatalogViewModel(
        IProductListViewModelFactory productListViewModelFactory,
        IProductService productService,
        IProductCategoryService productCategoryService)
    {
        _productListViewModelFactory = productListViewModelFactory;
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _productCategoryService = productCategoryService ?? throw new ArgumentNullException(nameof(productCategoryService));

        this
            .WhenAnyValue(x => x.CategoryId)
            .Where(x => x != Guid.Empty)
            .SelectMany(GetCategoryAsync)
            .Subscribe(category =>
            {
                CategoryName = category.Name;
            });

        this
            .WhenAnyValue(x => x.CategoryId)
            .Where(x => x != Guid.Empty)
            .SelectMany(GetProductsAsync)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(products =>
            {
                ProductListViewModel = _productListViewModelFactory.Create(products);
            });
    }

    public Guid CategoryId
    {
        get => _categoryId;
        set => this.RaiseAndSetIfChanged(ref _categoryId, value);
    }

    public string? CategoryName
    {
        get => _categoryName;
        set => this.RaiseAndSetIfChanged(ref _categoryName, value);
    }

    public ProductListViewModel? ProductListViewModel
    {
        get => _productListViewModel;
        set => this.RaiseAndSetIfChanged(ref _productListViewModel, value);
    }

    private async Task<ProductCategoryDto> GetCategoryAsync(Guid categoryId)
    {
        return await _productCategoryService.GetCategoryAsync(categoryId);
    }

    private async Task<ProductDto[]> GetProductsAsync(Guid categoryId)
    {
        var products = await _productService.SearchProductsAsync(new SearchProductsDto(categoryId, new Dictionary<Guid, NumericPropertyFilter>(), new Dictionary<Guid, TextPropertyFilter>()));
        return products.Products;
    }
}
