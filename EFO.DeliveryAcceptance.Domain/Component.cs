using EventForging;

namespace EFO.DeliveryAcceptance.Domain;

public class Component : IEventForged
{
    private bool _measured;
    private bool _weighed;

    public Component()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public ComponentId Id { get; private set; }
    public ComponentClass Class { get; private set; }

    public static Component RegisterArrival(ComponentId id, ComponentName name)
    {
        var component = new Component();
        var componentEvents = component.Events;
        componentEvents.Apply(new ComponentArrived(id.Value));
        componentEvents.Apply(new ComponentNamed(id.Value, name.Value));
        return component;
    }

    public void CompleteInspection(ComponentInspector componentInspector)
    {
        var errors = new List<string>();

        if (componentInspector.AllowedComponentClass < Class)
        {
            errors.Add(DomainErrors.ComponentInspectorDoesNotHaveRequiredCertification);
        }

        if (!_measured)
        {
            errors.Add(DomainErrors.ComponentNotMeasured);
        }

        if (!_weighed)
        {
            errors.Add(DomainErrors.ComponentNotWeighed);
        }

        DomainException.ThrowIfErrors(errors);

        Events.Apply(new ComponentInspectionCompleted(Id.Value));
    }

    private void Apply(ComponentArrived e)
    {
        Id = ComponentId.Restore(e.Id);
    }

    private void Apply(ComponentNamed e)
    {
    }

    private void Apply(ComponentClassified e)
    {
        Class = e.Class;
    }
}

public struct ComponentId
{
    public Guid Value { get; }

    private ComponentId(Guid value)
    {
        Value = value;
    }

    public static ComponentId Restore(Guid value) => new(value);
}

public struct ComponentName
{
    public string Value { get; }

    private ComponentName(string value)
    {
        Value = value;
    }

    public static ComponentName Restore(string value) => new(value);
}
