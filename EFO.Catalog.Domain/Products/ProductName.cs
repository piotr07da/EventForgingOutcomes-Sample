using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.Products;

public readonly struct ProductName
{
    private ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (ProductName)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductName lhs, ProductName rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductName lhs, ProductName rhs) => !(lhs == rhs);

    public static implicit operator ProductName(string value) => FromValue(value);
    public static implicit operator string(ProductName value) => value.Value;

    public static ProductName Restore(string value) => new(value);

    public static ProductName FromValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(new DomainError(CatalogDomainErrors.ProductNameCannotBeEmpty));
        }

        return new ProductName(value);
    }
}
