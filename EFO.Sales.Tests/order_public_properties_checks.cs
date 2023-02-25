// ReSharper disable InconsistentNaming

using EFO.Sales.Domain.Orders;
using EFO.Sales.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests;

public class order_public_properties_checks
{
    private readonly Test _test;
    private readonly Guid _orderId;

    public order_public_properties_checks()
    {
        _orderId = Guid.NewGuid();
        _test = Test.For(_orderId);
    }

    [Fact]
    public async Task given_order_started_then_order_Id_set()
    {
        await _test
            .Given(new OrderStarted(_orderId))
            .ThenAggregate<Order>(_orderId, order => order.Id.Value == _orderId)
            .TestAsync();
    }

    [Fact]
    public async Task given_order_item_added_then_order_Items_contains_added_item()
    {
        var orderItemId = Guid.NewGuid();
        var orderItemProductId = Guid.NewGuid();

        await _test
            .Given(new OrderStarted(_orderId), new OrderItemAdded(_orderId, orderItemId, orderItemProductId))
            .ThenAggregate<Order>(_orderId, order => order.Items.Contains(orderItemId) && order.Items.Find(orderItemId).Id.Value == orderItemId)
            .TestAsync();
    }

    [Fact]
    public async Task given_order_item_added_and_its_quantity_changed_then_added_order_item_Quantity_is_set()
    {
        var orderItemId = Guid.NewGuid();
        var orderItemProductId = Guid.NewGuid();

        await _test
            .Given(new OrderStarted(_orderId), new OrderItemAdded(_orderId, orderItemId, orderItemProductId), new OrderItemQuantityChanged(_orderId, orderItemId, 5463))
            .ThenAggregate<Order>(_orderId, order => order.Items.Contains(orderItemId) && order.Items.Find(orderItemId).Quantity == 5463)
            .TestAsync();
    }
}
