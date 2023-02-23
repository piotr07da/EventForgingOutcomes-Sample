// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands;
using EFO.Sales.Domain;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_pricing_product;

public class given_product
{
    private readonly Test _test;
    private readonly Guid _productId;

    public given_product()
    {
        _productId = Guid.NewGuid();

        _test = Test.For(_productId)
            .Given(new ProductIntroduced(_productId));
    }

    [Fact]
    public async Task and_given_product_not_priced_when_PriceProduct_then_product_priced()
    {
        _test
            .When(new PriceProduct(_productId, 1, 100m))
            .Then(new ProductPriced(_productId, 1, 100m));
        await _test.TestAsync();
    }
}
