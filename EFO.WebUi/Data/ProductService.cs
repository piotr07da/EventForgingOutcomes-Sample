using System.Text.Json;

namespace EFO.WebUi.Data;

public sealed class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<ProductDto[]> GetProductsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("EFO");

        var r = await httpClient.GetAsync("/products");
        var s = await r.Content.ReadAsStringAsync();

        var products = JsonSerializer.Deserialize<ProductsDto>(s, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, })!;
        return products.Products;
    }
}
