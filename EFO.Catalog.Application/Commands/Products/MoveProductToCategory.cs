namespace EFO.Catalog.Application.Commands.Products;

public sealed record MoveProductToCategory(Guid ProductId, Guid CategoryId);
