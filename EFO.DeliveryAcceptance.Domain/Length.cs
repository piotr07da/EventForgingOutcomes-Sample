namespace EFO.DeliveryAcceptance.Domain;

public struct Length
{
    public double Value { get; }

    private Length(double value)
    {
        Value = value;
    }

    internal static Length Restore(double value) => new(value);

    public static Length FromValue(double value)
    {
        return new Length(value);
    }
}
