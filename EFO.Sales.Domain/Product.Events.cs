namespace EFO.Sales.Domain;

public sealed record ProductIntroduced(Guid ProductId);

public sealed record ProductPriced(Guid ProductId, int ThresholdQuantity, decimal UnitPrice);
