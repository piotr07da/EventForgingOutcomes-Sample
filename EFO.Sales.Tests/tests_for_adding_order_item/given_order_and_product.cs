// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands;
using EFO.Sales.Domain;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_adding_order_item;

public class given_order_and_product
{
    private readonly Test _test;
    private readonly Guid _orderId;
    private readonly Guid _orderItemId;
    private readonly Guid _productId;

    public given_order_and_product()
    {
        _orderId = Guid.NewGuid();
        _orderItemId = Guid.NewGuid();
        _productId = Guid.NewGuid();
        _test = Test
            .ForMany()
            .Given(_orderId, new OrderStarted(_orderId))
            .Given(_productId, new ProductIntroduced(_productId));
    }

    [Fact]
    public async Task when_AddOrderItem_then_order_item_added_and_quantity_changed()
    {
        var quantity = 27;

        _test
            .Given(_productId, new ProductPriced(_productId, 1, 100m))
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, quantity))
            .ThenInOrder(_orderId,
                new OrderItemAdded(_orderId, _orderItemId, _productId),
                new OrderItemQuantityChanged(_orderId, _orderItemId, quantity))
            .ThenAny(_orderId);

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_AddOrderItem_with_quantity_lower_than_first_product_price_quantity_threshold_then_exception_thrown()
    {
        var quantity = 14;

        _test
            .Given(_productId, new ProductPriced(_productId, 15, 100m))
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, quantity))
            .ThenDomainExceptionWith(DomainErrors.QuantityToLowForPricing);

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_AddOrderItem_with_quantity_between_first_and_second_product_price_quantity_threshold_then_order_item_priced_for_first_quantity_threshold()
    {
        var quantity = 27;
        var firstThresholdUnitPrice = 100m;
        var secondThresholdUnitPrice = 90m;
        var expectedItemPrice = quantity * firstThresholdUnitPrice;

        _test
            .Given(_productId, new ProductPriced(_productId, 15, firstThresholdUnitPrice))
            .Given(_productId, new ProductPriced(_productId, 30, secondThresholdUnitPrice))
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, quantity))
            .ThenAny(_orderId)
            .Then(_orderId, new OrderItemPriced(_orderId, _orderItemId, expectedItemPrice))
            .ThenAny(_orderId);

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_AddOrderItem_with_quantity_above_second_product_price_quantity_threshold_then_order_item_priced_for_second_quantity_threshold()
    {
        var quantity = 37;
        var firstThresholdUnitPrice = 100m;
        var secondThresholdUnitPrice = 90m;
        var expectedItemPrice = quantity * secondThresholdUnitPrice;

        _test
            .Given(_productId, new ProductPriced(_productId, 15, firstThresholdUnitPrice))
            .Given(_productId, new ProductPriced(_productId, 30, secondThresholdUnitPrice))
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, quantity))
            .ThenAny(_orderId)
            .Then(_orderId, new OrderItemPriced(_orderId, _orderItemId, expectedItemPrice))
            .ThenAny(_orderId);

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_AddOrderItem_then_order_priced_the_same_as_order_item()
    {
        var quantity = 30;
        var unitPrice = 100m;
        var expectedItemPrice = quantity * unitPrice;

        _test
            .Given(_productId, new ProductPriced(_productId, 1, unitPrice))
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, quantity))
            .ThenAny(_orderId)
            .ThenInOrder(_orderId,
                new OrderItemPriced(_orderId, _orderItemId, expectedItemPrice),
                new OrderPriced(_orderId, expectedItemPrice))
            .ThenAny(_productId);

        await _test.TestAsync();
    }
}
