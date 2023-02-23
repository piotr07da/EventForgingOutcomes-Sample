// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands;
using EFO.Sales.Domain;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_pricing_product;

public class given_priced_product
{
    private readonly Test _test;
    private readonly Guid _productId;

    public given_priced_product()
    {
        _productId = Guid.NewGuid();

        _test = Test.For(_productId)
            .Given(new ProductIntroduced(_productId))
            .Given(new ProductPriced(_productId, 50, 100m));
    }

    [Fact]
    public async Task when_PriceProduct_for_the_same_threshold_with_same_price_then_product_priced()
    {
        _test
            .When(new PriceProduct(_productId, 50, 200m))
            .Then(new ProductPriced(_productId, 50, 200m));
        await _test.TestAsync();
    }

    [Theory]
    [InlineData(50)]
    [InlineData(350)]
    public async Task when_PriceProduct_for_the_same_threshold_with_different_price_then_product_priced(decimal differentPrice)
    {
        _test
            .When(new PriceProduct(_productId, 50, differentPrice))
            .Then(new ProductPriced(_productId, 50, differentPrice));
        await _test.TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_lower_threshold_with_the_higher_price_then_product_priced()
    {
        _test
            .When(new PriceProduct(_productId, 49, 250m))
            .Then(new ProductPriced(_productId, 49, 250m));
        await _test.TestAsync();
    }

    [Theory]
    [InlineData(100)]
    [InlineData(99)]
    public async Task when_PriceProduct_for_lower_threshold_with_the_same_or_lower_price_then_exception_thrown(decimal sameOrLowerPrice)
    {
        _test
            .When(new PriceProduct(_productId, 49, sameOrLowerPrice))
            .ThenDomainExceptionWith(DomainErrors.PriceForLowerQuantityThresholdMustBeHigher);
        await _test.TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_higher_threshold_with_the_lower_price_then_product_priced()
    {
        _test
            .When(new PriceProduct(_productId, 51, 50m))
            .Then(new ProductPriced(_productId, 51, 50m));
        await _test.TestAsync();
    }

    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    public async Task when_PriceProduct_for_higher_threshold_with_the_same_or_higher_price_then_exception_thrown(decimal sameOrHigherPrice)
    {
        _test
            .When(new PriceProduct(_productId, 51, sameOrHigherPrice))
            .ThenDomainExceptionWith(DomainErrors.PriceForHigherQuantityThresholdMustBeLower);
        await _test.TestAsync();
    }
}
