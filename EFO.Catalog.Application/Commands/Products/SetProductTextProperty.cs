namespace EFO.Catalog.Application.Commands.Products;

public sealed record SetProductTextProperty(Guid ProductId, Guid PropertyId, decimal PropertyText);
