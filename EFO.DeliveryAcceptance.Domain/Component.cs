using EventForging;

namespace EFO.DeliveryAcceptance.Domain;

public class Component : IEventForged
{
    public Component()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public static Component RegisterArrival(ComponentId id, ComponentName name)
    {
        var component = new Component();
        var componentEvents = component.Events;
        componentEvents.Apply(new ComponentArrived(id.Value));
        componentEvents.Apply(new ComponentNamed(name.Value));
        return component;
    }

    public void CompleteInspection()
    {
        if (true)
        {
            Events.Apply(new object());
        }
    }
}

public class ArrivalInspectorCertified
{
}

public class ComponentClassified
{
    public ComponentClassified(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public class ComponentNamed
{
    public ComponentNamed(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public class ComponentArrived
{
    public ComponentArrived(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public struct ComponentId
{
    public Guid Value { get; }

    private ComponentId(Guid value)
    {
        Value = value;
    }
}

public struct ComponentName
{
    public string Value { get; }

    private ComponentName(string value)
    {
        Value = value;
    }
}
