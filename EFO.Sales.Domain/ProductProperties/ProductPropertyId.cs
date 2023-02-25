using EFO.Shared.Domain;

namespace EFO.Sales.Domain.ProductProperties;

public readonly struct ProductPropertyId
{
    private ProductPropertyId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (ProductPropertyId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductPropertyId lhs, ProductPropertyId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductPropertyId lhs, ProductPropertyId rhs) => !(lhs == rhs);

    public static implicit operator ProductPropertyId(Guid value) => FromValue(value);
    public static implicit operator Guid(ProductPropertyId value) => value.Value;
    public static implicit operator string(ProductPropertyId value) => value.Value.ToString();

    public static ProductPropertyId Restore(Guid value) => new(value);

    public static ProductPropertyId FromValue(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(new DomainError(DomainErrors.ProductPropertyIdCannotBeEmpty));
        }

        return new ProductPropertyId(value);
    }
}
