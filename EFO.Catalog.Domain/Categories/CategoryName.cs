using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.Categories;

public readonly struct CategoryName
{
    private CategoryName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj != null && this == (CategoryName)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(CategoryName lhs, CategoryName rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(CategoryName lhs, CategoryName rhs) => !(lhs == rhs);

    public static implicit operator CategoryName(string value) => FromValue(value);
    public static implicit operator string(CategoryName value) => value.Value;

    public static CategoryName Restore(string value) => new(value);

    public static CategoryName FromValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException(new DomainError(CatalogDomainErrors.CategoryNameCannotBeEmpty));
        }

        return new CategoryName(value);
    }
}
