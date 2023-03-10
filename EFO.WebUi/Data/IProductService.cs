namespace EFO.WebUi.Data;

public interface IProductService
{
    Task<ProductDto[]> GetProductsAsync();
}
