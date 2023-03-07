using EventForging;

namespace EFO.Catalog.Domain.ProductProperties;

public class NumericProperty : IEventForged
{
    public NumericProperty()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public PropertyId Id { get; private set; }

    public static NumericProperty Define(PropertyId id, PropertyName name, ProductPropertyUnit unit)
    {
        var property = new NumericProperty();

        return property;
    }

    private void Apply(NumericPropertyDefined e)
    {
        Id = e.PropertyId;
    }
}
