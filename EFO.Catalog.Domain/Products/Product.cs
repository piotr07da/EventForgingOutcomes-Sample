using EFO.Catalog.Domain.Categories;
using EFO.Catalog.Domain.ProductProperties;
using EFO.Shared.Domain;
using EventForging;

namespace EFO.Catalog.Domain.Products;

public class Product : IEventForged
{
    public Product()
    {
        Events = Events.CreateFor(this);
        Errors = new List<string>();
    }

    public Events Events { get; }
    public IList<string> Errors { get; }

    public ProductId Id { get; private set; }


    public static Product Introduce(ProductId productId, ProductName productName)
    {
        var product = new Product();
        var events = product.Events;
        events.Apply(new ProductIntroduced(productId));
        events.Apply(new ProductNamed(productId, productName));
        return product;
    }

    public void SetProperty(NumericProperty property, NumericPropertyValue propertyValue)
    {
        Events.Apply(new ProductNumericPropertySet(Id, property.Id, propertyValue));
    }

    public void SetProperty(TextProperty property, TextPropertyValue propertyValue)
    {
        Events.Apply(new ProductTextPropertySet(Id, property.Id, propertyValue));
    }

    public void MoveToCategory(CategoryId categoryId)
    {
        Events.Apply(new ProductMovedToCategory(Id, categoryId));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    private void Apply(ProductIntroduced e)
    {
        Id = ProductId.Restore(e.ProductId);
    }

    private void Apply(ProductNamed e)
    {
    }

    private void Apply(ProductNumericPropertySet e)
    {
    }

    private void Apply(ProductTextPropertySet e)
    {
    }

    private void Apply(ProductMovedToCategory e)
    {
    }
}
