namespace EFO.SharedReadModel.ReadModel.Products;

public sealed record ProductNumericPropertyWithValueDto(Guid Id, string Name, string Unit, decimal Value);
