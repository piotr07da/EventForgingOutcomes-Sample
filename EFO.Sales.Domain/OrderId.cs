namespace EFO.Sales.Domain;

public readonly struct OrderId
{
    private OrderId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (OrderId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(OrderId lhs, OrderId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(OrderId lhs, OrderId rhs) => !(lhs == rhs);

    public static implicit operator OrderId(Guid value) => FromValue(value);
    public static implicit operator Guid(OrderId value) => value.Value;
    public static implicit operator string(OrderId value) => value.Value.ToString();

    public static OrderId Restore(Guid value) => new(value);

    public static OrderId FromValue(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(new DomainError(DomainErrors.OrderIdCannotBeEmpty));
        }

        return new OrderId(value);
    }
}
