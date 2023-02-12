namespace EFO.Sales.Domain;

public readonly struct OrderItemId
{
    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (OrderItemId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(OrderItemId lhs, OrderItemId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(OrderItemId lhs, OrderItemId rhs) => !(lhs == rhs);

    public static implicit operator OrderItemId(Guid value) => FromValue(value);
    public static implicit operator Guid(OrderItemId value) => value.Value;
    public static implicit operator string(OrderItemId value) => value.Value.ToString();

    public static OrderItemId Restore(Guid value) => new(value);

    public static OrderItemId FromValue(Guid value)
    {
        return new OrderItemId(value);
    }
}
