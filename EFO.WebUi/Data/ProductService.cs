using System.Text.Json;

namespace EFO.WebUi.Data;

public sealed class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<ProductDto[]> GetProductsAsync(Guid categoryId, IDictionary<Guid, NumericPropertyFilter> numericPropertiesFilters, IDictionary<Guid, TextPropertyFilter> textPropertiesFilters)
    {
        var httpClient = _httpClientFactory.CreateClient("EFO");

        var r = await httpClient.PostAsync("/products/search", JsonContent.Create(new SearchProductsBody(categoryId, numericPropertiesFilters, textPropertiesFilters)));
        var s = await r.Content.ReadAsStringAsync();

        var products = JsonSerializer.Deserialize<ProductsDto>(s, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, })!;
        return products.Products;
    }

    public sealed record SearchProductsBody(
        Guid CategoryId,
        IDictionary<Guid, NumericPropertyFilter> NumericPropertiesFilters,
        IDictionary<Guid, TextPropertyFilter> TextPropertiesFilters);

    public sealed record NumericPropertyFilter(decimal MinValue, decimal MaxValue);

    public sealed record TextPropertyFilter(string[] Values);
}
