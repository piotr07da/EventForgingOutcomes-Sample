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

    private readonly int _existingThresholdQuantity;
    private readonly decimal _existingThresholdPrice;

    public given_priced_product()
    {
        _productId = Guid.NewGuid();
        _existingThresholdQuantity = 50;
        _existingThresholdPrice = 100m;

        _test = Test.For(_productId)
            .Given(
                new ProductIntroduced(_productId),
                new ProductPriced(_productId, _existingThresholdQuantity, _existingThresholdPrice));
    }

    private int QuantityLowerThanExistingThreshold => _existingThresholdQuantity - 1;
    private int QuantityHigherThanExistingThreshold => _existingThresholdQuantity + 1;

    private decimal PriceLowerThanExistingThreshold => _existingThresholdPrice - 1m;
    private decimal PriceHigherThanExistingThreshold => _existingThresholdPrice + 1m;

    [Theory]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(350)]
    public async Task when_PriceProduct_for_the_same_threshold_then_product_priced(decimal newPrice)
    {
        _test
            .When(new PriceProduct(_productId, _existingThresholdQuantity, newPrice))
            .Then(new ProductPriced(_productId, _existingThresholdQuantity, newPrice));
        await _test.TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_lower_threshold_with_the_higher_price_then_product_priced()
    {
        _test
            .When(new PriceProduct(_productId, QuantityLowerThanExistingThreshold, PriceHigherThanExistingThreshold))
            .Then(new ProductPriced(_productId, QuantityLowerThanExistingThreshold, PriceHigherThanExistingThreshold));
        await _test.TestAsync();
    }

    [Theory]
    [InlineData(100)]
    [InlineData(99)]
    public async Task when_PriceProduct_for_lower_threshold_with_the_same_or_lower_price_then_exception_thrown(decimal sameOrLowerPrice)
    {
        _test
            .When(new PriceProduct(_productId, QuantityLowerThanExistingThreshold, sameOrLowerPrice))
            .ThenDomainExceptionWith(DomainErrors.PriceForLowerQuantityThresholdMustBeHigher);
        await _test.TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_higher_threshold_with_the_lower_price_then_product_priced()
    {
        _test
            .When(new PriceProduct(_productId, QuantityHigherThanExistingThreshold, PriceLowerThanExistingThreshold))
            .Then(new ProductPriced(_productId, QuantityHigherThanExistingThreshold, PriceLowerThanExistingThreshold));
        await _test.TestAsync();
    }

    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    public async Task when_PriceProduct_for_higher_threshold_with_the_same_or_higher_price_then_exception_thrown(decimal sameOrHigherPrice)
    {
        _test
            .When(new PriceProduct(_productId, QuantityHigherThanExistingThreshold, sameOrHigherPrice))
            .ThenDomainExceptionWith(DomainErrors.PriceForHigherQuantityThresholdMustBeLower);
        await _test.TestAsync();
    }
}
