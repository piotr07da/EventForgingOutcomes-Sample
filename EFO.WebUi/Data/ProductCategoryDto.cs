namespace EFO.WebUi.Data;

public sealed record ProductCategoryDto(Guid Id, Guid? ParentId, string Name);
