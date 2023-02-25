namespace EFO.Catalog.Domain;

public static class CatalogDomainErrors
{
    public static readonly string CategoryIdCannotBeEmpty = nameof(CategoryIdCannotBeEmpty);
    public static readonly string CategoryNameCannotBeEmpty = nameof(CategoryNameCannotBeEmpty);
    public static readonly string ProductNameCannotBeEmpty = nameof(ProductNameCannotBeEmpty);
    public static readonly string PropertyIdCannotBeEmpty = nameof(PropertyIdCannotBeEmpty);
    public static readonly string PropertyNameCannotBeEmpty = nameof(PropertyNameCannotBeEmpty);
    public static readonly string PropertyUnitCannotBeEmpty = nameof(PropertyUnitCannotBeEmpty);

    public static void AddIf(this IList<string> domainErrors, string domainError, bool condition)
    {
        if (condition)
        {
            domainErrors.Add(domainError);
        }
    }
}
