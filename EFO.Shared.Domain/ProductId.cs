namespace EFO.Shared.Domain;

public readonly struct ProductId
{
    private ProductId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (ProductId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(ProductId lhs, ProductId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(ProductId lhs, ProductId rhs) => !(lhs == rhs);

    public static implicit operator ProductId(Guid value) => FromValue(value);
    public static implicit operator Guid(ProductId value) => value.Value;
    public static implicit operator string(ProductId value) => value.Value.ToString();

    public static ProductId Restore(Guid value) => new(value);

    public static ProductId FromValue(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(new DomainError(DomainErrors.ProductIdCannotBeEmpty));
        }

        return new ProductId(value);
    }
}
