// ReSharper disable InconsistentNaming

using EFO.Catalog.Application.Commands.Products;
using EFO.Catalog.Domain.Categories;
using EFO.Catalog.Domain.Products;
using EventOutcomes;
using Xunit;

namespace EFO.Catalog.Tests.tests_for_moving_product_to_category;

public class given_product_and_category
{
    private readonly Test _test;
    private readonly Guid _productId;
    private readonly Guid _categoryId;

    public given_product_and_category()
    {
        _productId = Guid.NewGuid();
        _categoryId = Guid.NewGuid();

        _test = Test.ForMany()
            .Given(_productId, new ProductIntroduced(_productId))
            .Given(_categoryId, new CategoryAdded(_categoryId));
    }

    [Fact]
    public async Task when_MoveProductToCategory_then_product_moved_to_category()
    {
        _test
            .When(new MoveProductToCategory(_productId, _categoryId))
            .Then(_productId, new ProductMovedToCategory(_productId, _categoryId));

        await _test.TestAsync();
    }
}
