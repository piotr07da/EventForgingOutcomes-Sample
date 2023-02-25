using EventForging;

namespace EFO.Sales.Domain.ProductProperties;

public class ProductValueProperty : IEventForged
{
    private ProductValueProperty()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public static ProductValueProperty Define(ProductPropertyId id, ProductPropertyName name, ProductPropertyUnit unit)
    {
        var property = new ProductValueProperty();

        return property;
    }
}
