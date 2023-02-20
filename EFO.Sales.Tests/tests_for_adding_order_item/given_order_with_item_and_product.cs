// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands;
using EFO.Sales.Domain;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_adding_order_item;

public class given_order_with_item_and_product
{
    private readonly Test _test;
    private readonly Guid _orderId;
    private readonly Guid _orderItemId;
    private readonly Guid _productId;
    private readonly decimal _alreadyAddedItemPrice;
    private readonly decimal _productUnitPrice;

    public given_order_with_item_and_product()
    {
        _orderId = Guid.NewGuid();
        _orderItemId = Guid.NewGuid();
        _productId = Guid.NewGuid();
        _alreadyAddedItemPrice = 1000m;
        _productUnitPrice = 25m;
        var alreadyAddedItemId = Guid.NewGuid();
        _test = Test
            .ForMany()
            .Given(_orderId,
                new OrderStarted(_orderId),
                new OrderItemAdded(_orderId, alreadyAddedItemId, Guid.NewGuid()),
                new OrderItemPriced(_orderId, alreadyAddedItemId, _alreadyAddedItemPrice))
            .Given(_productId,
                new ProductIntroduced(_productId),
                new ProductPriced(_productId, 1, _productUnitPrice));
    }

    [Fact]
    public async Task when_AddOrderItem_then_order_priced_as_sum_of_both_items_prices()
    {
        const int quantity = 17;

        var expectedOrderPrice = _alreadyAddedItemPrice + quantity * _productUnitPrice;

        _test
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, quantity))
            .ThenAny(_orderId)
            .Then(_orderId, new OrderPriced(_orderId, expectedOrderPrice))
            .ThenAny(_orderId);

        await _test.TestAsync();
    }
}
