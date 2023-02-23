using EventForging;

namespace EFO.Sales.Domain;

public class Product : IEventForged
{
    public Product()
    {
        Events = Events.CreateFor(this);
        Errors = new List<string>();
        Prices = new ProductPrices(this);
    }

    public Events Events { get; }
    public IList<string> Errors { get; }

    public ProductId Id { get; private set; }

    public ProductPrices Prices { get; }

    public static Product Introduce(ProductId productId, ProductName productName)
    {
        var product = new Product();
        var events = product.Events;
        events.Apply(new ProductIntroduced(productId));
        events.Apply(new ProductNamed(productId, productName));
        return product;
    }

    public void Price(Quantity quantityThreshold, Money unitPrice)
    {
        Prices.Add(quantityThreshold, unitPrice);
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    private void Apply(ProductIntroduced e)
    {
        Id = ProductId.Restore(e.ProductId);
    }

    private void Apply(ProductNamed e)
    {
    }

    private void Apply(ProductPriced e) => Prices.Apply(e);
}
