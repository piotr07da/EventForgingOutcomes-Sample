// ReSharper disable InconsistentNaming

using EFO.Sales.Application.Commands.Orders;
using EFO.Sales.Domain.Customers;
using EFO.Sales.Domain.Orders;
using EventForging;
using EventOutcomes;
using Xunit;

namespace EFO.Sales.Tests.tests_for_starting_order;

public sealed class given_no_existing_order
{
    private readonly Guid _orderId;
    private readonly Guid _customerId;
    private readonly Test _test;

    public given_no_existing_order()
    {
        _orderId = Guid.NewGuid();
        _customerId = Guid.NewGuid();
        _test = Test
            .For(_orderId)
            .Given() // no events means not existing order
            .Given(_customerId, new CustomerRegistered(_customerId));
    }

    [Fact]
    public async Task when_StartOrder_then_order_started_and_customer_assigned()
    {
        _test
            .When(new StartOrder(_orderId, _customerId))
            .ThenInOrder(
                new OrderStarted(_orderId),
                new OrderCustomerAssigned(_orderId, _customerId));

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_StartOrder_for_not_existing_customer_then_exception_thrown()
    {
        var notExistingCustomerId = Guid.NewGuid();

        _test
            .When(new StartOrder(_orderId, notExistingCustomerId))
            .ThenException<AggregateNotFoundEventForgingException>();

        await _test.TestAsync();
    }
}
