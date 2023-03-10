namespace EFO.SharedReadModel.ReadModel.Products;

internal sealed class ProductsReadModel : IProductsReadModel
{
    private readonly IDictionary<Guid, ProductDto> _entries = new Dictionary<Guid, ProductDto>();

    public ProductsDto GetProducts()
    {
        return new ProductsDto(_entries.Values.ToArray());
    }

    public void TryAddProduct(Guid productId)
    {
        if (!_entries.ContainsKey(productId))
        {
            var product = new ProductDto(productId);
            _entries.Add(productId, product);
        }
    }

    public ProductDto GetProduct(Guid productId)
    {
        return _entries[productId];
    }
}
