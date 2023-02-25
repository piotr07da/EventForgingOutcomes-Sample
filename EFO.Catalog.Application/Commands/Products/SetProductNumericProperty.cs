namespace EFO.Catalog.Application.Commands.Products;

public sealed record SetProductNumericProperty(Guid ProductId, Guid PropertyId, decimal PropertyValue);
