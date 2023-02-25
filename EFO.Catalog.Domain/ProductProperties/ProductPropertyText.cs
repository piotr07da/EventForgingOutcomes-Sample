namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct ProductPropertyText
{
    private ProductPropertyText(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (ProductPropertyText)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductPropertyText lhs, ProductPropertyText rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductPropertyText lhs, ProductPropertyText rhs) => !(lhs == rhs);

    public static implicit operator ProductPropertyText(string value) => FromValue(value);
    public static implicit operator string(ProductPropertyText value) => value.Value;

    public static ProductPropertyText Restore(string value) => new(value);

    public static ProductPropertyText FromValue(string value)
    {
        return new ProductPropertyText(value);
    }
}
