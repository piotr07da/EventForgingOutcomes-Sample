namespace EFO.Catalog.Domain;

public static class DomainErrors
{
    public static readonly string ProductNameCannotBeEmpty = nameof(ProductNameCannotBeEmpty);
    public static readonly string ProductPropertyIdCannotBeEmpty = nameof(ProductPropertyIdCannotBeEmpty);
    public static readonly string ProductPropertyNameCannotBeEmpty = nameof(ProductPropertyNameCannotBeEmpty);
    public static readonly string ProductPropertyUnitCannotBeEmpty = nameof(ProductPropertyUnitCannotBeEmpty);

    public static void AddIf(this IList<string> domainErrors, string domainError, bool condition)
    {
        if (condition)
        {
            domainErrors.Add(domainError);
        }
    }
}
