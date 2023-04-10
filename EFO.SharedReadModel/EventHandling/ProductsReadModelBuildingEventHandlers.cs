using EFO.Catalog.Domain.Categories;
using EFO.Catalog.Domain.ProductProperties;
using EFO.Catalog.Domain.Products;
using EFO.Sales.Domain.Products;
using EFO.SharedReadModel.ReadModel.Products;
using EventForging.EventsHandling;
using CatalogProductIntroduced = EFO.Catalog.Domain.Products.ProductIntroduced;
using CatalogProductNamed = EFO.Catalog.Domain.Products.ProductNamed;
using SalesProductIntroduced = EFO.Sales.Domain.Products.ProductIntroduced;

namespace EFO.SharedReadModel.EventHandling;

public sealed class ProductsReadModelBuildingEventHandlers :
    IEventHandler<CategoryAdded>,
    IEventHandler<CategoryNamed>,
    IEventHandler<CategoryAttachedToParent>,
    IEventHandler<NumericPropertyDefined>,
    IEventHandler<TextPropertyDefined>,
    IEventHandler<CatalogProductIntroduced>,
    IEventHandler<SalesProductIntroduced>,
    IEventHandler<CatalogProductNamed>,
    IEventHandler<ProductMovedToCategory>,
    IEventHandler<ProductNumericPropertySet>,
    IEventHandler<ProductTextPropertySet>,
    IEventHandler<ProductPriced>
{
    private readonly IProductsReadModel _productsReadModel;
    private readonly ICategoriesReadModel _categoriesReadModel;
    private readonly IPropertiesReadModel _propertiesReadModel;

    public ProductsReadModelBuildingEventHandlers(
        IProductsReadModel productsReadModel,
        ICategoriesReadModel categoriesReadModel,
        IPropertiesReadModel propertiesReadModel)
    {
        _productsReadModel = productsReadModel ?? throw new ArgumentNullException(nameof(productsReadModel));
        _categoriesReadModel = categoriesReadModel ?? throw new ArgumentNullException(nameof(categoriesReadModel));
        _propertiesReadModel = propertiesReadModel ?? throw new ArgumentNullException(nameof(propertiesReadModel));
    }

    public string SubscriptionName => "ReadModelBuilder";

    public Task HandleAsync(CategoryAdded e, EventInfo ei, CancellationToken cancellationToken)
    {
        _categoriesReadModel.AddCategory(e.CategoryId);
        return Task.CompletedTask;
    }

    public Task HandleAsync(CategoryNamed e, EventInfo ei, CancellationToken cancellationToken)
    {
        var cat = _categoriesReadModel.GetCategory(e.CategoryId);
        cat.Name = e.Name;
        return Task.CompletedTask;
    }

    public Task HandleAsync(CategoryAttachedToParent e, EventInfo ei, CancellationToken cancellationToken)
    {
        var cat = _categoriesReadModel.GetCategory(e.CategoryId);
        cat.ParentId = e.ParentCategoryId;
        cat.Parent = _categoriesReadModel.GetCategory(e.ParentCategoryId);
        return Task.CompletedTask;
    }

    public Task HandleAsync(NumericPropertyDefined e, EventInfo ei, CancellationToken cancellationToken)
    {
        _propertiesReadModel.AddNumericProperty(e.PropertyId, e.PropertyName, e.PropertyUnit);
        return Task.CompletedTask;
    }

    public Task HandleAsync(TextPropertyDefined e, EventInfo ei, CancellationToken cancellationToken)
    {
        _propertiesReadModel.AddTextProperty(e.PropertyId, e.PropertyName);
        return Task.CompletedTask;
    }

    public Task HandleAsync(CatalogProductIntroduced e, EventInfo ei, CancellationToken cancellationToken)
    {
        _productsReadModel.TryAddProduct(e.ProductId);
        return Task.CompletedTask;
    }

    public Task HandleAsync(SalesProductIntroduced e, EventInfo ei, CancellationToken cancellationToken)
    {
        _productsReadModel.TryAddProduct(e.ProductId);
        return Task.CompletedTask;
    }

    public Task HandleAsync(CatalogProductNamed e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetProduct(e.ProductId);
        product.Name = e.Name;
        return Task.CompletedTask;
    }

    public Task HandleAsync(ProductMovedToCategory e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetProduct(e.ProductId);
        var category = _categoriesReadModel.GetCategory(e.CategoryId);
        var newCategoriesBranch = new List<ProductCategoryDto> { category, };
        while (category.ParentId != null)
        {
            category = _categoriesReadModel.GetCategory(category.ParentId.Value);
            newCategoriesBranch.Add(category);
        }

        var oldCategoriesBranch = product.Categories;
        product.Categories = newCategoriesBranch.ToArray();

        foreach (var c in oldCategoriesBranch)
        {
            --c.NumberOfProducts;
        }

        foreach (var c in newCategoriesBranch)
        {
            ++c.NumberOfProducts;
        }

        return Task.CompletedTask;
    }

    public Task HandleAsync(ProductNumericPropertySet e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetProduct(e.ProductId);
        var property = _propertiesReadModel.GetNumericProperty(e.PropertyId);
        product.NumericProperties = new List<ProductNumericPropertyWithValueDto>(product.NumericProperties) { new(property.Id, property.Name, property.Unit, e.PropertyValue), }.ToArray();
        return Task.CompletedTask;
    }

    public Task HandleAsync(ProductTextPropertySet e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetProduct(e.ProductId);
        var property = _propertiesReadModel.GetTextProperty(e.PropertyId);
        product.TextProperties = new List<ProductTextPropertyWithValueDto>(product.TextProperties) { new(property.Id, property.Name, e.PropertyValue), }.ToArray();
        return Task.CompletedTask;
    }

    public Task HandleAsync(ProductPriced e, EventInfo ei, CancellationToken cancellationToken)
    {
        var product = _productsReadModel.GetProduct(e.ProductId);
        var prices = product.Prices.ToList();
        prices.Add(new ProductDto.Price(e.QuantityThreshold, e.UnitPrice));
        prices = prices.OrderBy(p => p.QuantityThreshold).ToList();
        product.Prices = prices.ToArray();
        return Task.CompletedTask;
    }
}
