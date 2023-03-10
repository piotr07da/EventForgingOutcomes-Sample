namespace EFO.SharedReadModel.ReadModel.Products;

internal sealed class PropertiesReadModel : IPropertiesReadModel
{
    private readonly IDictionary<Guid, ProductNumericPropertyDto> _numericPropertyEntries = new Dictionary<Guid, ProductNumericPropertyDto>();
    private readonly IDictionary<Guid, ProductTextPropertyDto> _textPropertyEntries = new Dictionary<Guid, ProductTextPropertyDto>();

    public void AddNumericProperty(Guid propertyId, string name, string unit)
    {
        var property = new ProductNumericPropertyDto(propertyId, name, unit);
        _numericPropertyEntries.Add(propertyId, property);
    }

    public ProductNumericPropertyDto GetNumericProperty(Guid propertyId)
    {
        return _numericPropertyEntries[propertyId];
    }

    public void AddTextProperty(Guid propertyId, string name)
    {
        var property = new ProductTextPropertyDto(propertyId, name);
        _textPropertyEntries.Add(propertyId, property);
    }

    public ProductTextPropertyDto GetTextProperty(Guid propertyId)
    {
        return _textPropertyEntries[propertyId];
    }
}
