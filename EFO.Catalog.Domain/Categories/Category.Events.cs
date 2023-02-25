namespace EFO.Catalog.Domain.Categories;

public sealed record CategoryCreated(Guid CategoryId, string Name);

public sealed record CategoryAttachedToParent(Guid CategoryId, Guid ParentCategoryId);
