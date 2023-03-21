namespace EFO.SharedReadModel.Queries;

public sealed record GetProducts(Guid CategoryId, IDictionary<Guid, NumericPropertyFilter> NumericPropertiesFilters, IDictionary<Guid, TextPropertyFilter> TextPropertiesFilters);

public sealed record NumericPropertyFilter(decimal MinValue, decimal MaxValue);

public sealed record TextPropertyFilter(string[] Values);
