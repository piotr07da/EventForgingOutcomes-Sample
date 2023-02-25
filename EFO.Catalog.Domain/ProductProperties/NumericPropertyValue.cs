using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct NumericPropertyValue
{
    private NumericPropertyValue(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (NumericPropertyValue)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(NumericPropertyValue lhs, NumericPropertyValue rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(NumericPropertyValue lhs, NumericPropertyValue rhs) => !(lhs == rhs);

    public static implicit operator NumericPropertyValue(decimal value) => FromValue(value);
    public static implicit operator decimal(NumericPropertyValue value) => value.Value;

    public static NumericPropertyValue Restore(decimal value) => new(value);

    public static NumericPropertyValue FromValue(decimal value)
    {
        return new NumericPropertyValue(value);
    }
}
