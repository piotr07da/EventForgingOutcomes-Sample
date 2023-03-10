namespace EFO.SharedReadModel.ReadModel.Products;

public interface IPropertiesReadModel
{
    void AddNumericProperty(Guid propertyId, string name, string unit);

    ProductNumericPropertyDto GetNumericProperty(Guid propertyId);

    void AddTextProperty(Guid propertyId, string name);

    ProductTextPropertyDto GetTextProperty(Guid propertyId);
}
