using EventForging;

namespace EFO.DeliveryAcceptance.Domain;

public class ComponentInspector : IEventForged
{
    public ComponentInspector()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public ComponentInspectorId Id { get; private set; }
    public ComponentClass AllowedComponentClass { get; private set; }

    public static ComponentInspector Hire(ComponentInspectorId id)
    {
        var inspector = new ComponentInspector();
        var componentEvents = inspector.Events;
        componentEvents.Apply(new ComponentInspectorHired(id.Value));
        componentEvents.Apply(new ComponentInspectorCertified(id.Value, ComponentClass.Standard));
        return inspector;
    }

    private void Apply(ComponentInspectorHired e)
    {
        Id = ComponentInspectorId.Restore(e.Id);
    }

    private void Apply(ComponentInspectorCertified e)
    {
        AllowedComponentClass = e.ComponentClass;
    }
}
