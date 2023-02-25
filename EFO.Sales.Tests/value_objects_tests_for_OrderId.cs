// ReSharper disable InconsistentNaming

using EFO.Sales.Domain;
using EFO.Sales.Domain.Orders;
using EFO.Shared.Domain;
using Xunit;

namespace EFO.Sales.Tests;

public sealed class value_objects_tests_for_OrderId
{
    [Fact]
    public void OrderId_cannot_be_empty()
    {
        var ex = Assert.Throws<DomainException>(() => OrderId.FromValue(Guid.Empty));
        Assert.Contains(ex.Errors, e => e.Name == SalesDomainErrors.OrderIdCannotBeEmpty);
    }
}
