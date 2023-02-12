using EventForging;

namespace EFO.DeliveryAcceptance.Domain;

public class Component : IEventForged
{
    private bool _measured;
    private bool _weighed;
    private bool _inspectionCompleted;

    public Component()
    {
        Events = Events.CreateFor(this);
        Errors = new List<string>();
    }

    public Events Events { get; }
    public IList<string> Errors { get; }

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

    public void Measure(ComponentInspector componentInspector, Length width, Length height, Length depth)
    {
        EnsureComponentInspectorIsCertified(componentInspector);
        Errors.AddIf(DomainErrors.ComponentInspectionAlreadyCompleted, _inspectionCompleted);

        DomainException.ThrowIfErrors(Errors);

        Events.Apply(new ComponentMeasured(Id.Value, width.Value, height.Value, depth.Value));
    }

    public void Weigh(ComponentInspector componentInspector, Weight weight)
    {
        EnsureComponentInspectorIsCertified(componentInspector);
        Errors.AddIf(DomainErrors.ComponentInspectionAlreadyCompleted, _inspectionCompleted);

        DomainException.ThrowIfErrors(Errors);

        Events.Apply(new ComponentWeighed(Id.Value, weight.Value));
    }

    public void CompleteInspection(ComponentInspector componentInspector)
    {
        EnsureComponentInspectorIsCertified(componentInspector);
        Errors.AddIf(DomainErrors.ComponentNotMeasured, !_measured);
        Errors.AddIf(DomainErrors.ComponentNotWeighed, !_weighed);

        DomainException.ThrowIfErrors(Errors);

        Events.Apply(new ComponentInspectionCompleted(Id.Value));
    }

    private void EnsureComponentInspectorIsCertified(ComponentInspector componentInspector)
    {
        Errors.AddIf(DomainErrors.ComponentInspectorDoesNotHaveRequiredCertification, componentInspector.AllowedComponentClass < Class);
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

    private void Apply(ComponentMeasured e)
    {
        _measured = true;
    }

    private void Apply(ComponentWeighed e)
    {
        _weighed = true;
    }

    private void Apply(ComponentInspectionCompleted e)
    {
        _inspectionCompleted = true;
    }
}
