using EventForging;

namespace EFO.Catalog.Domain.ProductProperties;

public class TextProperty : IEventForged
{
    public TextProperty()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public PropertyId Id { get; private set; }

    public static TextProperty Define(PropertyId id, PropertyName name)
    {
        var property = new TextProperty();

        return property;
    }

    private void Apply(TextPropertyDefined e)
    {
        Id = e.PropertyId;
    }
}
