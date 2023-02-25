namespace EFO.Catalog.Domain.Products;

public sealed record ProductIntroduced(Guid ProductId);

public sealed record ProductNamed(Guid ProductId, string Name);

public sealed record ProductValuePropertySet(Guid ProductId, string PropertyName, decimal PropertyValue, string PropertyUnit);

public sealed record ProductTextPropertySet(Guid ProductId, string PropertyName, string PropertyValue);

public sealed record ProductMovedToCategory(Guid ProductId, Guid CategoryId);
