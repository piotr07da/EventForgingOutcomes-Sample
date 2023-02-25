using EFO.Shared.Domain;

namespace EFO.Sales.Domain.ProductProperties;

public readonly struct ProductPropertyValue
{
    private ProductPropertyValue(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (ProductPropertyValue)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductPropertyValue lhs, ProductPropertyValue rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductPropertyValue lhs, ProductPropertyValue rhs) => !(lhs == rhs);

    public static implicit operator ProductPropertyValue(decimal value) => FromValue(value);
    public static implicit operator decimal(ProductPropertyValue value) => value.Value;

    public static ProductPropertyValue Restore(decimal value) => new(value);

    public static ProductPropertyValue FromValue(decimal value)
    {
        return new ProductPropertyValue(value);
    }
}
