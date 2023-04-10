namespace EFO.WebUi.Data;

public sealed record ProductCategoryDto(Guid Id, Guid? ParentId, ProductCategoryDto? Parent, string Name, int NumberOfProducts);
