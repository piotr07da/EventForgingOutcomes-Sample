using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.ProductProperties;

public readonly struct TextPropertyValue
{
    private TextPropertyValue(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (TextPropertyValue)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(TextPropertyValue lhs, TextPropertyValue rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(TextPropertyValue lhs, TextPropertyValue rhs) => !(lhs == rhs);

    public static implicit operator TextPropertyValue(string value) => FromValue(value);
    public static implicit operator string(TextPropertyValue value) => value.Value;

    public static TextPropertyValue Restore(string value) => new(value);

    public static TextPropertyValue FromValue(string value)
    {
        return new TextPropertyValue(value);
    }
}
