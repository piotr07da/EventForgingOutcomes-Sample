namespace EFO.Sales.Domain;

public static class DomainErrors
{
    public static readonly string OrderItemWithGivenIdNotFound = nameof(OrderItemWithGivenIdNotFound);
    public static readonly string QuantityToLowForPricing = nameof(QuantityToLowForPricing);

    public static void AddIf(this IList<string> domainErrors, string domainError, bool condition)
    {
        if (condition)
        {
            domainErrors.Add(domainError);
        }
    }
}
