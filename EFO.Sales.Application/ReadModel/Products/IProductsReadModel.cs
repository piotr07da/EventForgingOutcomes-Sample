namespace EFO.Sales.Application.ReadModel.Products;

public interface IProductsReadModel
{
    ProductsDto GetProducts();

    ProductDto GetOrAdd(Guid productId);
}
