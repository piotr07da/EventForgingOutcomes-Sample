namespace EFO.Sales.Domain;

public sealed record ProductIntroduced(Guid ProductId);

public sealed record ProductPriced(Guid ProductId, decimal UnitPrice);

public sealed record ProductQuantityDiscountDefined(Guid ProductId, int ThresholdQuantity, decimal PercentageDiscount);
