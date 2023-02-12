namespace EFO.Sales.Domain;

public readonly struct Quantity
{
    private Quantity(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (Quantity)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(Quantity lhs, Quantity rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(Quantity lhs, Quantity rhs) => !(lhs == rhs);

    public static implicit operator Quantity(int value) => FromValue(value);
    public static implicit operator int(Quantity value) => value.Value;
    public static implicit operator string(Quantity value) => value.Value.ToString();

    public static Quantity Restore(int value) => new(value);

    public static Quantity FromValue(int value)
    {
        return new Quantity(value);
    }
}
