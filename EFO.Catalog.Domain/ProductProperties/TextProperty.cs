using EventForging;

namespace EFO.Catalog.Domain.ProductProperties;

public class TextProperty : IEventForged
{
    private TextProperty()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public static TextProperty Define(PropertyId id, PropertyName name)
    {
        var property = new TextProperty();

        return property;
    }
}
