using System.Globalization;
using EFO.Shared.Domain;

namespace EFO.Sales.Domain;

public readonly struct Money
{
    private Money(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    public override bool Equals(object? obj) => obj != null && this == (Money)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(Money lhs, Money rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(Money lhs, Money rhs) => !(lhs == rhs);

    public static Money operator +(Money lhs, Money rhs) => FromValue(lhs.Value + rhs.Value);
    public static Money operator *(Money lhs, Quantity rhs) => FromValue(lhs.Value * rhs.Value);

    public static implicit operator Money(decimal value) => FromValue(value);
    public static implicit operator decimal(Money value) => value.Value;
    public static implicit operator string(Money value) => value.Value.ToString(CultureInfo.InvariantCulture);

    public static Money Zero => new(0);

    public static Money Restore(decimal value) => new(value);

    public static Money FromValue(decimal value)
    {
        return new Money(value);
    }
}
