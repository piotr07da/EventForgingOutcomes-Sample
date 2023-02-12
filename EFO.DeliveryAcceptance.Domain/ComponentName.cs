namespace EFO.DeliveryAcceptance.Domain;

public struct ComponentName
{
    public string Value { get; }

    private ComponentName(string value)
    {
        Value = value;
    }

    public static ComponentName Restore(string value) => new(value);

    public static ComponentName FromValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.ComponentNameIsInvalid);
        }

        return new ComponentName(value);
    }
}
