// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands.Products;
using EFO.Sales.Domain;
using EFO.Sales.Domain.Products;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_pricing_product;

public class given_priced_product_for_two_thresholds
{
    private readonly Test _test;
    private readonly Guid _productId;
    private readonly int _firstThresholdQuantity;
    private readonly int _secondThresholdQuantity;
    private readonly decimal _firstThresholdPrice;
    private readonly decimal _secondThresholdPrice;

    public given_priced_product_for_two_thresholds()
    {
        _productId = Guid.NewGuid();
        _firstThresholdQuantity = 50;
        _secondThresholdQuantity = 500;
        _firstThresholdPrice = 100m;
        _secondThresholdPrice = 80m;

        _test = Test.For(_productId)
            .Given(
                new ProductIntroduced(_productId),
                new ProductPriced(_productId, _firstThresholdQuantity, _firstThresholdPrice),
                new ProductPriced(_productId, _secondThresholdQuantity, _secondThresholdPrice));
    }

    private int QuantityLowerThanFirstThreshold => _firstThresholdQuantity - 1;
    private int QuantityBetweenFirstAndSecondThreshold => (_firstThresholdQuantity + _secondThresholdQuantity) / 2;
    private int QuantityHigherThanSecondThreshold => _secondThresholdQuantity + 1;
    private decimal PriceLowerThanFirstThreshold => _firstThresholdPrice - 1m;
    private decimal PriceHigherThanFirstThreshold => _firstThresholdPrice + 1m;
    private decimal PriceLowerThanSecondThreshold => _secondThresholdPrice - 1m;
    private decimal PriceHigherThanSecondThreshold => _secondThresholdPrice + 1m;
    private decimal PriceBetweenFirstAndSecondThreshold => (_firstThresholdPrice + _secondThresholdPrice) / 2;

    [Fact]
    public async Task price_for_quantity_lower_than_first_threshold_shall_throw_exception()
    {
        await _test
            .ThenAggregate<Product>(_productId, p =>
            {
                try
                {
                    p.Prices.GetUnitPriceForQuantity(QuantityLowerThanFirstThreshold);
                    return false;
                }
                catch
                {
                    return true;
                }
            })
            .TestAsync();
    }

    [Fact]
    public async Task price_for_quantity_between_first_and_second_threshold_shall_return_price_from_first_threshold()
    {
        await _test
            .ThenAggregate<Product>(_productId, p => p.Prices.GetUnitPriceForQuantity(QuantityBetweenFirstAndSecondThreshold) == _firstThresholdPrice)
            .TestAsync();
    }

    [Fact]
    public async Task price_for_quantity_higher_than_second_threshold_shall_return_price_from_second_threshold()
    {
        await _test
            .ThenAggregate<Product>(_productId, p => p.Prices.GetUnitPriceForQuantity(QuantityHigherThanSecondThreshold) == _secondThresholdPrice)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_lower_than_first_threshold_and_price_higher_then_product_priced()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityLowerThanFirstThreshold, PriceHigherThanFirstThreshold))
            .Then(new ProductPriced(_productId, QuantityLowerThanFirstThreshold, PriceHigherThanFirstThreshold))
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_lower_than_first_threshold_and_price_the_same_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityLowerThanFirstThreshold, _firstThresholdPrice))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForLowerQuantityThresholdMustBeHigher)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_lower_than_first_threshold_and_price_lower_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityLowerThanFirstThreshold, PriceLowerThanFirstThreshold))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForLowerQuantityThresholdMustBeHigher)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_same_as_first_threshold_and_price_higher_than_second_threshold_then_product_priced()
    {
        await _test
            .When(new PriceProduct(_productId, _firstThresholdQuantity, PriceHigherThanSecondThreshold))
            .Then(new ProductPriced(_productId, _firstThresholdQuantity, PriceHigherThanSecondThreshold))
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_same_as_first_threshold_and_price_same_as_second_threshold_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, _firstThresholdQuantity, _secondThresholdPrice))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForLowerQuantityThresholdMustBeHigher)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_same_as_first_threshold_and_price_lower_as_second_threshold_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, _firstThresholdQuantity, PriceLowerThanSecondThreshold))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForLowerQuantityThresholdMustBeHigher)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_between_first_and_second_threshold_and_price_between_first_and_second_threshold_then_product_priced()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityBetweenFirstAndSecondThreshold, PriceBetweenFirstAndSecondThreshold))
            .Then(new ProductPriced(_productId, QuantityBetweenFirstAndSecondThreshold, PriceBetweenFirstAndSecondThreshold))
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_between_first_and_second_threshold_and_price_higher_than_first_threshold_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityBetweenFirstAndSecondThreshold, PriceHigherThanFirstThreshold))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForHigherQuantityThresholdMustBeLower)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_between_first_and_second_threshold_and_price_lower_than_second_threshold_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityBetweenFirstAndSecondThreshold, PriceLowerThanSecondThreshold))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForLowerQuantityThresholdMustBeHigher)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_same_as_second_threshold_and_price_lower_than_first_threshold_then_product_priced()
    {
        await _test
            .When(new PriceProduct(_productId, _secondThresholdQuantity, PriceLowerThanFirstThreshold))
            .Then(new ProductPriced(_productId, _secondThresholdQuantity, PriceLowerThanFirstThreshold))
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_same_as_second_threshold_and_price_same_as_first_threshold_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, _secondThresholdQuantity, _firstThresholdPrice))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForHigherQuantityThresholdMustBeLower)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_same_as_second_threshold_and_price_higher_than_first_threshold_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, _secondThresholdQuantity, PriceHigherThanFirstThreshold))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForHigherQuantityThresholdMustBeLower)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_higher_than_second_threshold_and_price_lower_then_product_priced()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityHigherThanSecondThreshold, PriceLowerThanSecondThreshold))
            .Then(new ProductPriced(_productId, QuantityHigherThanSecondThreshold, PriceLowerThanSecondThreshold))
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_higher_than_second_threshold_and_price_the_same_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityHigherThanSecondThreshold, _secondThresholdPrice))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForHigherQuantityThresholdMustBeLower)
            .TestAsync();
    }

    [Fact]
    public async Task when_PriceProduct_for_threshold_higher_than_second_threshold_and_price_lower_then_exception_thrown()
    {
        await _test
            .When(new PriceProduct(_productId, QuantityHigherThanSecondThreshold, PriceHigherThanSecondThreshold))
            .ThenDomainExceptionWith(SalesDomainErrors.PriceForHigherQuantityThresholdMustBeLower)
            .TestAsync();
    }
}
