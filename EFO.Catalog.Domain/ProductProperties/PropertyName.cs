using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct PropertyName
{
    private PropertyName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (PropertyName)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(PropertyName lhs, PropertyName rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(PropertyName lhs, PropertyName rhs) => !(lhs == rhs);

    public static implicit operator PropertyName(string value) => FromValue(value);
    public static implicit operator string(PropertyName value) => value.Value;

    public static PropertyName Restore(string value) => new(value);

    public static PropertyName FromValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(new DomainError(CatalogDomainErrors.PropertyNameCannotBeEmpty));
        }

        return new PropertyName(value);
    }
}
