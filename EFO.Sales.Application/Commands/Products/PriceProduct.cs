namespace EFO.Sales.Application.Commands.Products;

public sealed record PriceProduct(Guid ProductId, int QuantityThreshold, decimal UnitPrice);
