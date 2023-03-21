using EFO.Sales.Application.Commands.Customers;
using EFO.Sales.Domain.Customers;
using EventOutcomes;
using Xunit;

// ReSharper disable InconsistentNaming

namespace EFO.Sales.Tests.tests_for_registering_customer;

public class given_no_customer
{
    private readonly Test _test;
    private readonly Guid _customerId;

    public given_no_customer()
    {
        _customerId = Guid.NewGuid();

        _test = Test.For(_customerId);
    }

    [Fact]
    public async Task when_RegisterCustomer_then_customer_registered()
    {
        await _test
            .When(new RegisterCustomer(_customerId))
            .Then(new CustomerRegistered(_customerId))
            .TestAsync();
    }
}
