using EventForging;

namespace EFO.Catalog.Domain.ProductProperties;

public class NumericProperty : IEventForged
{
    private NumericProperty()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public static NumericProperty Define(PropertyId id, PropertyName name, ProductPropertyUnit unit)
    {
        var property = new NumericProperty();

        return property;
    }
}
