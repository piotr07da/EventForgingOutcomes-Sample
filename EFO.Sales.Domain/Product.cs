using EventForging;

namespace EFO.Sales.Domain;

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

    public ProductPrices Prices { get; } = new();

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    private void Apply(ProductIntroduced e)
    {
        Id = ProductId.Restore(e.ProductId);
    }

    private void Apply(ProductPriced e) => Prices.Apply(e);
}
