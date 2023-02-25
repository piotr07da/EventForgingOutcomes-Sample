namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct ProductPropertyUnit
{
    private ProductPropertyUnit(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (ProductPropertyUnit)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductPropertyUnit lhs, ProductPropertyUnit rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductPropertyUnit lhs, ProductPropertyUnit rhs) => !(lhs == rhs);

    public static implicit operator ProductPropertyUnit(string value) => FromValue(value);
    public static implicit operator string(ProductPropertyUnit value) => value.Value;

    public static ProductPropertyUnit Restore(string value) => new(value);

    public static ProductPropertyUnit FromValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(new DomainError(DomainErrors.ProductPropertyUnitCannotBeEmpty));
        }

        return new ProductPropertyUnit(value);
    }
}
