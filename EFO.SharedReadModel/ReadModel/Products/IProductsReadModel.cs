namespace EFO.SharedReadModel.ReadModel.Products;

public interface IProductsReadModel
{
    ProductsDto GetProducts();

    void TryAddProduct(Guid productId);

    ProductDto GetProduct(Guid productId);
}
