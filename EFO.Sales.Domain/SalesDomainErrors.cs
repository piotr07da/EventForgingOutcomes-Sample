namespace EFO.Sales.Domain;

public static class SalesDomainErrors
{
    public static readonly string OrderItemWithGivenIdNotFound = nameof(OrderItemWithGivenIdNotFound);
    public static readonly string OrderIdCannotBeEmpty = nameof(OrderIdCannotBeEmpty);
    public static readonly string PriceForLowerQuantityThresholdMustBeHigher = nameof(PriceForLowerQuantityThresholdMustBeHigher);
    public static readonly string PriceForHigherQuantityThresholdMustBeLower = nameof(PriceForHigherQuantityThresholdMustBeLower);
    public static readonly string ProductIdCannotBeEmpty = nameof(ProductIdCannotBeEmpty);
    public static readonly string ProductNameCannotBeEmpty = nameof(ProductNameCannotBeEmpty);
    public static readonly string QuantityToLowForPricing = nameof(QuantityToLowForPricing);

    public static void AddIf(this IList<string> domainErrors, string domainError, bool condition)
    {
        if (condition)
        {
            domainErrors.Add(domainError);
        }
    }
}
