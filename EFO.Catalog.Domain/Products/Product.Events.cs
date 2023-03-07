namespace EFO.Catalog.Domain.Products;

public sealed record ProductIntroduced(Guid ProductId);

public sealed record ProductNamed(Guid ProductId, string Name);

public sealed record ProductDescribed(Guid ProductId, string Description);

public sealed record ProductNumericPropertySet(Guid ProductId, Guid PropertyId, decimal PropertyValue);

public sealed record ProductTextPropertySet(Guid ProductId, Guid PropertyId, string PropertyValue);

public sealed record ProductMovedToCategory(Guid ProductId, Guid CategoryId);
