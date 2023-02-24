namespace EFO.Sales.Application.ReadModel.Products;

public sealed class ProductDto
{
    public ProductDto(Guid productId)
    {
        ProductId = productId;
        Prices = Array.Empty<Price>();
    }

    public Guid ProductId { get; set; }

    public string? Name { get; set; }

    public Price[] Prices { get; set; }

    public sealed record Price(int QuantityThreshold, decimal UnitPrice);
}
