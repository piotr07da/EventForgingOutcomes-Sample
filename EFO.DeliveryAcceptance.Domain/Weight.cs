namespace EFO.DeliveryAcceptance.Domain;

public struct Weight
{
    public double Value { get; }

    private Weight(double value)
    {
        Value = value;
    }

    internal static Weight Restore(double value) => new(value);

    public static Weight FromValue(double value)
    {
        return new Weight(value);
    }
}
