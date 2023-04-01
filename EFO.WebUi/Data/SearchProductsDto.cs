namespace EFO.WebUi.Data;

public sealed record SearchProductsDto(
        Guid CategoryId,
        IDictionary<Guid, NumericPropertyFilter> NumericPropertiesFilters,
        IDictionary<Guid, TextPropertyFilter> TextPropertiesFilters);
