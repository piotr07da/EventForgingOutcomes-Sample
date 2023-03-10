namespace EFO.Catalog.Domain.Categories;

public sealed record CategoryAdded(Guid CategoryId);

public sealed record CategoryNamed(Guid CategoryId, string Name);

public sealed record CategoryAttachedToParent(Guid CategoryId, Guid ParentCategoryId);
