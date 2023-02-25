namespace EFO.Sales.Domain.Products;

public sealed record ProductIntroduced(Guid ProductId);

public sealed record ProductNamed(Guid ProductId, string Name);

public sealed record ProductPriced(Guid ProductId, int QuantityThreshold, decimal UnitPrice);
