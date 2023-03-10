namespace EFO.SharedReadModel.ReadModel.Products;

public sealed record ProductCategoryDto(Guid Id)
{
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
}
