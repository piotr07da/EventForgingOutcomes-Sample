using System.Text.Json;

namespace EFO.WebUi.Data;

public sealed class ProductCategoriesService : IProductCategoriesService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductCategoriesService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<ProductCategoryDto[]> GetCategoriesAsync(Guid? parentCategoryId)
    {
        var httpClient = _httpClientFactory.CreateClient("EFO");

        var r = await httpClient.GetAsync("/product-categories");
        var s = await r.Content.ReadAsStringAsync();

        var products = JsonSerializer.Deserialize<ProductCategoriesDto>(s, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, })!;
        return products.Categories;
    }
}
