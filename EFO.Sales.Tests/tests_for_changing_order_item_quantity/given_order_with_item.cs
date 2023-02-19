// ReSharper disable InconsistentNaming

using EFO.Sales.Application;
using EFO.Sales.Domain;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_changing_order_item_quantity;

public class given_order_with_item
{
    private readonly Test _test;
    private readonly Guid _orderId;
    private readonly Guid _orderItemId;
    private readonly Guid _productId;
    private readonly (decimal UnitPrice, int QuantityThreshold)[] _productPrices;

    public given_order_with_item()
    {
        _orderId = Guid.NewGuid();
        _orderItemId = Guid.NewGuid();
        _productId = Guid.NewGuid();
        _productPrices = new[]
        {
            (20m, 1),
            (18m, 30),
            (16m, 60),
        };

        _test = Test.ForMany()
            .Given(
                _orderId,
                new OrderStarted(_orderId),
                new OrderItemAdded(_orderId, _orderItemId, _productId),
                new OrderItemQuantityChanged(_orderId, _orderItemId, 45),
                new OrderPriced(_orderId, _productPrices[1].UnitPrice * 45))
            .Given(
                _productId,
                new ProductIntroduced(_productId),
                new ProductPriced(_productId, _productPrices[0].QuantityThreshold, _productPrices[0].UnitPrice),
                new ProductPriced(_productId, _productPrices[1].QuantityThreshold, _productPrices[1].UnitPrice),
                new ProductPriced(_productId, _productPrices[2].QuantityThreshold, _productPrices[2].UnitPrice));
    }

    [Fact]
    public async Task when_ChangeOrderItemQuantity_within_same_pricing_quantity_threshold_then_item_quantity_changed_and_item_priced_holding_the_same_price_and_order_priced()
    {
        var newQuantity = 30;
        var expectedPriceQuantityThreshold = _productPrices[1];
        var expectedNewPrice = expectedPriceQuantityThreshold.UnitPrice * newQuantity;

        _test
            .When(new ChangeOrderItemQuantity(_orderId, _orderItemId, newQuantity))
            .ThenInOrder(_orderId,
                new OrderItemQuantityChanged(_orderId, _orderItemId, newQuantity),
                new OrderItemPriced(_orderId, _orderItemId, expectedNewPrice),
                new OrderPriced(_orderId, expectedNewPrice));

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_ChangeOrderItemQuantity_for_lower_pricing_quantity_threshold_then_item_quantity_changed_and_item_priced_for_lower_quantity_threshold_level_and_order_priced()
    {
        var newQuantity = 29;
        var expectedPriceQuantityThreshold = _productPrices[0];
        var expectedNewPrice = expectedPriceQuantityThreshold.UnitPrice * newQuantity;

        _test
            .When(new ChangeOrderItemQuantity(_orderId, _orderItemId, newQuantity))
            .ThenInOrder(_orderId,
                new OrderItemQuantityChanged(_orderId, _orderItemId, newQuantity),
                new OrderItemPriced(_orderId, _orderItemId, expectedNewPrice),
                new OrderPriced(_orderId, expectedNewPrice));

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_ChangeOrderItemQuantity_for_higher_pricing_quantity_threshold_then_item_quantity_changed_and_item_priced_for_higher_quantity_threshold_level_and_order_priced()
    {
        var newQuantity = 60;
        var expectedPriceQuantityThreshold = _productPrices[2];
        var expectedNewPrice = expectedPriceQuantityThreshold.UnitPrice * newQuantity;

        _test
            .When(new ChangeOrderItemQuantity(_orderId, _orderItemId, newQuantity))
            .ThenInOrder(_orderId,
                new OrderItemQuantityChanged(_orderId, _orderItemId, newQuantity),
                new OrderItemPriced(_orderId, _orderItemId, expectedNewPrice),
                new OrderPriced(_orderId, expectedNewPrice));

        await _test.TestAsync();
    }
}
