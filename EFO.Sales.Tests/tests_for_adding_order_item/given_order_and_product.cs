// ReSharper disable InconsistentNaming

using EFO.Sales.Application;
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
    public async Task when_AddOrderItem()
    {
        _test
            .When(new AddOrderItem(_orderId, _orderItemId, _productId, 27))
            .Then(_orderItemId, new OrderItemAdded(_orderId, _orderItemId, _productId, 27));

        await _test.TestAsync();
    }
}
