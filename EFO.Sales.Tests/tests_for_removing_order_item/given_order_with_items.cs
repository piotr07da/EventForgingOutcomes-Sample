// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands.Orders;
using EFO.Sales.Domain.Orders;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_removing_order_item;

public class given_order_with_items
{
    private readonly Test _test;
    private readonly Guid _orderId;
    private readonly Guid _firstItemId;
    private readonly decimal _firstItemPrice;
    private readonly Guid _secondItemId;
    private readonly decimal _secondItemPrice;
    private readonly Guid _thirdItemId;
    private readonly decimal _thirdItemPrice;

    public given_order_with_items()
    {
        _orderId = Guid.NewGuid();
        _firstItemId = Guid.NewGuid();
        _firstItemPrice = 598m;
        _secondItemId = Guid.NewGuid();
        _secondItemPrice = 1983m;
        _thirdItemId = Guid.NewGuid();
        _thirdItemPrice = 234m;

        _test = Test.For(_orderId)
            .Given(
                new OrderStarted(_orderId),
                new OrderItemAdded(_orderId, _firstItemId, Guid.NewGuid()),
                new OrderItemPriced(_orderId, _firstItemId, _firstItemPrice),
                new OrderItemAdded(_orderId, _secondItemId, Guid.NewGuid()),
                new OrderItemPriced(_orderId, _secondItemId, _secondItemPrice),
                new OrderItemAdded(_orderId, _thirdItemId, Guid.NewGuid()),
                new OrderItemPriced(_orderId, _thirdItemId, _thirdItemPrice));
    }

    [Fact]
    public async Task when_RemoveOrderItem_for_first_item_then_first_item_removed_and_order_priced_for_two_remaining_items()
    {
        await _test
            .When(new RemoveOrderItem(_orderId, _firstItemId))
            .ThenInOrder(
                new OrderItemRemoved(_orderId, _firstItemId),
                new OrderPriced(_orderId, _secondItemPrice + _thirdItemPrice))
            .TestAsync();
    }

    [Fact]
    public async Task when_RemoveOrderItem_for_second_item_then_second_item_removed_and_order_priced_for_two_remaining_items()
    {
        await _test
            .When(new RemoveOrderItem(_orderId, _secondItemId))
            .ThenInOrder(
                new OrderItemRemoved(_orderId, _secondItemId),
                new OrderPriced(_orderId, _firstItemPrice + _thirdItemPrice))
            .TestAsync();
    }

    [Fact]
    public async Task when_RemoveOrderItem_for_third_item_then_third_item_removed_and_order_priced_for_two_remaining_items()
    {
        await _test
            .When(new RemoveOrderItem(_orderId, _thirdItemId))
            .ThenInOrder(
                new OrderItemRemoved(_orderId, _thirdItemId),
                new OrderPriced(_orderId, _firstItemPrice + _secondItemPrice))
            .TestAsync();
    }
}
