namespace EFO.SharedReadModel.ReadModel.Products;

internal sealed class ProductsReadModel : IProductsReadModel
{
    private readonly IDictionary<Guid, ProductDto> _entries = new Dictionary<Guid, ProductDto>();

    public ProductsDto GetProducts(Guid categoryId, IDictionary<Guid, (decimal MinValue, decimal MaxValue)> numericPropertiesFilters, IDictionary<Guid, string[]> textPropertiesFilters)
    {
        return new ProductsDto(_entries
            .Values
            .Where(v => v.Categories.Any(c => c.Id == categoryId))
            .Where(v =>
            {
                foreach (var pFilter in numericPropertiesFilters)
                {
                    var p = v.NumericProperties.FirstOrDefault(p => p.Id == pFilter.Key);
                    if (p == null)
                        continue;

                    if (p.Value < pFilter.Value.MinValue || p.Value > pFilter.Value.MaxValue)
                    {
                        return false;
                    }
                }

                return true;
            })
            .Where(v =>
            {
                foreach (var pFilter in textPropertiesFilters)
                {
                    var p = v.TextProperties.FirstOrDefault(p => p.Id == pFilter.Key);
                    if (p == null)
                        continue;

                    if (!pFilter.Value.Contains(p.Value))
                    {
                        return false;
                    }
                }

                return true;
            })
            .ToArray());
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
