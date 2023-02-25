namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct ProductPropertyName
{
    private ProductPropertyName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (ProductPropertyName)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductPropertyName lhs, ProductPropertyName rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductPropertyName lhs, ProductPropertyName rhs) => !(lhs == rhs);

    public static implicit operator ProductPropertyName(string value) => FromValue(value);
    public static implicit operator string(ProductPropertyName value) => value.Value;

    public static ProductPropertyName Restore(string value) => new(value);

    public static ProductPropertyName FromValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(new DomainError(DomainErrors.ProductPropertyNameCannotBeEmpty));
        }

        return new ProductPropertyName(value);
    }
}
