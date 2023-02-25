using EventForging;

namespace EFO.Sales.Domain.ProductProperties;

public class ProductTextProperty : IEventForged
{
    private ProductTextProperty()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public static ProductTextProperty Define(ProductPropertyId id, ProductPropertyName name)
    {
        var property = new ProductTextProperty();

        return property;
    }
}
