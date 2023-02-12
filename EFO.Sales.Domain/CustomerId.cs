namespace EFO.Sales.Domain;

public readonly struct CustomerId
{
    private CustomerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (CustomerId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(CustomerId lhs, CustomerId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(CustomerId lhs, CustomerId rhs) => !(lhs == rhs);

    public static implicit operator CustomerId(Guid value) => FromValue(value);
    public static implicit operator Guid(CustomerId value) => value.Value;
    public static implicit operator string(CustomerId value) => value.Value.ToString();

    public static CustomerId Restore(Guid value) => new(value);

    public static CustomerId FromValue(Guid value)
    {
        return new CustomerId(value);
    }
}
