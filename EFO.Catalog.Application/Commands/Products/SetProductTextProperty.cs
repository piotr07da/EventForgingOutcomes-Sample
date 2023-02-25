namespace EFO.Catalog.Application.Commands.Products;

public sealed record SetProductTextProperty(Guid ProductId, string PropertyName, decimal PropertyText);
