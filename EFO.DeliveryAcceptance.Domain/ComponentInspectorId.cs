namespace EFO.DeliveryAcceptance.Domain;

public struct ComponentInspectorId
{
    public Guid Value { get; }

    private ComponentInspectorId(Guid value)
    {
        Value = value;
    }

    internal static ComponentInspectorId Restore(Guid value) => new(value);

    public static ComponentInspectorId FromValue(Guid value)
    {
        return new ComponentInspectorId(value);
    }
}
