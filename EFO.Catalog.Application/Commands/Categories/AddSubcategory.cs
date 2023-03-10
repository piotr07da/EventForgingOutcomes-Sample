namespace EFO.Catalog.Application.Commands.Categories;

public sealed record AddSubcategory(Guid CategoryId, Guid SubcategoryId, string SubcategoryName);
