namespace EFO.Shared.Domain;

public static class SharedDomainErrors
{
    public static readonly string ProductIdCannotBeEmpty = nameof(ProductIdCannotBeEmpty);

    public static void AddIf(this IList<string> domainErrors, string domainError, bool condition)
    {
        if (condition)
        {
            domainErrors.Add(domainError);
        }
    }
}
