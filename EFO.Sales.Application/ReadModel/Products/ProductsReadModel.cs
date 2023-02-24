namespace EFO.Sales.Application.ReadModel.Products;

internal sealed class ProductsReadModel : IProductsReadModel
{
    private readonly IDictionary<Guid, ProductDto> _entries = new Dictionary<Guid, ProductDto>();

    public ProductsDto GetProducts()
    {
        return new ProductsDto(_entries.Values.ToArray());
    }

    public ProductDto GetOrAdd(Guid productId)
    {
        if (!_entries.TryGetValue(productId, out var product))
        {
            product = new ProductDto(productId);
            _entries.Add(productId, product);
        }

        return product;
    }
}
