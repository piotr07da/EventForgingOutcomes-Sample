using System.Reactive.Linq;
using EFO.WebUi.Components;
using EFO.WebUi.Components.ProductList;
using EFO.WebUi.Data;
using ReactiveUI;

namespace EFO.WebUi.Pages;

public class CatalogViewModel : ReactiveObject
{
    private readonly IProductCategoryService _productCategoryService;
    private readonly IProductService _productService;

    private Guid _categoryId;
    private string _categoryName;
    private IEnumerable<ProductCategoryItemViewModel> _parentCategories;
    private IEnumerable<ProductCategoryItemViewModel> _subcategories;
    private ProductListViewModel? _productListViewModel;

    public CatalogViewModel(
        IProductListViewModelFactory productListViewModelFactory,
        IProductService productService,
        IProductCategoryService productCategoryService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _productCategoryService = productCategoryService ?? throw new ArgumentNullException(nameof(productCategoryService));

        _categoryName = string.Empty;
        _parentCategories = Array.Empty<ProductCategoryItemViewModel>();
        _subcategories = Array.Empty<ProductCategoryItemViewModel>();

        var whenCategoryIdChanged = this
            .WhenAnyValue(x => x.CategoryId)
            .Where(x => x != Guid.Empty);

        whenCategoryIdChanged
            .SelectMany(GetCategoryAsync)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(category =>
            {
                CategoryName = category.Name;
                var parentCategories = new List<ProductCategoryItemViewModel>();
                var currentCategory = category;
                while (currentCategory.Parent != null)
                {
                    parentCategories.Add(new ProductCategoryItemViewModel(currentCategory.Parent));
                    currentCategory = currentCategory.Parent;
                }

                ParentCategories = parentCategories;
            });

        whenCategoryIdChanged
            .SelectMany(GetSubcategoriesAsync)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(subcategories =>
            {
                Subcategories = subcategories.OrderBy(sc => sc.Name).Select(sc => new ProductCategoryItemViewModel(sc));
            });

        whenCategoryIdChanged
            .SelectMany(GetProductsAsync)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(products =>
            {
                ProductListViewModel = productListViewModelFactory.Create(products);
            });
    }

    public Guid CategoryId
    {
        get => _categoryId;
        set => this.RaiseAndSetIfChanged(ref _categoryId, value);
    }

    public string CategoryName
    {
        get => _categoryName;
        set => this.RaiseAndSetIfChanged(ref _categoryName, value);
    }

    public IEnumerable<ProductCategoryItemViewModel> ParentCategories
    {
        get => _parentCategories;
        set => this.RaiseAndSetIfChanged(ref _parentCategories, value);
    }

    public IEnumerable<ProductCategoryItemViewModel> Subcategories
    {
        get => _subcategories;
        set => this.RaiseAndSetIfChanged(ref _subcategories, value);
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

    private async Task<ProductCategoryDto[]> GetSubcategoriesAsync(Guid parentCategoryId)
    {
        var subcategories = await _productCategoryService.GetCategoriesAsync(parentCategoryId);
        return subcategories.Categories;
    }

    private async Task<ProductDto[]> GetProductsAsync(Guid categoryId)
    {
        var products = await _productService.SearchProductsAsync(new SearchProductsDto(categoryId, new Dictionary<Guid, NumericPropertyFilter>(), new Dictionary<Guid, TextPropertyFilter>()));
        return products.Products;
    }
}
