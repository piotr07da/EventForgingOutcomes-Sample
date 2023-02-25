using EFO.Shared.Domain;
using EventForging;

namespace EFO.Sales.Domain.Products;

public class ProductPrices
{
    private readonly Product _product;
    private readonly IDictionary<Quantity, Money> _pricesForQuantityThreshold = new Dictionary<Quantity, Money>();

    public ProductPrices(Product product)
    {
        _product = product;
    }

    private Events Events => _product.Events;

    private (Quantity QuantityThreshold, Money UnitPrice)[] PricesOrderedByQuantityThreshold =>
        _pricesForQuantityThreshold
            .Select(pfq => (pfq.Key, pfq.Value))
            .OrderBy(pfq => pfq.Key.Value)
            .ToArray();

    public Money GetUnitPriceForQuantity(Quantity quantity)
    {
        for (var i = PricesOrderedByQuantityThreshold.Length - 1; i >= 0; --i)
        {
            var priceForThreshold = PricesOrderedByQuantityThreshold[i];
            if (quantity >= priceForThreshold.QuantityThreshold)
            {
                return priceForThreshold.UnitPrice;
            }
        }

        throw new DomainException(new DomainError(DomainErrors.QuantityToLowForPricing)
            .WithData("Quantity", quantity)
            .WithData("MinimalQuantity", PricesOrderedByQuantityThreshold[0].QuantityThreshold));
    }

    public void Add(Quantity quantityThreshold, Money unitPrice)
    {
        if (PriceForLowerQuantityThresholdIfSuchExistsIsSameOrLower(quantityThreshold, unitPrice))
        {
            throw new DomainException(new DomainError(DomainErrors.PriceForLowerQuantityThresholdMustBeHigher));
        }

        if (PriceForHigherQuantityThresholdIfSuchExistsIsSameOrHigher(quantityThreshold, unitPrice))
        {
            throw new DomainException(new DomainError(DomainErrors.PriceForHigherQuantityThresholdMustBeLower));
        }

        Events.Apply(new ProductPriced(_product.Id, quantityThreshold, unitPrice));
    }

    private bool PriceForLowerQuantityThresholdIfSuchExistsIsSameOrLower(Quantity quantityThreshold, Money unitPrice)
    {
        var orderedPrices = PricesOrderedByQuantityThreshold;

        foreach (var op in orderedPrices)
        {
            if (quantityThreshold < op.QuantityThreshold)
            {
                if (unitPrice <= op.UnitPrice)
                {
                    return true;
                }

                break;
            }
        }

        return false;
    }

    private bool PriceForHigherQuantityThresholdIfSuchExistsIsSameOrHigher(Quantity quantityThreshold, Money unitPrice)
    {
        var orderedPrices = PricesOrderedByQuantityThreshold;

        for (var i = orderedPrices.Length - 1; i >= 0; --i)
        {
            var op = orderedPrices[i];

            if (quantityThreshold > op.QuantityThreshold)
            {
                if (unitPrice >= op.UnitPrice)
                {
                    return true;
                }

                break;
            }
        }

        return false;
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    internal void Apply(ProductPriced e)
    {
        _pricesForQuantityThreshold[e.QuantityThreshold] = e.UnitPrice;
    }
}
