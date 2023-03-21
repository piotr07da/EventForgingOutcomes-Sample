using EFO.SharedReadModel.Queries;

namespace EFO.WebApi.Controllers.Products;

public sealed record SearchProductsBody(IDictionary<Guid, NumericPropertyFilter> NumericPropertiesFilters, IDictionary<Guid, TextPropertyFilter> TextPropertiesFilters);
