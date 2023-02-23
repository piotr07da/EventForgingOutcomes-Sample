// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands;
using EFO.Sales.Domain;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_introducing_product;

public class given_no_product
{
    private readonly Test _test;
    private readonly Guid _productId;

    public given_no_product()
    {
        _productId = Guid.NewGuid();
        _test = Test.For(_productId);
    }

    [Fact]
    public async Task when_IntroduceProduct_then_product_introduced()
    {
        var productName = "Product Name 123";

        _test
            .When(new IntroduceProduct(_productId, productName))
            .ThenInOrder(
                new ProductIntroduced(_productId),
                new ProductNamed(_productId, productName));

        await _test.TestAsync();
    }
}
