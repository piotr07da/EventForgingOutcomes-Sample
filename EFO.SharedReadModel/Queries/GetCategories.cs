namespace EFO.SharedReadModel.Queries;

public sealed record GetCategories(Guid? ParentCategoryId);

public sealed record GetCategory(Guid CategoryId);
