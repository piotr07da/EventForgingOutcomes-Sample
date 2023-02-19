namespace EFO.Sales.Domain;

public class ProductPrices
{
    private readonly IDictionary<Quantity, Money> _pricesForQuantityThreshold = new Dictionary<Quantity, Money>();

    private PriceForQuantityThreshold[] PricesForQuantityThreshold => _pricesForQuantityThreshold.Select(pfq => PriceForQuantityThreshold.FromValues(pfq.Key, pfq.Value)).OrderBy(pfq => pfq.Threshold.Value).ToArray();

    public Money GetUnitPriceForQuantity(Quantity quantity)
    {
        for (var i = PricesForQuantityThreshold.Length - 1; i >= 0; --i)
        {
            var priceForThreshold = PricesForQuantityThreshold[i];
            if (quantity >= priceForThreshold.Threshold)
            {
                return priceForThreshold.UnitPrice;
            }
        }

        throw new DomainException(new DomainError(DomainErrors.QuantityToLowForPricing)
            .WithData("Quantity", quantity)
            .WithData("MinimalQuantity", PricesForQuantityThreshold[0].Threshold));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    internal void Apply(ProductPriced e)
    {
        _pricesForQuantityThreshold[e.ThresholdQuantity] = e.UnitPrice;
    }
}
