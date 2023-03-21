namespace EFO.SharedReadModel.ReadModel.Products;

public interface IProductsReadModel
{
    ProductsDto GetProducts(Guid categoryId, IDictionary<Guid, (decimal MinValue, decimal MaxValue)> numericPropertiesFilters, IDictionary<Guid, string[]> textPropertiesFilters);

    void TryAddProduct(Guid productId);

    ProductDto GetProduct(Guid productId);
}
