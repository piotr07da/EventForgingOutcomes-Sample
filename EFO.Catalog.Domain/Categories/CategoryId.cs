using EFO.Shared.Domain;

namespace EFO.Catalog.Domain.Categories;

public readonly struct CategoryId
{
    private CategoryId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();
    public override bool Equals(object? obj) => obj != null && this == (CategoryId)obj;
    public override int GetHashCode() => EqualityHelper.GetHashCode(Value);
    public static bool operator ==(CategoryId lhs, CategoryId rhs) => EqualityHelper.Equals(lhs, rhs, x => new object[] { x.Value, });
    public static bool operator !=(CategoryId lhs, CategoryId rhs) => !(lhs == rhs);

    public static implicit operator CategoryId(Guid value) => FromValue(value);
    public static implicit operator Guid(CategoryId value) => value.Value;
    public static implicit operator string(CategoryId value) => value.Value.ToString();

    public static CategoryId Restore(Guid value) => new(value);

    public static CategoryId FromValue(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(new DomainError(CatalogDomainErrors.CategoryIdCannotBeEmpty));
        }

        return new CategoryId(value);
    }
}
