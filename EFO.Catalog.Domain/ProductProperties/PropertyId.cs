using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct PropertyId
{
    private PropertyId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (PropertyId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(PropertyId lhs, PropertyId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(PropertyId lhs, PropertyId rhs) => !(lhs == rhs);

    public static implicit operator PropertyId(Guid value) => FromValue(value);
    public static implicit operator Guid(PropertyId value) => value.Value;
    public static implicit operator string(PropertyId value) => value.Value.ToString();

    public static PropertyId Restore(Guid value) => new(value);

    public static PropertyId FromValue(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(new DomainError(CatalogDomainErrors.PropertyIdCannotBeEmpty));
        }

        return new PropertyId(value);
    }
}
