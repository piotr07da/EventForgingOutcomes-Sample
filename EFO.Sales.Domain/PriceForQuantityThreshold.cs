namespace EFO.Sales.Domain;

public readonly struct PriceForQuantityThreshold
{
    private PriceForQuantityThreshold(Quantity threshold, Money unitPrice)
    {
        Threshold = threshold;
        UnitPrice = unitPrice;
    }

    public Quantity Threshold { get; }
    public Money UnitPrice { get; }

    public override string ToString() => $"{Threshold} - {UnitPrice}";
    public override bool Equals(object? obj) => obj != null && this == (PriceForQuantityThreshold)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Threshold, UnitPrice);
    public static bool operator ==(PriceForQuantityThreshold lhs, PriceForQuantityThreshold rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Threshold, x.UnitPrice, });
    public static bool operator !=(PriceForQuantityThreshold lhs, PriceForQuantityThreshold rhs) => !(lhs == rhs);

    public static PriceForQuantityThreshold Restore(Quantity threshold, Money unitPrice) => new(threshold, unitPrice);

    public static PriceForQuantityThreshold FromValues(Quantity threshold, Money unitPrice)
    {
        return new PriceForQuantityThreshold(threshold, unitPrice);
    }
}
