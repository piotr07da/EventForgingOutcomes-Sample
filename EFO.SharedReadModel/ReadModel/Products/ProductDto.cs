namespace EFO.SharedReadModel.ReadModel.Products;

public sealed class ProductDto
{
    public ProductDto(Guid productId)
    {
        ProductId = productId;
        Name = string.Empty;
        Categories = Array.Empty<ProductCategoryDto>();
        NumericProperties = Array.Empty<ProductNumericPropertyWithValueDto>();
        TextProperties = Array.Empty<ProductTextPropertyWithValueDto>();
        Prices = Array.Empty<Price>();
    }

    public Guid ProductId { get; set; }

    public string Name { get; set; }

    public ProductCategoryDto[] Categories { get; set; }

    public ProductNumericPropertyWithValueDto[] NumericProperties { get; set; }

    public ProductTextPropertyWithValueDto[] TextProperties { get; set; }

    public Price[] Prices { get; set; }

    public sealed record Price(int QuantityThreshold, decimal UnitPrice);
}
