namespace EFO.Catalog.Application.Commands.Products;

public sealed record SetProductValueProperty(Guid ProductId, string PropertyName, decimal PropertyValue, string PropertyUnit);
