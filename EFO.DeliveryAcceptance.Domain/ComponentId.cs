namespace EFO.DeliveryAcceptance.Domain;

public struct ComponentId
{
    public Guid Value { get; }

    private ComponentId(Guid value)
    {
        Value = value;
    }

    public static ComponentId Restore(Guid value) => new(value);

    public static ComponentId FromValue(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(DomainErrors.ComponentIdIsInvalid);
        }

        return new ComponentId(value);
    }
}
